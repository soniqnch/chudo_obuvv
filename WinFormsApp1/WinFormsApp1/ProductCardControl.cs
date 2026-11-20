using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class ProductCardControl : UserControl
    {
        public ProductCardControl()
        {
            InitializeComponent();
        }
        public void SetProduct(
            string manufacturer, string productName, string category, 
            string composition, int totalQuantity, decimal finalPrice, string? imageFile)
        {
            lblProductTitle.Text = $"{manufacturer} | {productName}";
            lblCategory.Text = $"Категория: {category}";
            lblQuantity.Text = totalQuantity > 5
                ? "Количество: много":
                "Количество: мало"; 
            lblMaterial.Text = $"Состав: {composition}"; 
            lblPrice.Text = $"{finalPrice:N2} ₽"; 
            SetProductImage(imageFile); 

            BackColor = totalQuantity <= 3 
                ? Color.FromArgb(255, 128, 128)
                : Color.White;
        }
        internal void EnableSelection(EventHandler handler)
        {
            Cursor = Cursors.Hand;
            Click += handler;
            foreach (Control child in Controls)
            {
                child.Cursor = Cursors.Hand;
                child.Click += handler;
            }
        }



        private void SetProductImage(string? imageFile)
        {
            string resources = Path.Combine(AppContext.BaseDirectory, "Resources");
            Image? photo = null;
            if (!string.IsNullOrWhiteSpace(imageFile))
            {
                string fileName = Path.GetFileName(imageFile.Trim());
                if (string.Equals(Path.GetExtension(fileName), ".png",
                        StringComparison.OrdinalIgnoreCase))
                {
                    photo = ReadImage(Path.Combine(resources, "Images", fileName));
                }
            }
            picProduct.Image?.Dispose(); 
            picProduct.Image = photo ?? ReadImage(Path.Combine(resources, "picture.png"));
        }
        internal static void SetMonthParameters(MySqlCommand command)
        {
            DateTime today = DateTime.Today;
            DateTime currentMonthStart = new(today.Year, today.Month, 1);
            command.Parameters.AddWithValue("@previousMonthStart", currentMonthStart.AddMonths(-1));
            command.Parameters.AddWithValue("@currentMonthStart", currentMonthStart);
        }
        internal static Image? ReadImage(string path)
        {
            try
            {
                using var stream = File.OpenRead(path);
                using var original = Image.FromStream(stream);
                return new Bitmap(original);
            }

            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
            {
                return null;
            }
        }
        


        private void lblCategory_Click(object sender, EventArgs e)
        {

        }

        private void lblMaterial_Click(object sender, EventArgs e)
        {

        }

        private void lblQuantity_Click(object sender, EventArgs e)
        {

        }
    }
}
