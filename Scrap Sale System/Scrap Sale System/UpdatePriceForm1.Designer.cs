namespace Scrap_Sale_System
{
    partial class UpdatePriceForm1
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
            this.PriceList = new System.Windows.Forms.DataGridView();
            this.SearchItem = new System.Windows.Forms.TextBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.radSolidWaste = new System.Windows.Forms.RadioButton();
            this.radWires = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PriceList)).BeginInit();
            this.SuspendLayout();
            // 
            // PriceList
            // 
            this.PriceList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.PriceList.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.PriceList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PriceList.Location = new System.Drawing.Point(12, 132);
            this.PriceList.Name = "PriceList";
            this.PriceList.Size = new System.Drawing.Size(352, 348);
            this.PriceList.TabIndex = 0;
            this.PriceList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.PriceList_CellContentClick);
            // 
            // SearchItem
            // 
            this.SearchItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchItem.Location = new System.Drawing.Point(125, 91);
            this.SearchItem.Name = "SearchItem";
            this.SearchItem.Size = new System.Drawing.Size(169, 26);
            this.SearchItem.TabIndex = 1;
            this.SearchItem.TextChanged += new System.EventHandler(this.SearchItem_TextChanged);
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(283, 486);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(81, 33);
            this.UpdateButton.TabIndex = 2;
            this.UpdateButton.Text = "Update";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // radSolidWaste
            // 
            this.radSolidWaste.AutoSize = true;
            this.radSolidWaste.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radSolidWaste.ForeColor = System.Drawing.Color.Green;
            this.radSolidWaste.Location = new System.Drawing.Point(79, 51);
            this.radSolidWaste.Name = "radSolidWaste";
            this.radSolidWaste.Size = new System.Drawing.Size(118, 24);
            this.radSolidWaste.TabIndex = 3;
            this.radSolidWaste.TabStop = true;
            this.radSolidWaste.Text = "SolidWaste";
            this.radSolidWaste.UseVisualStyleBackColor = true;
            this.radSolidWaste.CheckedChanged += new System.EventHandler(this.radSolidWaste_CheckedChanged);
            // 
            // radWires
            // 
            this.radWires.AutoSize = true;
            this.radWires.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radWires.ForeColor = System.Drawing.Color.Green;
            this.radWires.Location = new System.Drawing.Point(207, 51);
            this.radWires.Name = "radWires";
            this.radWires.Size = new System.Drawing.Size(72, 24);
            this.radWires.TabIndex = 4;
            this.radWires.TabStop = true;
            this.radWires.Text = "Wires";
            this.radWires.UseVisualStyleBackColor = true;
            this.radWires.CheckedChanged += new System.EventHandler(this.radWires_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(410, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(12, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Search Items";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Green;
            this.label3.Location = new System.Drawing.Point(12, 493);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 26);
            this.label3.TabIndex = 7;
            this.label3.Text = "Click the Prices value in the \r\nPrice column to start editing.";
            // 
            // UpdatePriceForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(374, 531);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radWires);
            this.Controls.Add(this.radSolidWaste);
            this.Controls.Add(this.UpdateButton);
            this.Controls.Add(this.SearchItem);
            this.Controls.Add(this.PriceList);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdatePriceForm1";
            this.Text = "UpdatePriceForm1";
            this.Load += new System.EventHandler(this.UpdatePriceForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PriceList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView PriceList;
        private System.Windows.Forms.TextBox SearchItem;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.RadioButton radSolidWaste;
        private System.Windows.Forms.RadioButton radWires;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}