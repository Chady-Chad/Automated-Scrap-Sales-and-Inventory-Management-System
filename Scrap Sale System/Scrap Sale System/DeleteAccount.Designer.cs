namespace Scrap_Sale_System
{
    partial class DeleteAccount
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
            this.AccountRecorded = new System.Windows.Forms.DataGridView();
            this.AccountConfirm = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.confirmPassword = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.SearchBar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.hidePass1 = new System.Windows.Forms.PictureBox();
            this.conPass = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.AccountRecorded)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidePass1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.conPass)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountRecorded
            // 
            this.AccountRecorded.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AccountRecorded.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.AccountRecorded.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AccountRecorded.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.AccountRecorded.Location = new System.Drawing.Point(12, 47);
            this.AccountRecorded.Name = "AccountRecorded";
            this.AccountRecorded.Size = new System.Drawing.Size(330, 323);
            this.AccountRecorded.TabIndex = 0;
            this.AccountRecorded.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AccountRecorded_CellContentClick);
            // 
            // AccountConfirm
            // 
            this.AccountConfirm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AccountConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountConfirm.Location = new System.Drawing.Point(481, 126);
            this.AccountConfirm.Name = "AccountConfirm";
            this.AccountConfirm.Size = new System.Drawing.Size(195, 29);
            this.AccountConfirm.TabIndex = 1;
            this.AccountConfirm.TextChanged += new System.EventHandler(this.AccountConfirm_TextChanged);
            // 
            // password
            // 
            this.password.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.password.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(481, 182);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(195, 29);
            this.password.TabIndex = 2;
            this.password.TextChanged += new System.EventHandler(this.password_TextChanged);
            // 
            // confirmPassword
            // 
            this.confirmPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.confirmPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.confirmPassword.Location = new System.Drawing.Point(481, 240);
            this.confirmPassword.Name = "confirmPassword";
            this.confirmPassword.Size = new System.Drawing.Size(195, 29);
            this.confirmPassword.TabIndex = 3;
            this.confirmPassword.TextChanged += new System.EventHandler(this.confirmPassword_TextChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DeleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DeleteButton.Location = new System.Drawing.Point(523, 315);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(106, 34);
            this.DeleteButton.TabIndex = 4;
            this.DeleteButton.Text = "Delete In Charge Account";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // SearchBar
            // 
            this.SearchBar.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SearchBar.Font = new System.Drawing.Font("Maiandra GD", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchBar.Location = new System.Drawing.Point(12, 389);
            this.SearchBar.Name = "SearchBar";
            this.SearchBar.Size = new System.Drawing.Size(214, 33);
            this.SearchBar.TabIndex = 26;
            this.SearchBar.TextChanged += new System.EventHandler(this.SearchBar_TextChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(400, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 27);
            this.label1.TabIndex = 25;
            this.label1.Text = "Recorded account names:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(355, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 28;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(355, 191);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 29;
            this.label3.Text = "Password";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(352, 249);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 15);
            this.label4.TabIndex = 30;
            this.label4.Text = "Confirm Password";
            // 
            // hidePass1
            // 
            this.hidePass1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.hidePass1.BackgroundImage = global::Scrap_Sale_System.Properties.Resources.eye;
            this.hidePass1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.hidePass1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.hidePass1.Location = new System.Drawing.Point(682, 185);
            this.hidePass1.Name = "hidePass1";
            this.hidePass1.Size = new System.Drawing.Size(24, 26);
            this.hidePass1.TabIndex = 97;
            this.hidePass1.TabStop = false;
            this.hidePass1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hidePass1_MouseDown);
            this.hidePass1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hidePass1_MouseUp);
            // 
            // conPass
            // 
            this.conPass.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.conPass.BackgroundImage = global::Scrap_Sale_System.Properties.Resources.eye;
            this.conPass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.conPass.Cursor = System.Windows.Forms.Cursors.Hand;
            this.conPass.Location = new System.Drawing.Point(682, 243);
            this.conPass.Name = "conPass";
            this.conPass.Size = new System.Drawing.Size(24, 26);
            this.conPass.TabIndex = 98;
            this.conPass.TabStop = false;
            this.conPass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.conPass_MouseDown);
            this.conPass.MouseUp += new System.Windows.Forms.MouseEventHandler(this.conPass_MouseUp);
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(232, 389);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 30);
            this.label5.TabIndex = 99;
            this.label5.Text = "Search Account \r\nNames";
            // 
            // DeleteAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 453);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.conPass);
            this.Controls.Add(this.hidePass1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SearchBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeleteButton);
            this.Controls.Add(this.confirmPassword);
            this.Controls.Add(this.password);
            this.Controls.Add(this.AccountConfirm);
            this.Controls.Add(this.AccountRecorded);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DeleteAccount";
            this.Text = "DeleteAccount";
            this.Load += new System.EventHandler(this.DeleteAccount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.AccountRecorded)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hidePass1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.conPass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView AccountRecorded;
        private System.Windows.Forms.TextBox AccountConfirm;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.TextBox confirmPassword;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.TextBox SearchBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox hidePass1;
        private System.Windows.Forms.PictureBox conPass;
        private System.Windows.Forms.Label label5;
    }
}