namespace WinFormsApp1
{
    partial class CatalogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblCatalogTitle = new Label();
            lblUserName = new Label();
            btnBack = new Button();
            btnOrder = new Button();
            btnOrderList = new Button();
            flpProduct = new FlowLayoutPanel();
            lblSearch = new Label();
            lblCategoryFilter = new Label();
            lblPriceSort = new Label();
            txtSearch = new TextBox();
            cboCategory = new ComboBox();
            cboPriceSort = new ComboBox();
            SuspendLayout();
            // 
            // lblCatalogTitle
            // 
            lblCatalogTitle.AutoSize = true;
            lblCatalogTitle.Location = new Point(12, 18);
            lblCatalogTitle.Name = "lblCatalogTitle";
            lblCatalogTitle.Size = new Size(86, 15);
            lblCatalogTitle.TabIndex = 0;
            lblCatalogTitle.Text = "Каталог обуви";
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(1043, 18);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(37, 15);
            lblUserName.TabIndex = 1;
            lblUserName.Text = "Гость";
            lblUserName.Click += lblUserName_Click;
            // 
            // btnBack
            // 
            btnBack.Location = new Point(12, 57);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 23);
            btnBack.TabIndex = 2;
            btnBack.Text = "Назад";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // btnOrder
            // 
            btnOrder.Enabled = false;
            btnOrder.Location = new Point(221, 57);
            btnOrder.Name = "btnOrder";
            btnOrder.Size = new Size(75, 23);
            btnOrder.TabIndex = 3;
            btnOrder.Text = "Заказ (0)";
            btnOrder.UseVisualStyleBackColor = true;
            btnOrder.Click += btnOrder_Click;
            // 
            // btnOrderList
            // 
            btnOrderList.Enabled = false;
            btnOrderList.Location = new Point(112, 57);
            btnOrderList.Name = "btnOrderList";
            btnOrderList.Size = new Size(75, 23);
            btnOrderList.TabIndex = 4;
            btnOrderList.Text = "Список заказов";
            btnOrderList.UseVisualStyleBackColor = true;
            btnOrderList.Visible = false;
            btnOrderList.Click += btnOrderList_Click;
            // 
            // flpProduct
            // 
            flpProduct.AutoScroll = true;
            flpProduct.FlowDirection = FlowDirection.TopDown;
            flpProduct.Location = new Point(3, 132);
            flpProduct.Name = "flpProduct";
            flpProduct.Size = new Size(1233, 486);
            flpProduct.TabIndex = 5;
            flpProduct.WrapContents = false;
            // 
            // lblSearch
            // 
            lblSearch.AutoSize = true;
            lblSearch.Location = new Point(12, 103);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(200, 15);
            lblSearch.TabIndex = 6;
            lblSearch.Text = "Поиск по названию или описанию";
            lblSearch.Click += lblSearch_Click;
            // 
            // lblCategoryFilter
            // 
            lblCategoryFilter.AutoSize = true;
            lblCategoryFilter.Location = new Point(597, 82);
            lblCategoryFilter.Name = "lblCategoryFilter";
            lblCategoryFilter.Size = new Size(63, 15);
            lblCategoryFilter.TabIndex = 7;
            lblCategoryFilter.Text = "Категория";
            // 
            // lblPriceSort
            // 
            lblPriceSort.AutoSize = true;
            lblPriceSort.Location = new Point(823, 82);
            lblPriceSort.Name = "lblPriceSort";
            lblPriceSort.Size = new Size(119, 15);
            lblPriceSort.TabIndex = 8;
            lblPriceSort.Text = "Сортировка по цене";
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(221, 100);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(340, 23);
            txtSearch.TabIndex = 9;
            txtSearch.TextChanged += CatalogOptionsChanged;
            // 
            // cboCategory
            // 
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategory.FormattingEnabled = true;
            cboCategory.Location = new Point(597, 100);
            cboCategory.Name = "cboCategory";
            cboCategory.Size = new Size(205, 23);
            cboCategory.TabIndex = 10;
            cboCategory.SelectedIndexChanged += CatalogOptionsChanged;
            // 
            // cboPriceSort
            // 
            cboPriceSort.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPriceSort.FormattingEnabled = true;
            cboPriceSort.Items.AddRange(new object[] { "По умолчанию", "Сначала дешевле", "Сначала дороже" });
            cboPriceSort.Location = new Point(823, 100);
            cboPriceSort.Name = "cboPriceSort";
            cboPriceSort.Size = new Size(220, 23);
            cboPriceSort.TabIndex = 11;
            cboPriceSort.SelectedIndexChanged += CatalogOptionsChanged;
            // 
            // CatalogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1237, 617);
            Controls.Add(cboPriceSort);
            Controls.Add(cboCategory);
            Controls.Add(txtSearch);
            Controls.Add(lblPriceSort);
            Controls.Add(lblCategoryFilter);
            Controls.Add(lblSearch);
            Controls.Add(flpProduct);
            Controls.Add(btnOrderList);
            Controls.Add(btnOrder);
            Controls.Add(btnBack);
            Controls.Add(lblUserName);
            Controls.Add(lblCatalogTitle);
            Name = "CatalogForm";
            Text = "Чудо Обувь - каталог";
            Load += CatalogForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblCatalogTitle;
        private Label lblUserName;
        private Button btnBack;
        private Button btnOrder;
        private Button btnOrderList;
        private FlowLayoutPanel flpProduct;
        private Label lblSearch;
        private Label lblCategoryFilter;
        private Label lblPriceSort;
        private TextBox txtSearch;
        private ComboBox cboCategory;
        private ComboBox cboPriceSort;
    }
}