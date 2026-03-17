namespace Scrap_Sale_System
{
    partial class ImportTransactionPanel1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Con = new System.Windows.Forms.TabPage();
            this.AdjustItem = new System.Windows.Forms.Button();
            this.SyncMainServer = new System.Windows.Forms.Button();
            this.ReturnToMain = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.Con);
            this.tabControl1.Location = new System.Drawing.Point(3, 42);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(718, 318);
            this.tabControl1.TabIndex = 9;
            // 
            // Con
            // 
            this.Con.Location = new System.Drawing.Point(4, 22);
            this.Con.Name = "Con";
            this.Con.Padding = new System.Windows.Forms.Padding(3);
            this.Con.Size = new System.Drawing.Size(710, 292);
            this.Con.TabIndex = 0;
            this.Con.Text = "Scrap Transactions";
            this.Con.UseVisualStyleBackColor = true;
            this.Con.Click += new System.EventHandler(this.Con_Click);
            // 
            // AdjustItem
            // 
            this.AdjustItem.BackColor = System.Drawing.Color.Black;
            this.AdjustItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdjustItem.ForeColor = System.Drawing.Color.White;
            this.AdjustItem.Location = new System.Drawing.Point(12, 366);
            this.AdjustItem.Name = "AdjustItem";
            this.AdjustItem.Size = new System.Drawing.Size(142, 40);
            this.AdjustItem.TabIndex = 16;
            this.AdjustItem.TabStop = false;
            this.AdjustItem.Text = "Adjust Item Weight";
            this.AdjustItem.UseVisualStyleBackColor = false;
            // 
            // SyncMainServer
            // 
            this.SyncMainServer.BackColor = System.Drawing.Color.Green;
            this.SyncMainServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SyncMainServer.ForeColor = System.Drawing.Color.White;
            this.SyncMainServer.Location = new System.Drawing.Point(160, 366);
            this.SyncMainServer.Name = "SyncMainServer";
            this.SyncMainServer.Size = new System.Drawing.Size(187, 40);
            this.SyncMainServer.TabIndex = 17;
            this.SyncMainServer.TabStop = false;
            this.SyncMainServer.Text = "Sync to Main Data Server";
            this.SyncMainServer.UseVisualStyleBackColor = false;
            this.SyncMainServer.Click += new System.EventHandler(this.SyncMainServer_Click);
            // 
            // ReturnToMain
            // 
            this.ReturnToMain.BackColor = System.Drawing.Color.White;
            this.ReturnToMain.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.ReturnToMain.Font = new System.Drawing.Font("Maiandra GD", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReturnToMain.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ReturnToMain.Location = new System.Drawing.Point(3, 3);
            this.ReturnToMain.Name = "ReturnToMain";
            this.ReturnToMain.Size = new System.Drawing.Size(121, 33);
            this.ReturnToMain.TabIndex = 18;
            this.ReturnToMain.Text = "Return to Main";
            this.ReturnToMain.UseVisualStyleBackColor = false;
            this.ReturnToMain.Click += new System.EventHandler(this.ReturnToMain_Click);
            // 
            // ImportTransactionPanel1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ReturnToMain);
            this.Controls.Add(this.SyncMainServer);
            this.Controls.Add(this.AdjustItem);
            this.Controls.Add(this.tabControl1);
            this.Name = "ImportTransactionPanel1";
            this.Size = new System.Drawing.Size(721, 561);
            this.Load += new System.EventHandler(this.ImportTransactionPanel1_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Con;
        private System.Windows.Forms.Button AdjustItem;
        private System.Windows.Forms.Button SyncMainServer;
        private System.Windows.Forms.Button ReturnToMain;
    }
}
