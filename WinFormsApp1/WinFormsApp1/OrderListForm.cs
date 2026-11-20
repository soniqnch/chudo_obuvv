using MySqlConnector;

namespace WinFormsApp1;

internal sealed class OrderListForm : Form
{
    private readonly ListView orders = new()
    {
        Dock = DockStyle.Fill,
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false
    };
    private readonly ListView items = new()
    {
        Dock = DockStyle.Fill,
        View = View.Details,
        FullRowSelect = true,
        MultiSelect = false
    };
    private readonly Label total = new() { AutoSize = true, Padding = new Padding(6) };

    private readonly Button deleteOrder = new() { Text = "Удалить заказ", AutoSize = true, Enabled = false };
    private readonly Button editDate = new() { Text = "Сохранить дату", AutoSize = true, Enabled = false };
    private readonly Button deleteItem = new() { Text = "Удалить позицию", AutoSize = true, Enabled = false };
    private readonly DateTimePicker orderDate = new() { Format = DateTimePickerFormat.Short };
    internal bool HasChanges { get; private set; }
    public OrderListForm(bool isAdministrator)
    {
        Text = "Чудо Обувь - список заказов";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(965, 570);
        MinimumSize = new Size(981, 609);
        orders.Columns.Add("Номер", 90);
        orders.Columns.Add("Дата заказа", 130);
        orders.Columns.Add("ФИО клиента", 560);

        items.Columns.Add("Производство | Наименование", 440);
        items.Columns.Add("Размер", 80);
        items.Columns.Add("Пар", 70);
        items.Columns.Add("Цена за пару", 145);
        items.Columns.Add("Сумма", 145);

        var back = new Button { Text = "Назад", AutoSize = true, DialogResult = DialogResult.Cancel };
        var refresh = new Button { Text = "Обновить", AutoSize = true };
        refresh.Click += (_, _) => LoadOrders();
        CancelButton = back;
        var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill };

        var add = new Button { Text = "Новый заказ", AutoSize = true};
        add.Click += (_, _) => AddOrder();
        editDate.Click += (_, _) => ChangeDate();
        deleteOrder.Click += (_, _) => DeleteSelection(wholeOrder: true);
        deleteItem.Click += (_, _) => DeleteSelection(wholeOrder: false);
        AppStyle.Apply(this, add);
        actions.BackColor = AppStyle.Secondary;


