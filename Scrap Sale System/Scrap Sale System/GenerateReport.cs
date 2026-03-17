using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using System.Configuration;
namespace Scrap_Sale_System
{
    public partial class GenerateReport : UserControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ServerPathDbConnection"].ConnectionString;
        //string connectionString = "server=10.0.253.60;user=root;password=Windows7;database=Scraps";
        //  string connectionString = "server=localhost;user=root;password=Windows7;database=Scraps";
        //string connectionString = "server=localhost;user=root;password=masterx;database=Scraps";
        public GenerateReport()
        {

            InitializeComponent();
            Show.ChartAreas[0].AxisX.Title = "Company";
            Show.ChartAreas[0].AxisY.Title = "Total Scraps (kg)";
            Show.Series.Clear();
            Show.Series.Add("Scraps");
            Show.Series["Scraps"].ChartType = SeriesChartType.Column;
            ShowRecordViewer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DesignDataGridView();
            InitializeDataGridViewColumns();

           
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void GenerateButton_Click(object sender, EventArgs e)
        {
            LoadReport();
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void DownloadReport_Click(object sender, EventArgs e)
        {

        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadReport()
        {
            DateTime startDate = dateFromHistory.Value.Date;
            DateTime endDate = dateToHistory.Value.Date;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                // TOTAL TRANSACTIONS (excluding cancelled transactions)
                MySqlCommand cmdCount = new MySqlCommand(
                    @"SELECT COUNT(*) 
              FROM TransactionMasterfile tm
              WHERE tm.TransactionDate BETWEEN @start AND @end 
                AND tm.IsCancel = 0
                AND NOT EXISTS (
                    SELECT 1 FROM CancelledTransaction ct 
                    WHERE ct.TransactionID = tm.Transaction_ID
                )", conn);

                cmdCount.Parameters.AddWithValue("@start", startDate);
                cmdCount.Parameters.AddWithValue("@end", endDate);
                int totalTransactions = Convert.ToInt32(cmdCount.ExecuteScalar());
                TotalTransaction.Text = $"Total Transactions: {totalTransactions}";


                // TOTAL SCRAPS (exclude IsVoid = 1)
                MySqlCommand cmdScraps = new MySqlCommand(
                    @"SELECT c.CustomerName, SUM(td.Weight) AS TotalScraps
              FROM TransactionMasterfile tm
              JOIN TransactionDetails td ON tm.Transaction_ID = td.TransactionID
              JOIN CustomerMasterfile c ON tm.CustomerID = c.Customer_ID
              WHERE tm.TransactionDate BETWEEN @start AND @end
                AND tm.IsCancel = 0
                AND td.IsVoid = 0
                AND NOT EXISTS (
                    SELECT 1 FROM CancelledTransaction ct
                    WHERE ct.TransactionID = tm.Transaction_ID
                )
              GROUP BY c.CustomerName", conn);

                cmdScraps.Parameters.AddWithValue("@start", startDate);
                cmdScraps.Parameters.AddWithValue("@end", endDate);

                DataTable dt = new DataTable();
                dt.Load(cmdScraps.ExecuteReader());
                ShowRecordViewer.DataSource = dt;


                Show.Series["Scraps"].Points.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    Show.Series["Scraps"].Points.AddXY(row["CustomerName"], row["TotalScraps"]);
                }

                // Total scraps value
                double totalScrapWeight = 0;
                foreach (DataRow row in dt.Rows)
                    totalScrapWeight += Convert.ToDouble(row["TotalScraps"]);

                TotalScraps.Text = $"Total Scraps: {totalScrapWeight} kg";
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void InitializeDataGridViewColumns()
        {

            ShowRecordViewer.Columns.Clear();


            DataGridViewTextBoxColumn customerColumn = new DataGridViewTextBoxColumn();
            customerColumn.Name = "CustomerName";
            customerColumn.HeaderText = "Customer Name";
            customerColumn.DataPropertyName = "CustomerName";
            customerColumn.ReadOnly = true;
            ShowRecordViewer.Columns.Add(customerColumn);


            DataGridViewTextBoxColumn totalScrapsColumn = new DataGridViewTextBoxColumn();
            totalScrapsColumn.Name = "TotalScraps";
            totalScrapsColumn.HeaderText = "Total Scraps (kg)";
            totalScrapsColumn.DataPropertyName = "TotalScraps";
            totalScrapsColumn.ReadOnly = true;
            ShowRecordViewer.Columns.Add(totalScrapsColumn);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void DesignDataGridView()
        {

            ShowRecordViewer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ShowRecordViewer.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ShowRecordViewer.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ShowRecordViewer.AllowUserToAddRows = false;
            ShowRecordViewer.AllowUserToResizeRows = false;
            ShowRecordViewer.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ShowRecordViewer.MultiSelect = false;
            ShowRecordViewer.ReadOnly = true;
            ShowRecordViewer.RowTemplate.Height = 35;
            ShowRecordViewer.BorderStyle = BorderStyle.None;
            ShowRecordViewer.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            ShowRecordViewer.GridColor = Color.LightGray;
            ShowRecordViewer.EnableHeadersVisualStyles = false;


            ShowRecordViewer.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            ShowRecordViewer.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            ShowRecordViewer.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            ShowRecordViewer.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            ShowRecordViewer.RowsDefaultCellStyle.BackColor = Color.White;
            ShowRecordViewer.RowsDefaultCellStyle.ForeColor = Color.Black;
            ShowRecordViewer.RowsDefaultCellStyle.Font = new Font("Segoe UI", 10);
            ShowRecordViewer.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            ShowRecordViewer.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);


            ShowRecordViewer.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
            ShowRecordViewer.DefaultCellStyle.SelectionForeColor = Color.White;


            ShowRecordViewer.RowHeadersVisible = false;

        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void dataGridView1ScrollControl(object sender, KeyEventArgs e)
        {
            if (ShowRecordViewer.Rows.Count == 0)
                return;

            int firstVisible = ShowRecordViewer.FirstDisplayedScrollingRowIndex;

            if (e.KeyCode == Keys.Down)
            {
                if (firstVisible < ShowRecordViewer.Rows.Count - 1)
                    ShowRecordViewer.FirstDisplayedScrollingRowIndex = firstVisible + 1;

                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (firstVisible > 0)
                    ShowRecordViewer.FirstDisplayedScrollingRowIndex = firstVisible - 1;

                e.Handled = true;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //isolate these voids for future functions
        private void UserControl1_Load(object sender, EventArgs e){ }
        private void ShowRecordViewer_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        private void Show_Click(object sender, EventArgs e){ }
        private void TotalTransaction_Click(object sender, EventArgs e){ }
        private void TotalScraps_Click(object sender, EventArgs e) { }
        private void dateFromHistory_ValueChanged(object sender, EventArgs e) { }
        private void dateToHistory_ValueChanged(object sender, EventArgs e){ }

    }
}
