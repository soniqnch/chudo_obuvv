using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace WinFormsApp1
{


    public partial class CatalogForm : Form
    {
        private readonly int userId;
        private bool closeAfterOrder;
        public string? UserRole { get; }
        private readonly List<CatalogEntry> products = new();
        private readonly List<OrderLine> orderLines = new();
        private sealed record CatalogEntry(
            ProductCardControl Card, string Name, string Description,
            int CategoryId, decimal Price);

        private sealed record CategoryOption(int? Id, string Name)
        { 
            public override string ToString() => Name; 
        }






        public CatalogForm()
        {
            InitializeComponent();
            AppStyle.Apply(this, btnOrder);
            flpProduct.BackColor = AppStyle.Secondary;
            btnOrder.Visible = false;

        }
        public CatalogForm(string displayName, string? role, int userId) : this()
        {
            lblUserName.Text = displayName;
            UserRole = role?.Trim();
            this.userId = userId;
            btnOrder.Visible = userId > 0;
            btnOrder.Enabled = userId > 0;
            btnOrderList.Visible = userId > 0 &&
            UserRole is "Administrator" or "Администратор" or "Manager" or "Менеджер";
            btnOrderList.Enabled = true;


        }

        internal static CatalogForm ForGuest()
        {
            var catalog = new CatalogForm();
            catalog.lblUserName.Text = "Гость";
            return catalog;
        }
        internal static CatalogForm ForClient(int clientId, string clientName)
        {
            var catalog = new CatalogForm(clientName, null, clientId);
            catalog.Text = "Чудо Обувь - новый заказ для клиента";
            catalog.closeAfterOrder = true;
            return catalog;
        }

        private void LoadProducts(bool loadCategories = true)
        {
            const string sql = ProductDetailsForm.ProductQuery + " ORDER BY p.product_name, p.product_id";

            using var connection = Database.CreateConnection();
            connection.Open();
            if (loadCategories)
                LoadCategories(connection);
            using var command = new MySqlCommand(sql, connection);
            ProductDetailsForm.SetMonthParameters(command);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var card = new ProductCardControl();
                decimal finalPrice = ProductDetailsForm.FillCard(card, reader);
                int productId = reader.GetInt32(reader.GetOrdinal("product_id"));
                if (userId > 0)
                    card.EnableSelection((_, _) => OpenProduct(productId));
                string productName = reader.GetString(1);

                int descriptionColumn = reader.GetOrdinal("description");
                string description = reader.IsDBNull(descriptionColumn)
                    ? string.Empty
                    : reader.GetString(descriptionColumn);
                products.Add(new CatalogEntry(card, productName, description,
                    reader.GetInt32(reader.GetOrdinal("category_id")), finalPrice));
                flpProduct.Controls.Add(card);
            }
        }
        private void OpenProduct(int productId)
        {
            try
            {
                using var details = new ProductDetailsForm(productId);
                if (details.ShowDialog(this) == DialogResult.OK && details.SelectedLine is OrderLine line)
                {
                    int index = orderLines.FindIndex(item => item.StockItemId == line.StockItemId);
                    if (index >= 0)
                    {
                        int previousQuantity = orderLines[index].Quantity;
                        if (line.Quantity > line.Available - previousQuantity)
                        {
                            MessageBox.Show(this, "С учётом позиций в заказе доступных пар недостаточно. Уменьшите количество.",
                                "Недостаточно товара", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        orderLines[index] = line with { Quantity = previousQuantity + line.Quantity };
                    }
                    else
                    {
                        orderLines.Add(line);
                    }

                    UpdateOrderButton();
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Не удалось загрузить товар. Проверьте соединение и попробуйте ещё раз.",
                    "Ошибка загрузки товара", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateOrderButton()
        {
            btnOrder.Text = $"Заказ ({orderLines.Count})";
        }
        private void btnOrder_Click(object? sender, EventArgs e)
        {
            if (userId <= 0)
                return;

            using var order = new OrderForm(userId, orderLines);
            DialogResult result = order.ShowDialog(this);
            UpdateOrderButton();
            if (result != DialogResult.OK)
                return;

            if (closeAfterOrder)
            {
                DialogResult = DialogResult.OK;
                return;
            }
            ReloadProducts();
        }

 
        private void ReloadProducts()
        {
            foreach (CatalogEntry product in products)
                product.Card.Dispose();
            products.Clear();

            try
            {
                LoadProducts(loadCategories: false);
                ApplyCatalogOptions();
            }
            catch (MySqlException)
            {
                MessageBox.Show(this, "Чтобы обновить остатки, откройте каталог снова.",
                    "Не удалось обновить каталог", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void LoadCategories(MySqlConnection connection)
        {
            const string sql = "SELECT category_id, category_name FROM categories ORDER BY category_name"; 
            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            cboCategory.Items.Add(new CategoryOption(null, "Все категории"));
            while (reader.Read())
            { 
                cboCategory.Items.Add(new CategoryOption(reader.GetInt32(0), reader.GetString(1)));
            }
        }
        private void CatalogOptionsChanged(object? sender, EventArgs e)
        { 
            ApplyCatalogOptions(); 
        }
        private void ApplyCatalogOptions()
        {
            string search = txtSearch.Text.Trim();
            int? categoryId = (cboCategory.SelectedItem as CategoryOption)?.Id;
            IEnumerable<CatalogEntry> orderedProducts = cboPriceSort.SelectedIndex switch
            { 
                1 => products.OrderBy(product => product.Price),
                2 => products.OrderByDescending(product => product.Price),
                _ => products 
            };
            flpProduct.SuspendLayout(); try
            {
                int index = 0; foreach (CatalogEntry product in orderedProducts)
                {
                    bool categoryMatches = categoryId == null || product.CategoryId == categoryId;
                    bool searchMatches = search.Length == 0
                        || product.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase)
                        || product.Description.Contains(search, StringComparison.CurrentCultureIgnoreCase); 
                    flpProduct.Controls.SetChildIndex(product.Card, index++);
                    product.Card.Visible = categoryMatches && searchMatches;
                }
            }
            finally { flpProduct.ResumeLayout(); }
            flpProduct.AutoScrollPosition = Point.Empty;
        }




        private void btnBack_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void btnOrderList_Click(object? sender, EventArgs e)
        {
            using var orders = new OrderListForm(UserRole is "Administrator" or "Администратор");
            orders.ShowDialog(this);
            if (orders.HasChanges)
                ReloadProducts();
        }



        private void CatalogForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadProducts();
                cboCategory.SelectedIndex = 0;
                cboPriceSort.SelectedIndex = 0;
            }
            catch (MySqlException)
            {
                MessageBox.Show(
                    "Не удалось загрузить товары. Проверьте подключение к MySQL и названия столбцов.",
                    "Ошибка загрузки каталога",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void lblSearch_Click(object sender, EventArgs e)
        {

        }

        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lblUserName_Click(object sender, EventArgs e)
        {

        }

        private void cboPriceSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
