using MySqlConnector;

namespace WinFormsApp1;

internal sealed record OrderLine(
    int StockItemId, string Name, decimal Size, int Quantity, decimal Price, int Available);
internal sealed class OrderForm : Form
{
    private readonly int userId;
    private readonly List<OrderLine> lines;
    private readonly ListView items = new()
    {
        Dock = DockStyle.Fill,
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false
    };
    private readonly Label total = new() { AutoSize = true, Padding = new Padding(6) };
    private readonly Button confirm = new() { Text = "Подтвердить заказ", AutoSize = true, Enabled = false }; private readonly NumericUpDown quantity = new() { Minimum = 1, Maximum = 1, Width = 80,
        Enabled = false };
    private readonly Button updateQuantity = new() { Text = "Изменить количество", AutoSize = true, 
        Enabled = false };
    public OrderForm(int userId, List<OrderLine> lines)
    {
        this.userId = userId;
        this.lines = lines;
        Text = "Чудо Обувь - заказ";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(820, 385);
        MinimumSize = new Size(836, 424);
        items.Columns.Add("Модель", 370);
        items.Columns.Add("Размер", 75);
        items.Columns.Add("Пар", 55);
        items.Columns.Add("Цена за пару", 120);
        items.Columns.Add("Сумма", 120);

        var back = new Button { Text = "Назад", AutoSize = true, DialogResult = DialogResult.Cancel };
        var remove = new Button { Text = "Убрать позицию", AutoSize = true, Enabled = false };
        var clear = new Button { Text = "Отменить заказ", AutoSize = true };
        CancelButton = back;
        items.SelectedIndexChanged += (_, _) =>
        {
        bool selected = items.SelectedIndices.Count > 0;
        remove.Enabled = selected;
        quantity.Enabled = selected;
        updateQuantity.Enabled = selected;
        if (selected)
            {
                OrderLine line = lines[items.SelectedIndices[0]];
                quantity.Maximum = line.Available;
                quantity.Value = line.Quantity;
            }
        };
        updateQuantity.Click += (_, _) =>
        {
            if (items.SelectedIndices.Count == 0)
                return;

            int index = items.SelectedIndices[0];
            lines[index] = lines[index] with { Quantity = (int)quantity.Value };
            ShowLines();
            items.Items[index].Selected = true;
        };
        remove.Click += (_, _) =>
        {
            if (items.SelectedIndices.Count == 0)
                return;
            lines.RemoveAt(items.SelectedIndices[0]);
            ShowLines();
        };
        confirm.Click += (_, _) => ConfirmOrder();
        confirm.Enabled = lines.Count > 0;

        AppStyle.Apply(this, confirm);
        var actions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, BackColor = AppStyle.Secondary};
        actions.Controls.AddRange(new Control[] { back, remove, clear, confirm, total });
        var quantityActions = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        quantityActions.Controls.Add(new Label { Text = "Количество пар", AutoSize = true, Padding = new Padding(0, 5, 0, 0) });
        quantityActions.Controls.Add(quantity);
        quantityActions.Controls.Add(updateQuantity);
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 3,
            Padding = new Padding(12)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(items, 0, 0);
        layout.Controls.Add(quantityActions, 0, 1);
        layout.Controls.Add(actions, 0, 2);
        Controls.Add(layout);
        ShowLines();
    }
    private void ShowLines()
    {
        items.Items.Clear();
        foreach (OrderLine line in lines)
        {
            items.Items.Add(new ListViewItem(new[]
            {
                line.Name, line.Size.ToString("0.##"), line.Quantity.ToString(),
                $"{line.Price:N2} ₽", $"{line.Price * line.Quantity:N2} ₽"
            }));
        }

        total.Text = $"Итого: {lines.Sum(line => line.Price * line.Quantity):N2} ₽";
    }
    private void ConfirmOrder()
    {
        if (MessageBox.Show(this, "Сохранить заказ и списать выбранные пары с остатка?",
            "Подтверждение заказа", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
        try
        {
            long orderId = SaveOrder();
            lines.Clear();
            MessageBox.Show(this, $"Заказ № {orderId} оформлен.", "Заказ сохранён",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show(this, ex.Message, "Недостаточно товара",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (MySqlException)
        {
            MessageBox.Show(this, "Не удалось подтвердить сохранение. Проверьте список заказов перед повторной попыткой.",
                "Ошибка сохранения заказа", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private long SaveOrder()
    {
        using var connection = Database.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        using var order = new MySqlCommand(
            "INSERT INTO orders (order_date, user_id) VALUES (@date, @userId)", connection, transaction);
        order.Parameters.AddWithValue("@date", DateTime.Today);
        order.Parameters.AddWithValue("@userId", userId);
        order.ExecuteNonQuery();
        long orderId = order.LastInsertedId;
        foreach (OrderLine line in lines.OrderBy(item => item.StockItemId))
        {
            using var stock = new MySqlCommand(
                "UPDATE stock_items SET quantity = quantity - @quantity WHERE stock_item_id = @id AND quantity >= @quantity",
                connection, transaction);
            stock.Parameters.AddWithValue("@quantity", line.Quantity);
            stock.Parameters.AddWithValue("@id", line.StockItemId);
            if (stock.ExecuteNonQuery() != 1)
                throw new InvalidOperationException(
                    $"Для модели «{line.Name}», размер {line.Size:0.##}, осталось меньше пар. Уберите позицию и выберите количество заново.");
            using var item = new MySqlCommand(
                "INSERT INTO order_items (order_id, stock_item_id, quantity, unit_price) VALUES (@orderId, @id, @quantity, @price)",
                connection, transaction);
            item.Parameters.AddWithValue("@orderId", orderId);
            item.Parameters.AddWithValue("@id", line.StockItemId);
            item.Parameters.AddWithValue("@quantity", line.Quantity);
            item.Parameters.AddWithValue("@price", line.Price);
            item.ExecuteNonQuery();
        }

        transaction.Commit();
        return orderId;
    }








}

