using MySqlConnector;
namespace WinFormsApp1;

internal sealed class ProductDetailsForm : Form
{
    internal const string ProductQuery = @"
SELECT
    m.manufacturer_name,
    p.product_name,
    c.category_name,
    COALESCE(p.composition, 'Не указан'),
    p.price,
    p.image_file,
    p.description,
    p.category_id,
    COALESCE((
        SELECT SUM(si.quantity)
        FROM stock_items AS si
        WHERE si.product_id = p.product_id
    ), 0) AS total_quantity,
    p.product_id,
    EXISTS (
        SELECT 1
        FROM order_items AS oi
        JOIN orders AS o ON o.order_id = oi.order_id
        JOIN stock_items AS si ON si.stock_item_id = oi.stock_item_id
        WHERE si.product_id = p.product_id
          AND o.order_date >= @previousMonthStart
          AND o.order_date < @currentMonthStart
    ) AS has_previous_month_orders
FROM products AS p
JOIN categories AS c ON c.category_id = p.category_id
JOIN manufacturers AS m ON m.manufacturer_id = p.manufacturer_id";

    private sealed record StockSize(int Id, decimal Size, int Quantity)
    {
        public override string ToString() => Size.ToString("0.##");
    }

    internal OrderLine? SelectedLine { get; private set; }
    public ProductDetailsForm(int productId)
    {
        Text = "Чудо Обувь - просмотр товара";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(830, 565);
        MinimumSize = new Size(846, 604);

        var card = new ProductCardControl { Dock = DockStyle.Fill };
        var description = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Dock = DockStyle.Fill
        };
        var sizeRange = new Label { AutoSize = true };
        var availableSizes = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 100 };
        var quantity = new NumericUpDown { Minimum = 1, Maximum = 1, Width = 80, Enabled = false };
        var availability = new Label { AutoSize = true, Padding = new Padding(8, 5, 0, 0) };
        var back = new Button { Text = "Назад", AutoSize = true, DialogResult = DialogResult.Cancel };
        CancelButton = back;
        var add = new Button { Text = "Добавить в заказ", AutoSize = true, Enabled = false };

        AppStyle.Apply(this, add);
        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, BackColor = AppStyle.Secondary };
    actions.Controls.AddRange(new Control[] { back, add });
        var selection = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        selection.Controls.AddRange(new Control[]
        {
            new Label { Text = "Размер", AutoSize = true, Padding = new Padding(0, 5, 0, 0) },
            availableSizes,
            new Label { Text = "Количество пар", AutoSize = true, Padding = new Padding(12, 5, 0, 0) },
            quantity,
            availability
        });
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 6,
            Padding = new Padding(12)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 191));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(card, 0, 0);
        layout.Controls.Add(new Label { Text = "Описание", AutoSize = true }, 0, 1);
        layout.Controls.Add(description, 0, 2);
        layout.Controls.Add(sizeRange, 0, 3);
        layout.Controls.Add(selection, 0, 4);
        layout.Controls.Add(actions, 0, 5);
        Controls.Add(layout);
        availableSizes.SelectedIndexChanged += (_, _) =>
        {
            if (availableSizes.SelectedItem is StockSize selected)
            {
                quantity.Value = 1;
                quantity.Maximum = selected.Quantity;
                quantity.Enabled = true;
                availability.Text = $"Доступно пар: {selected.Quantity}";
            }
        };
        string productName;
        decimal price;

        using var connection = Database.CreateConnection();
        connection.Open();
        using (var command = new MySqlCommand(ProductQuery + " WHERE p.product_id = @id", connection))
        {
            SetMonthParameters(command);
            command.Parameters.AddWithValue("@id", productId);
            using var reader = command.ExecuteReader();
            if (!reader.Read())
            {
                description.Text = "Товар больше недоступен.";
                card.Visible = false;
                availableSizes.Enabled = false;
                return;
            }
            productName = reader.GetString(1);
            price = FillCard(card, reader);
            description.Text = ReadOptionalText(reader, "description") ?? "Описание отсутствует.";
        }
        const string sizesSql = @"
            SELECT si.stock_item_id, s.size_value, si.quantity
            FROM stock_items AS si
            JOIN sizes AS s ON s.size_id = si.size_id
            WHERE si.product_id = @id

            ORDER BY s.size_value";
        var sizes = new List<string>();
        using (var command = new MySqlCommand(sizesSql, connection))
        {
            command.Parameters.AddWithValue("@id", productId);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var size = new StockSize(reader.GetInt32(0), reader.GetDecimal(1), reader.GetInt32(2));
                sizes.Add(size.ToString());
                if (size.Quantity > 0)
                    availableSizes.Items.Add(size);
            }
        }
        sizeRange.Text = sizes.Count > 0
            ? $"Размерный ряд: {string.Join(", ", sizes)}"
            : "Размерный ряд отсутствует.";
        if (availableSizes.Items.Count > 0)
        {
            availableSizes.SelectedIndex = 0;
            add.Enabled = true;
            add.Click += (_, _) =>
            {
            if (availableSizes.SelectedItem is StockSize selected)
                {
                    SelectedLine = new OrderLine(selected.Id, productName, selected.Size,
                        (int)quantity.Value, price, selected.Quantity);
                    DialogResult = DialogResult.OK;
                }
            };
        }
        else
        {
            availableSizes.Enabled = false;
            availability.Text = "Доступных размеров нет.";
        }
    }




    internal static void SetMonthParameters(MySqlCommand command)
    {
        DateTime today = DateTime.Today;
        DateTime currentMonthStart = new(today.Year, today.Month, 1);

        command.Parameters.AddWithValue("@previousMonthStart", currentMonthStart.AddMonths(-1));
        command.Parameters.AddWithValue("@currentMonthStart", currentMonthStart);
    }

    internal static decimal FillCard(ProductCardControl card, MySqlDataReader reader)
    {
        decimal basePrice = reader.GetDecimal(4);
        bool hasPreviousMonthOrders = Convert.ToBoolean(reader["has_previous_month_orders"]);
        decimal finalPrice = hasPreviousMonthOrders ? basePrice : Math.Round(basePrice * 0.75m, 2, MidpointRounding.AwayFromZero);
        int totalQuantity = Convert.ToInt32(reader["total_quantity"]);
        card.SetProduct(reader.GetString(0), reader.GetString(1), reader.GetString(2),
            reader.GetString(3), totalQuantity, finalPrice, ReadOptionalText(reader, "image_file"));
        return finalPrice;
    }
    private static string? ReadOptionalText(MySqlDataReader reader, string column)
    {
        int index = reader.GetOrdinal(column);
        return reader.IsDBNull(index) ? null : reader.GetString(index);
    }
}
