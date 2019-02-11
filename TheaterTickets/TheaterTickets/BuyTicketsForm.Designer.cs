namespace TheaterTickets
{
    partial class BuyTicketsForm
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.PlaceNumeretic1 = new System.Windows.Forms.NumericUpDown();
            this.PlaceNumeretic2 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BuyTicket = new System.Windows.Forms.Button();
            this.Price = new System.Windows.Forms.Label();
            this.LoginButton = new System.Windows.Forms.Button();
            this.UsernameTextBox = new System.Windows.Forms.TextBox();
            this.PasswordText = new System.Windows.Forms.TextBox();
            this.RegisterButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.PlaceNumeretic1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlaceNumeretic2)).BeginInit();
            this.SuspendLayout();
            // 
            // PlaceNumeretic1
            // 
            this.PlaceNumeretic1.Location = new System.Drawing.Point(12, 12);
            this.PlaceNumeretic1.Name = "PlaceNumeretic1";
            this.PlaceNumeretic1.Size = new System.Drawing.Size(31, 20);
            this.PlaceNumeretic1.TabIndex = 1;
            this.PlaceNumeretic1.ValueChanged += new System.EventHandler(this.PlaceNumeretic1_ValueChanged);
            // 
            // PlaceNumeretic2
            // 
            this.PlaceNumeretic2.Location = new System.Drawing.Point(12, 39);
            this.PlaceNumeretic2.Name = "PlaceNumeretic2";
            this.PlaceNumeretic2.Size = new System.Drawing.Size(31, 20);
            this.PlaceNumeretic2.TabIndex = 2;
            this.PlaceNumeretic2.ValueChanged += new System.EventHandler(this.PlaceNumeretic2_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ряд";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Кресло";
            // 
            // BuyTicket
            // 
            this.BuyTicket.Enabled = false;
            this.BuyTicket.Location = new System.Drawing.Point(12, 65);
            this.BuyTicket.Name = "BuyTicket";
            this.BuyTicket.Size = new System.Drawing.Size(119, 23);
            this.BuyTicket.TabIndex = 5;
            this.BuyTicket.Text = "Купить билет";
            this.BuyTicket.UseVisualStyleBackColor = true;
            this.BuyTicket.Click += new System.EventHandler(this.BuyTicket_Click);
            // 
            // Price
            // 
            this.Price.AutoSize = true;
            this.Price.Location = new System.Drawing.Point(161, 41);
            this.Price.Name = "Price";
            this.Price.Size = new System.Drawing.Size(33, 13);
            this.Price.TabIndex = 6;
            this.Price.Text = "Цена";
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(262, 60);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 7;
            this.LoginButton.Text = "Войти";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // UsernameTextBox
            // 
            this.UsernameTextBox.Location = new System.Drawing.Point(243, 8);
            this.UsernameTextBox.Name = "UsernameTextBox";
            this.UsernameTextBox.Size = new System.Drawing.Size(112, 20);
            this.UsernameTextBox.TabIndex = 8;
            // 
            // PasswordText
            // 
            this.PasswordText.Location = new System.Drawing.Point(243, 34);
            this.PasswordText.Name = "PasswordText";
            this.PasswordText.Size = new System.Drawing.Size(112, 20);
            this.PasswordText.TabIndex = 9;
            this.PasswordText.UseSystemPasswordChar = true;
            // 
            // RegisterButton
            // 
            this.RegisterButton.Location = new System.Drawing.Point(243, 89);
            this.RegisterButton.Name = "RegisterButton";
            this.RegisterButton.Size = new System.Drawing.Size(112, 23);
            this.RegisterButton.TabIndex = 10;
            this.RegisterButton.Text = "Зарегестрироваться";
            this.RegisterButton.UseVisualStyleBackColor = true;
            this.RegisterButton.Click += new System.EventHandler(this.RegisterButton_Click);
            // 
            // BuyTicketsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 170);
            this.Controls.Add(this.RegisterButton);
            this.Controls.Add(this.PasswordText);
            this.Controls.Add(this.UsernameTextBox);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.Price);
            this.Controls.Add(this.BuyTicket);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PlaceNumeretic2);
            this.Controls.Add(this.PlaceNumeretic1);
            this.Name = "BuyTicketsForm";
            this.Text = "Купить билет";
            ((System.ComponentModel.ISupportInitialize)(this.PlaceNumeretic1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PlaceNumeretic2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown PlaceNumeretic1;
        private System.Windows.Forms.NumericUpDown PlaceNumeretic2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BuyTicket;
        private System.Windows.Forms.Label Price;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.TextBox UsernameTextBox;
        private System.Windows.Forms.TextBox PasswordText;
        private System.Windows.Forms.Button RegisterButton;
    }
}

