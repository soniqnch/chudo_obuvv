namespace WinFormsApp1
{
    partial class ProductCardControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            lblProductTitle = new Label();
            lblCategory = new Label();
            lblQuantity = new Label();
            lblMaterial = new Label();
            lblPrice = new Label();
            picProduct = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picProduct).BeginInit();
            SuspendLayout();
            // 
            // lblProductTitle
            // 
            lblProductTitle.BorderStyle = BorderStyle.FixedSingle;
            lblProductTitle.Location = new Point(156, 12);
            lblProductTitle.Name = "lblProductTitle";
            lblProductTitle.Size = new Size(420, 25);
            lblProductTitle.TabIndex = 0;
            lblProductTitle.Text = "Производство и название";
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.Location = new Point(156, 47);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(69, 15);
            lblCategory.TabIndex = 1;
            lblCategory.Text = "Категория";
            lblCategory.Click += lblCategory_Click;
            // 
            // lblQuantity
            // 
            lblQuantity.AutoSize = true;
            lblQuantity.Location = new Point(156, 71);
            lblQuantity.Name = "lblQuantity";
            lblQuantity.Size = new Size(76, 15);
            lblQuantity.TabIndex = 2;
            lblQuantity.Text = "Количество";
            lblQuantity.Click += lblQuantity_Click;
            // 
            // lblMaterial
            // 
            lblMaterial.Location = new Point(156, 98);
            lblMaterial.Name = "lblMaterial";
            lblMaterial.Size = new Size(76, 24);
            lblMaterial.TabIndex = 3;
            lblMaterial.Text = "Состав";
            lblMaterial.Click += lblMaterial_Click;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.Font = new Font("Calibri", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 204);
            lblPrice.Location = new Point(645, 47);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(47, 21);
            lblPrice.TabIndex = 4;
            lblPrice.Text = "Цена";
            // 
            // picProduct
            // 
            picProduct.BorderStyle = BorderStyle.FixedSingle;
            picProduct.Location = new Point(12, 12);
            picProduct.Name = "picProduct";
            picProduct.Size = new Size(128, 110);
            picProduct.SizeMode = PictureBoxSizeMode.Zoom;
            picProduct.TabIndex = 5;
            picProduct.TabStop = false;
            // 
            // ProductCardControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(picProduct);
            Controls.Add(lblPrice);
            Controls.Add(lblMaterial);
            Controls.Add(lblQuantity);
            Controls.Add(lblCategory);
            Controls.Add(lblProductTitle);
            Font = new Font("Microsoft Sans Serif", 9F);
            Name = "ProductCardControl";
            Size = new Size(798, 183);
            ((System.ComponentModel.ISupportInitialize)picProduct).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblProductTitle;
        private Label lblCategory;
        private Label lblQuantity;
        private Label lblMaterial;
        private Label lblPrice;
        private PictureBox picProduct;
    }
}
