namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnLogin = new Button();
            idTitle = new Label();
            idLogin = new Label();
            txtLogin = new TextBox();
            btnGuest = new Button();
            picLogo = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(277, 224);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(196, 33);
            btnLogin.TabIndex = 0;
            btnLogin.Text = "Войти";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // idTitle
            // 
            idTitle.AutoSize = true;
            idTitle.Location = new Point(328, 121);
            idTitle.Name = "idTitle";
            idTitle.Size = new Size(97, 17);
            idTitle.TabIndex = 1;
            idTitle.Text = "Вход в систему";
            // 
            // idLogin
            // 
            idLogin.AutoSize = true;
            idLogin.Location = new Point(277, 154);
            idLogin.Name = "idLogin";
            idLogin.Size = new Size(44, 17);
            idLogin.TabIndex = 2;
            idLogin.Text = "Логин";
            idLogin.Click += label2_Click;
            // 
            // txtLogin
            // 
            txtLogin.Location = new Point(277, 183);
            txtLogin.Name = "txtLogin";
            txtLogin.Size = new Size(196, 24);
            txtLogin.TabIndex = 3;
            // 
            // btnGuest
            // 
            btnGuest.Location = new Point(277, 263);
            btnGuest.Name = "btnGuest";
            btnGuest.Size = new Size(196, 38);
            btnGuest.TabIndex = 4;
            btnGuest.Text = "Смотреть каталог без входа";
            btnGuest.UseVisualStyleBackColor = true;
            btnGuest.Click += btnGuest_Click;
            // 
            // picLogo
            // 
            picLogo.Location = new Point(298, 22);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(161, 79);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 5;
            picLogo.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(picLogo);
            Controls.Add(btnGuest);
            Controls.Add(txtLogin);
            Controls.Add(idLogin);
            Controls.Add(idTitle);
            Controls.Add(btnLogin);
            Name = "Form1";
            Text = "Чудо Обувь — вход в систему";
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLogin;
        private Label idTitle;
        private Label idLogin;
        private TextBox txtLogin;
        private Button btnGuest;
        private PictureBox picLogo;
    }
}