        orderDate.Visible = editDate.Visible = deleteItem.Visible = isAdministrator;
        orderDate.Enabled = false;
        actions.Controls.AddRange(new Control[]
        {
            back, refresh, add, deleteOrder, orderDate, editDate, deleteItem, total
        });
        items.SelectedIndexChanged += (_, _) => deleteItem.Enabled = items.SelectedItems.Count > 0;


        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 5,
            Padding = new Padding(12)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 52));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 48));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

        layout.Controls.Add(new Label { Text = "Заказы", AutoSize = true }, 0, 0);
        layout.Controls.Add(orders, 0, 1);
        layout.Controls.Add(new Label { Text = "Состав выбранного заказа", AutoSize = true }, 0, 2);
        layout.Controls.Add(items, 0, 3);
        layout.Controls.Add(actions, 0, 4);
        Controls.Add(layout);
        orders.SelectedIndexChanged += (_, _) => LoadSelectedOrder();
        Load += (_, _) => LoadOrders();
    }
    private void LoadOrders()
    {
        long? selectedId = orders.SelectedItems.Count == 0 ? null : (long)orders.SelectedItems[0].Tag!;
        const string sql = @"
            SELECT o.order_id, o.order_date,
                   CONCAT_WS(' ', u.last_name, u.first_name, NULLIF(TRIM(u.patronymic), ''))
            FROM orders AS o
            JOIN users AS u ON u.user_id = o.user_id
            ORDER BY o.order_date DESC, o.order_id DESC";
        var rows = new List<ListViewItem>();
        try
        {
            using var connection = Database.CreateConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                long id = reader.GetInt64(0);
                rows.Add(new ListViewItem(new[]
                {
                    id.ToString(), reader.GetDateTime(1).ToString("dd.MM.yyyy"), reader.GetString(2)
                })
                { Tag = id });
            }
        }
        catch (MySqlException)
        {
            MessageBox.Show(this, "Не удалось загрузить заказы. Проверьте подключение и нажмите «Обновить».",
                "Ошибка загрузки заказов", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }
        orders.Items.Clear();
        UpdateSelection();
        orders.Items.AddRange(rows.ToArray());
        items.Items.Clear();
        total.Text = rows.Count == 0 ? "Заказов пока нет." : "Выберите заказ.";
        var selected = rows.Find(row => (long)row.Tag! == selectedId) ?? rows.FirstOrDefault();
        if (selected != null)
        {
            selected.Selected = true;
            selected.EnsureVisible();
        }
    }
    private void LoadSelectedOrder()
    {
        items.Items.Clear();
        UpdateSelection();
        total.Text = "Выберите заказ.";
        if (orders.SelectedItems.Count == 0)
            return;
        const string sql = @"
            SELECT m.manufacturer_name, p.product_name, s.size_value, oi.quantity, oi.unit_price, oi.order_item_id
            FROM order_items AS oi
            JOIN stock_items AS si ON si.stock_item_id = oi.stock_item_id
            JOIN products AS p ON p.product_id = si.product_id
            JOIN manufacturers AS m ON m.manufacturer_id = p.manufacturer_id
            JOIN sizes AS s ON s.size_id = si.size_id
            WHERE oi.order_id = @id
            ORDER BY p.product_name, s.size_value";
        decimal sum = 0;
        var rows = new List<ListViewItem>();
        try
        {
            using var connection = Database.CreateConnection();
            connection.Open();
            using var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", orders.SelectedItems[0].Tag);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                int quantity = reader.GetInt32(3);
                decimal unitPrice = reader.GetDecimal(4);
                decimal lineTotal = unitPrice * quantity;
                sum += lineTotal;
                rows.Add(new ListViewItem(new[]
                {
                    $"{reader.GetString(0)} | {reader.GetString(1)}",
                    reader.GetDecimal(2).ToString("0.##"), quantity.ToString(),
                    $"{unitPrice:N2} ₽", $"{lineTotal:N2} ₽"
                })
                { Tag = reader.GetInt64(5) });
            }

            items.Items.AddRange(rows.ToArray());
            total.Text = $"Итого по заказу: {sum:N2} ₽";
        }
        catch (MySqlException)
        {
            total.Text = "Состав заказа недоступен.";
            MessageBox.Show(this, "Не удалось загрузить состав заказа. Нажмите «Обновить» и повторите выбор.",
                "Ошибка загрузки состава", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    private void UpdateSelection()
    {
        bool selected = orders.SelectedItems.Count > 0;
        deleteOrder.Enabled = editDate.Enabled = orderDate.Enabled = selected;
        if (selected && DateTime.TryParseExact(orders.SelectedItems[0].SubItems[1].Text,
            "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out DateTime date))
        {
            orderDate.Value = date;
        }
    }
    private sealed record Client(int Id, string Name)
    {
        public override string ToString() => Name;
    }
    private void AddOrder()
    {
        var clients = new List<Client>();
        try
        {
            using var connection = Database.CreateConnection();
            connection.Open();
            const string sql = @"
                SELECT u.user_id, CONCAT_WS(' ', u.last_name, u.first_name, NULLIF(TRIM(u.patronymic), ''))
                FROM users AS u
                JOIN roles AS r ON r.role_id = u.role_id
                WHERE LOWER(TRIM(r.role_name)) IN (
                    'авторизованный пользователь', 'пользователь', 'клиент',
                    'client', 'customer', 'user', 'authorized user', 'authorizeduser', 'stock_keeper')
                ORDER BY u.last_name, u.first_name, u.user_id";
            using var command = new MySqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                clients.Add(new Client(reader.GetInt32(0), reader.GetString(1)));
        }
        catch (MySqlException)
        {
            ShowError("Не удалось загрузить список клиентов.");
            return;
        }
        if (clients.Count == 0)
        {
            MessageBox.Show(this, "В базе нет пользователей с ролью клиента.", "Выбор клиента",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using var picker = new Form
        {
            Text = "Выбор клиента",
            ClientSize = new Size(460, 120),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };
        var list = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            DataSource = clients,
            Location = new Point(12, 22),
            Width = 436
        };
        var next = new Button { Text = "Открыть каталог", AutoSize = true, Location = new Point(12, 65), DialogResult = DialogResult.OK };
        AppStyle.Apply(picker, next);
        var cancel = new Button { Text = "Отмена", AutoSize = true, Location = new Point(150, 65), DialogResult = DialogResult.Cancel };
        picker.Controls.AddRange(new Control[] { list, next, cancel });
        picker.AcceptButton = next;
        picker.CancelButton = cancel;
        if (picker.ShowDialog(this) != DialogResult.OK || list.SelectedItem is not Client client)
            return;

        using var catalog = CatalogForm.ForClient(client.Id, client.Name);
        if (catalog.ShowDialog(this) == DialogResult.OK)
        {
            HasChanges = true;
            LoadOrders();
        }
    }

    private void ChangeDate()
    {
        if (!editDate.Visible || orders.SelectedItems.Count == 0)
            return;

        try
        {
            using var connection = Database.CreateConnection();
            connection.Open();
            using var command = new MySqlCommand("UPDATE orders SET order_date = @date WHERE order_id = @id", connection);
            command.Parameters.AddWithValue("@id", orders.SelectedItems[0].Tag);
            command.Parameters.AddWithValue("@date", orderDate.Value.Date);
            command.ExecuteNonQuery();
            HasChanges = true;
            LoadOrders();
        }
        catch (MySqlException)
        {
            ShowError("Не удалось сохранить дату заказа.");
        }
    }
    private void DeleteSelection(bool wholeOrder)
    {
        if (orders.SelectedItems.Count == 0 || (!wholeOrder && (!deleteItem.Visible || items.SelectedItems.Count == 0)))
            return;

        long orderId = (long)orders.SelectedItems[0].Tag!;
        long? itemId = wholeOrder ? null : (long)items.SelectedItems[0].Tag!;
        string prompt = wholeOrder
            ? "Удалить заказ целиком и вернуть все пары на склад?"
            : "Удалить выбранную позицию и вернуть её пары на склад?";
        if (MessageBox.Show(this, prompt, "Подтверждение удаления",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;
        try
        {
            using var connection = Database.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();
            using var order = new MySqlCommand("SELECT order_id FROM orders WHERE order_id = @order FOR UPDATE", connection, transaction);
            order.Parameters.AddWithValue("@order", orderId);
            if (order.ExecuteScalar() == null)
            {
                transaction.Rollback();
                LoadOrders();
                return;
            }
            string condition = itemId.HasValue ? " AND order_item_id = @item" : "";
            using var read = new MySqlCommand(
                "SELECT stock_item_id, quantity FROM order_items WHERE order_id = @order" + condition + " FOR UPDATE",
                connection, transaction);
            read.Parameters.AddWithValue("@order", orderId);
            if (itemId.HasValue)
                read.Parameters.AddWithValue("@item", itemId.Value);
            var returns = new SortedDictionary<int, int>();
            using (var reader = read.ExecuteReader())
            {
                while (reader.Read())
                {
                    int stockId = reader.GetInt32(0);
                    returns[stockId] = returns.GetValueOrDefault(stockId) + reader.GetInt32(1);
                }
            }
            foreach (var entry in returns)
            {
                using var stock = new MySqlCommand("UPDATE stock_items SET quantity = quantity + @qty WHERE stock_item_id = @id",
                    connection, transaction);
                stock.Parameters.AddWithValue("@qty", entry.Value);
                stock.Parameters.AddWithValue("@id", entry.Key);
                stock.ExecuteNonQuery();
            }
            using var delete = new MySqlCommand("DELETE FROM order_items WHERE order_id = @order" + condition,
                connection, transaction);
            delete.Parameters.AddWithValue("@order", orderId);
            if (itemId.HasValue)
                delete.Parameters.AddWithValue("@item", itemId.Value);
            delete.ExecuteNonQuery();
            if (wholeOrder)
            {
                using var deleteHeader = new MySqlCommand("DELETE FROM orders WHERE order_id = @order", connection, transaction);
                deleteHeader.Parameters.AddWithValue("@order", orderId);
                deleteHeader.ExecuteNonQuery();
            }

            transaction.Commit();
            HasChanges = true;
            LoadOrders();
        }
        catch (MySqlException)
        {
            ShowError("Не удалось подтвердить удаление. Нажмите «Обновить» перед повторной попыткой.");
        }
    }



    private void ShowError(string message)
    {
        MessageBox.Show(this, message, "Работа с заказами", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }



}



