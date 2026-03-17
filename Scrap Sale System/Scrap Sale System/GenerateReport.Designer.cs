namespace Scrap_Sale_System
{
    partial class GenerateReport
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.Show = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.ShowRecordViewer = new System.Windows.Forms.DataGridView();
            this.TotalScraps = new System.Windows.Forms.Label();
            this.TotalTransaction = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.dateToHistory = new System.Windows.Forms.DateTimePicker();
            this.dateFromHistory = new System.Windows.Forms.DateTimePicker();
            this.DownloadReport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Show)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShowRecordViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // Show
            // 
            this.Show.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea3.Name = "ChartArea1";
            this.Show.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.Show.Legends.Add(legend3);
            this.Show.Location = new System.Drawing.Point(14, 14);
            this.Show.Name = "Show";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.Show.Series.Add(series3);
            this.Show.Size = new System.Drawing.Size(664, 556);
            this.Show.TabIndex = 0;
            this.Show.Text = "chart1";
            this.Show.Click += new System.EventHandler(this.Show_Click);
            // 
            // ShowRecordViewer
            // 
            this.ShowRecordViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowRecordViewer.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ShowRecordViewer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ShowRecordViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ShowRecordViewer.Location = new System.Drawing.Point(688, 14);
            this.ShowRecordViewer.Name = "ShowRecordViewer";
            this.ShowRecordViewer.ReadOnly = true;
            this.ShowRecordViewer.Size = new System.Drawing.Size(527, 331);
            this.ShowRecordViewer.TabIndex = 2;
            this.ShowRecordViewer.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ShowRecordViewer_CellContentClick);
            // 
            // TotalScraps
            // 
            this.TotalScraps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalScraps.AutoSize = true;
            this.TotalScraps.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalScraps.ForeColor = System.Drawing.Color.Blue;
            this.TotalScraps.Location = new System.Drawing.Point(684, 436);
            this.TotalScraps.Name = "TotalScraps";
            this.TotalScraps.Size = new System.Drawing.Size(132, 24);
            this.TotalScraps.TabIndex = 6;
            this.TotalScraps.Text = "Total Scraps:";
            this.TotalScraps.Click += new System.EventHandler(this.TotalScraps_Click);
            // 
            // TotalTransaction
            // 
            this.TotalTransaction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalTransaction.AutoSize = true;
            this.TotalTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalTransaction.ForeColor = System.Drawing.Color.Blue;
            this.TotalTransaction.Location = new System.Drawing.Point(684, 394);
            this.TotalTransaction.Name = "TotalTransaction";
            this.TotalTransaction.Size = new System.Drawing.Size(215, 25);
            this.TotalTransaction.TabIndex = 5;
            this.TotalTransaction.Text = "Total Transactions:";
            this.TotalTransaction.Click += new System.EventHandler(this.TotalTransaction_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Enabled = false;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(39, 598);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(77, 32);
            this.richTextBox1.TabIndex = 16;
            this.richTextBox1.TabStop = false;
            this.richTextBox1.Text = "Period:";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(273, 602);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(27, 16);
            this.label4.TabIndex = 15;
            this.label4.Text = "To:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(122, 602);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 16);
            this.label3.TabIndex = 14;
            this.label3.Text = "From:";
            // 
            // GenerateButton
            // 
            this.GenerateButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GenerateButton.BackColor = System.Drawing.Color.Gray;
            this.GenerateButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.GenerateButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.GenerateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GenerateButton.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.GenerateButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GenerateButton.Location = new System.Drawing.Point(407, 590);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(127, 40);
            this.GenerateButton.TabIndex = 13;
            this.GenerateButton.TabStop = false;
            this.GenerateButton.Text = "GENERATE";
            this.GenerateButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.GenerateButton.UseVisualStyleBackColor = false;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // dateToHistory
            // 
            this.dateToHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateToHistory.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateToHistory.Location = new System.Drawing.Point(303, 602);
            this.dateToHistory.Name = "dateToHistory";
            this.dateToHistory.Size = new System.Drawing.Size(98, 20);
            this.dateToHistory.TabIndex = 12;
            this.dateToHistory.TabStop = false;
            this.dateToHistory.ValueChanged += new System.EventHandler(this.dateToHistory_ValueChanged);
            // 
            // dateFromHistory
            // 
            this.dateFromHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateFromHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateFromHistory.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateFromHistory.Location = new System.Drawing.Point(169, 602);
            this.dateFromHistory.Name = "dateFromHistory";
            this.dateFromHistory.Size = new System.Drawing.Size(95, 20);
            this.dateFromHistory.TabIndex = 11;
            this.dateFromHistory.TabStop = false;
            this.dateFromHistory.ValueChanged += new System.EventHandler(this.dateFromHistory_ValueChanged);
            // 
            // DownloadReport
            // 
            this.DownloadReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DownloadReport.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.DownloadReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadReport.Location = new System.Drawing.Point(689, 500);
            this.DownloadReport.Name = "DownloadReport";
            this.DownloadReport.Size = new System.Drawing.Size(119, 40);
            this.DownloadReport.TabIndex = 17;
            this.DownloadReport.Text = "Download Report";
            this.DownloadReport.UseVisualStyleBackColor = true;
            this.DownloadReport.Click += new System.EventHandler(this.DownloadReport_Click);
            // 
            // GenerateReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DownloadReport);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.dateToHistory);
            this.Controls.Add(this.dateFromHistory);
            this.Controls.Add(this.TotalScraps);
            this.Controls.Add(this.TotalTransaction);
            this.Controls.Add(this.ShowRecordViewer);
            this.Controls.Add(this.Show);
            this.Name = "GenerateReport";
            this.Size = new System.Drawing.Size(1239, 659);
            this.Load += new System.EventHandler(this.UserControl1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Show)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ShowRecordViewer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart Show;
        private System.Windows.Forms.DataGridView ShowRecordViewer;
        private System.Windows.Forms.Label TotalScraps;
        private System.Windows.Forms.Label TotalTransaction;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button GenerateButton;
        private System.Windows.Forms.DateTimePicker dateToHistory;
        private System.Windows.Forms.DateTimePicker dateFromHistory;
        private System.Windows.Forms.Button DownloadReport;
    }
}
