using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Security.Cryptography;
using MySqlX.XDevAPI.Common;
using Mysqlx.Resultset;
using Mysqlx.Crud;
using System.Runtime.CompilerServices;
using Org.BouncyCastle.Asn1.Sec;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics.Eventing.Reader;

namespace Scrap_Sale_System
{
    public partial class Main : Form
    {
         string connectionString = ConfigurationManager.ConnectionStrings["ServerPathDbConnection"].ConnectionString;
        
        //  string connectionString = "server=10.0.253.60;user=root;password=Windows7;database=Scraps";
        //  string connectionString = "server=localhost;user=root;password=Windows7;database=Scraps";
        //  string connectionString = "server=localhost;user=root;password=masterx;database=Scraps";
        private bool isLoggingOut = false;
        private ToolTip refreshTransactionList = new ToolTip();
        public Main()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormClosing += Main_FormClosing;
            VAT.ReadOnly = true;

            SolidWasteSummarydataGrid.ReadOnly = true;
            WiresSummarydataGrid.ReadOnly = true;
            bottomAmountList.ReadOnly = true;
            bottomAmountListSolidWaste.ReadOnly = true;
            OtherScrapsSummary.ReadOnly = true;
            OtherScrapsAmount.ReadOnly = true;


            SolidWasteSummarydataGrid.RowHeadersVisible = false;
            SolidWasteSummarydataGrid.AllowUserToAddRows = false;
            SolidWasteSummarydataGrid.AllowUserToResizeColumns = false;
            SolidWasteSummarydataGrid.AllowUserToResizeRows = false;
            WiresSummarydataGrid.RowHeadersVisible = false;
            WiresSummarydataGrid.AllowUserToAddRows = false;
            WiresSummarydataGrid.AllowUserToResizeColumns = false;
            WiresSummarydataGrid.AllowUserToResizeRows = false;
            bottomAmountList.RowHeadersVisible = false;
            bottomAmountList.AllowUserToAddRows = false;
            bottomAmountList.AllowUserToResizeColumns = false;
            bottomAmountList.AllowUserToResizeRows = false;
            bottomAmountListSolidWaste.RowHeadersVisible = false;
            bottomAmountListSolidWaste.AllowUserToAddRows = false;
            bottomAmountListSolidWaste.AllowUserToResizeColumns = false;
            bottomAmountListSolidWaste.AllowUserToResizeRows = false;

            bottomAmountListSolidWaste.ColumnHeadersVisible = false;
            bottomAmountList.ColumnHeadersVisible = false;

        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private Dictionary<string, decimal> GetItemPrices()
        {
            var prices = new Dictionary<string, decimal>();

            string query = @"
        SELECT im.ItemDescription, pl.PRICE
        FROM ItemMasterfile im
        JOIN PriceList pl ON im.ItemID = pl.ItemID
    ";

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var cmd = new MySqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string desc = reader.GetString("ItemDescription");
                        decimal price = reader.GetDecimal("PRICE");
                        prices[desc] = price;
                    }
                }
            }

            return prices;
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
     
            DialogResult result = MessageBox.Show(
                "Are you sure you want to exit? Please save all the processes before leaving.", "Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void StyleGridOnClick(DataGridView grid, int rowIndex, int colIndex)
        {
            if (rowIndex < 0 || colIndex < 0) return;


            foreach (DataGridViewRow row in grid.Rows)
            {
                row.DefaultCellStyle.BackColor = Color.White;
                row.DefaultCellStyle.Font = new Font(grid.Font, FontStyle.Regular);
            }

            foreach (DataGridViewColumn col in grid.Columns)
            {
                col.DefaultCellStyle.BackColor = Color.White;
            }


            grid.Columns[colIndex].DefaultCellStyle.BackColor = Color.FromArgb(235, 245, 255);

            grid.Rows[rowIndex].DefaultCellStyle.BackColor = Color.FromArgb(220, 235, 250);
            grid.Rows[rowIndex].DefaultCellStyle.Font =
                new Font(grid.Font, FontStyle.Bold);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void InitializeGrids()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                
                SolidWasteSummarydataGrid.Columns.Clear();
                SolidWasteSummarydataGrid.Rows.Clear();
                bottomAmountListSolidWaste.Columns.Clear();
                bottomAmountListSolidWaste.Rows.Clear();

                string solidWasteQuery = @"
            SELECT ItemID, ItemDescription 
            FROM ItemMasterfile im
            JOIN CategoryMasterfile cm ON im.CategoryID = cm.CategoryID
            WHERE cm.CategoryDescription <> 'Wires'
            ORDER BY cm.CategoryID, im.ItemID;
        ";

                using (var cmd = new MySqlCommand(solidWasteQuery, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string itemDesc = reader.GetString("ItemDescription");

                        int colIdx = SolidWasteSummarydataGrid.Columns.Add(itemDesc, itemDesc);
                        SolidWasteSummarydataGrid.Columns[colIdx].HeaderCell.Style.BackColor = Color.LightBlue;
                        SolidWasteSummarydataGrid.Columns[colIdx].HeaderCell.Style.Font = new Font(SolidWasteSummarydataGrid.Font, FontStyle.Bold);
                        SolidWasteSummarydataGrid.Columns[colIdx].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        bottomAmountListSolidWaste.Columns.Add(itemDesc, itemDesc);
                    }
                }

                bottomAmountListSolidWaste.Rows.Add(6); // Total, Price, Price/Net VAT, NET of VAT, Add VAT, Total Amount

                // ===== Wires Grid =====
                WiresSummarydataGrid.Columns.Clear();
                WiresSummarydataGrid.Rows.Clear();
                bottomAmountList.Columns.Clear();
                bottomAmountList.Rows.Clear();

                string wiresQuery = @"
            SELECT ItemID, ItemDescription 
            FROM ItemMasterfile im
            JOIN CategoryMasterfile cm ON im.CategoryID = cm.CategoryID
            WHERE cm.CategoryDescription = 'Wires'
            ORDER BY im.ItemID;
        ";

                using (var cmd = new MySqlCommand(wiresQuery, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string itemDesc = reader.GetString("ItemDescription");

                        int colIdx = WiresSummarydataGrid.Columns.Add(itemDesc, itemDesc);
                        WiresSummarydataGrid.Columns[colIdx].HeaderCell.Style.BackColor = Color.LightYellow;
                        WiresSummarydataGrid.Columns[colIdx].HeaderCell.Style.Font = new Font(WiresSummarydataGrid.Font, FontStyle.Bold);
                        WiresSummarydataGrid.Columns[colIdx].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                        bottomAmountList.Columns.Add(itemDesc, itemDesc);
                    }
                }

                bottomAmountList.Rows.Add(6); // Total, Price, Price/Net VAT, NET of VAT, Add VAT, Total Amount

         
                OtherScrapsSummary.Columns.Clear();
                OtherScrapsSummary.Rows.Clear();
                OtherScrapsAmount.Columns.Clear();
                OtherScrapsAmount.Rows.Clear();
            }
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void SolidWasteSummarydataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StyleGridOnClick(SolidWasteSummarydataGrid, e.RowIndex, e.ColumnIndex);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void bottomAmountListSolidWaste_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            StyleGridOnClick(bottomAmountListSolidWaste, e.RowIndex, e.ColumnIndex);

            if (e.RowIndex == 0) // Total
                bottomAmountListSolidWaste.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            else if (e.RowIndex == 1) 
                bottomAmountListSolidWaste.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            else if (e.RowIndex == 2) // Price
                bottomAmountListSolidWaste.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            else if (e.RowIndex == 3) // Net VAT row 
                bottomAmountListSolidWaste.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void WiresSummarydataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StyleGridOnClick(WiresSummarydataGrid, e.RowIndex, e.ColumnIndex);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void bottomAmountList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            StyleGridOnClick(bottomAmountList, e.RowIndex, e.ColumnIndex);

            if (e.RowIndex == 0) // Total
                bottomAmountList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGray;
            else if (e.RowIndex == 1) // Blank row
                bottomAmountList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            else if (e.RowIndex == 2) // Price
                bottomAmountList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            else if (e.RowIndex == 3) // Net VAT
                bottomAmountList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void OtherScrapsSummary_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            StyleGridOnClick(OtherScrapsSummary, e.RowIndex, e.ColumnIndex);
        }
        private void RefreshAllTransactionData()
        {
            if (availableTransaction.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a transaction first.",
                    "No Transaction Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            string selectedTransactionNo = availableTransaction.SelectedItem.ToString();

            // Reload main data grids
            LoadCategoryColumns();              
            LoadCategoryColumnForWires();       
            LoadOtherScrapsSummary();         

            TotalValueBottomAmountSolidWaste();
            TotalValueBottomAmountWires();

        
            LoadHaulingDateAndCustomerName(selectedTransactionNo);


            ClearGridSelections();
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ClearGridSelections()
        {
            SolidWasteSummarydataGrid.ClearSelection();
            bottomAmountListSolidWaste.ClearSelection();
            WiresSummarydataGrid.ClearSelection();
            bottomAmountList.ClearSelection();
            OtherScrapsSummary.ClearSelection();
            OtherScrapsAmount.ClearSelection();
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void OtherScrapsAmount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            StyleGridOnClick(OtherScrapsAmount, e.RowIndex, e.ColumnIndex);

            if (e.RowIndex == 0) // Total row
                OtherScrapsAmount.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeGrids();

            LoadCategoryColumns();
            LoadCategoryColumnForWires();
            LoadTransactionNumbers();
            //======================================================
            refreshTransactionList.AutoPopDelay = 50000;
            refreshTransactionList.InitialDelay = 300;
            refreshTransactionList.ReshowDelay = 500;
            refreshTransactionList.ShowAlways = true;

            refreshTransactionList.SetToolTip(RefreshForTransactionLists,"Refresh the Transaction combo lists");
            //======================================================
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadCategoryColumns()
        {
            if (availableTransaction.SelectedItem == null)
                return;

            string selectedTransactionNo = availableTransaction.SelectedItem.ToString();

            string itemsQuery = @"
        SELECT im.ItemID, im.ItemDescription
        FROM ItemMasterfile im
        JOIN CategoryMasterfile cm ON im.CategoryID = cm.CategoryID
        WHERE cm.CategoryDescription <> 'Wires'
        ORDER BY cm.CategoryID, im.ItemID";

            string transactionQuery = @"
        SELECT td.TransactionID, td.ItemID, td.Weight, td.IsVoid
        FROM TransactionDetails td
        JOIN TransactionMasterfile tm ON td.TransactionID = tm.Transaction_ID
        JOIN ItemMasterfile im ON td.ItemID = im.ItemID
        JOIN CategoryMasterfile cm ON im.CategoryID = cm.CategoryID
        WHERE cm.CategoryDescription <> 'Wires'
          AND td.IsVoid = 0
          AND tm.`Transaction_No.` = @TransactionNo
        ORDER BY td.TransactionDetailID";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    
                    connection.Open();

                    SolidWasteSummarydataGrid.Columns.Clear();
                    SolidWasteSummarydataGrid.Rows.Clear();

                    var itemColumns = new Dictionary<int, int>();

                  
                    using (var cmd = new MySqlCommand(itemsQuery, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int itemId = reader.GetInt32("ItemID");
                            string itemDesc = reader.GetString("ItemDescription");

                            int colIdx = SolidWasteSummarydataGrid.Columns.Add(itemDesc, itemDesc);
                            SolidWasteSummarydataGrid.Columns[colIdx].HeaderCell.Style.BackColor = Color.LightBlue;
                            SolidWasteSummarydataGrid.Columns[colIdx].HeaderCell.Style.Font =
                                new Font(SolidWasteSummarydataGrid.Font, FontStyle.Bold);
                            SolidWasteSummarydataGrid.Columns[colIdx].HeaderCell.Style.Alignment =
                                DataGridViewContentAlignment.MiddleCenter;

                            itemColumns[itemId] = colIdx;
                        }
                    }

                    int isVoidColIdx = SolidWasteSummarydataGrid.Columns.Add("IsVoid", "IsVoid");
                    SolidWasteSummarydataGrid.Columns[isVoidColIdx].Visible = false;

               
                    using (var cmd = new MySqlCommand(transactionQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@TransactionNo", selectedTransactionNo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int itemId = reader.GetInt32("ItemID");
                                decimal weight = reader.IsDBNull(reader.GetOrdinal("Weight")) ? 0m : reader.GetDecimal("Weight");
                                int isVoid = reader.GetInt32("IsVoid");

                                var row = new DataGridViewRow();
                                row.CreateCells(SolidWasteSummarydataGrid);

                                foreach (var kvp in itemColumns)
                                {
                                    int colIdx = kvp.Value;
                                    if (kvp.Key == itemId)
                                        row.Cells[colIdx].Value = weight == 0m ? "" : weight.ToString();
                                    else
                                        row.Cells[colIdx].Value = "";
                                }

                                // Store IsVoid flag
                                row.Cells[isVoidColIdx].Value = isVoid;

                                SolidWasteSummarydataGrid.Rows.Add(row);
                            }
                        }
                    }

                    SolidWasteSummarydataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Items: " + ex.Message);
                }
            }
            TotalValueBottomAmountSolidWaste();
        }

        //===============The totalVAlueBottomAmountSolidWaste is used to view total for each designated column from the SolidWasteSummarydataGrid=====================
        private void TotalValueBottomAmountSolidWaste()
        {
            bottomAmountListSolidWaste.Columns.Clear();
            bottomAmountListSolidWaste.Rows.Clear();

            var prices = GetItemPrices();

            foreach (DataGridViewColumn col in SolidWasteSummarydataGrid.Columns)
            {
                if (col.Name != "IsVoid")
                    bottomAmountListSolidWaste.Columns.Add(col.Name, col.HeaderText);
            }

            int totalRow = bottomAmountListSolidWaste.Rows.Add();   // Total KG
            int blankRow = bottomAmountListSolidWaste.Rows.Add();   // Blank
            int priceRow = bottomAmountListSolidWaste.Rows.Add();   // Price
            int netRow = bottomAmountListSolidWaste.Rows.Add();     // Net Amount
            int vatRow = bottomAmountListSolidWaste.Rows.Add();     // VAT
            int totalAmountRow = bottomAmountListSolidWaste.Rows.Add(); // TOTAL AMOUNT

            for (int colIdx = 0; colIdx < SolidWasteSummarydataGrid.Columns.Count; colIdx++)
            {
                if (SolidWasteSummarydataGrid.Columns[colIdx].Name == "IsVoid")
                    continue;

                decimal sum = 0m;

                foreach (DataGridViewRow row in SolidWasteSummarydataGrid.Rows)
                {
                    if (row.Cells["IsVoid"].Value?.ToString() == "1")
                        continue;

                    if (row.Cells[colIdx].Value != null &&
                        decimal.TryParse(row.Cells[colIdx].Value.ToString(), out decimal val))
                    {
                        sum += val;
                    }
                }

                bottomAmountListSolidWaste.Rows[totalRow].Cells[colIdx].Value =
                    sum == 0m ? "" : sum.ToString("N2");

                if (prices.ContainsKey(SolidWasteSummarydataGrid.Columns[colIdx].HeaderText))
                {
                    decimal price = prices[SolidWasteSummarydataGrid.Columns[colIdx].HeaderText];
                    decimal netAmount = decimal.Round(sum * price, 2, MidpointRounding.AwayFromZero);
                    decimal vatAmount = decimal.Round(netAmount * 0.12m, 2, MidpointRounding.AwayFromZero);
                    decimal totalAmount = netAmount + vatAmount;


                    bottomAmountListSolidWaste.Rows[priceRow].Cells[colIdx].Value = price.ToString("N2");
                    bottomAmountListSolidWaste.Rows[netRow].Cells[colIdx].Value = netAmount.ToString("N2");
                    bottomAmountListSolidWaste.Rows[vatRow].Cells[colIdx].Value = vatAmount.ToString("N2");
                    bottomAmountListSolidWaste.Rows[totalAmountRow].Cells[colIdx].Value = totalAmount.ToString("N2");

                }
            }

            bottomAmountListSolidWaste.Rows[totalRow].DefaultCellStyle.BackColor = Color.LightGray;
            bottomAmountListSolidWaste.Rows[blankRow].DefaultCellStyle.BackColor = Color.White;
            bottomAmountListSolidWaste.Rows[priceRow].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            bottomAmountListSolidWaste.Rows[netRow].DefaultCellStyle.BackColor = Color.LightGreen;
            bottomAmountListSolidWaste.Rows[vatRow].DefaultCellStyle.BackColor = Color.LightPink;

            bottomAmountListSolidWaste.Rows[totalAmountRow].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            bottomAmountListSolidWaste.Rows[totalAmountRow].DefaultCellStyle.Font =
                new Font(bottomAmountListSolidWaste.Font, FontStyle.Bold);

            bottomAmountListSolidWaste.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            bottomAmountListSolidWaste.ColumnHeadersVisible = false;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadOtherScrapsSummary()
        {
            if (availableTransaction.SelectedItem == null)
                return;

            string selectedTransactionNo = availableTransaction.SelectedItem.ToString();

            string itemQuery = @"
        SELECT DISTINCT td.OtherScraps
        FROM TransactionDetails td
        JOIN TransactionMasterfile tm ON td.TransactionID = tm.Transaction_ID
        WHERE td.OtherScraps IS NOT NULL
          AND td.OtherScraps <> ''
          AND tm.`Transaction_No.` = @TransactionNo
          AND tm.IsCancel = 0
        ORDER BY td.OtherScraps";

            string dataQuery = @"
        SELECT td.OtherScraps, td.Weight, td.IsVoid
        FROM TransactionDetails td
        JOIN TransactionMasterfile tm ON td.TransactionID = tm.Transaction_ID
        WHERE td.OtherScraps IS NOT NULL
          AND td.OtherScraps <> ''
          AND td.IsVoid = 0
          AND tm.`Transaction_No.` = @TransactionNo
          AND tm.IsCancel = 0
        ORDER BY td.TransactionDetailID";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    OtherScrapsSummary.Columns.Clear();
                    OtherScrapsSummary.Rows.Clear();

                    Dictionary<string, int> columnMap = new Dictionary<string, int>();

        
                    using (var cmd = new MySqlCommand(itemQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@TransactionNo", selectedTransactionNo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string scrapName = reader.GetString("OtherScraps");

                                int colIndex = OtherScrapsSummary.Columns.Add(scrapName, scrapName);
                                OtherScrapsSummary.Columns[colIndex].HeaderCell.Style.Font =
                                    new Font(OtherScrapsSummary.Font, FontStyle.Bold);
                                OtherScrapsSummary.Columns[colIndex].HeaderCell.Style.Alignment =
                                    DataGridViewContentAlignment.MiddleCenter;

                                columnMap[scrapName] = colIndex;
                            }
                        }
                    }

                 
                    using (var cmd = new MySqlCommand(dataQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@TransactionNo", selectedTransactionNo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string scrapName = reader.GetString("OtherScraps");
                                decimal weight = reader.GetDecimal("Weight");

                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(OtherScrapsSummary);

                                foreach (var kvp in columnMap)
                                {
                                    if (kvp.Key == scrapName)
                                        row.Cells[kvp.Value].Value = weight == 0m ? "" : weight.ToString();
                                    else
                                        row.Cells[kvp.Value].Value = "";
                                }

                                OtherScrapsSummary.Rows.Add(row);
                            }
                        }
                    }

                    OtherScrapsSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Other Scraps: " + ex.Message);
                }
            }

    
            LoadOtherScrapsAmount();
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadOtherScrapsAmount()
        {
            OtherScrapsAmount.Columns.Clear();
            OtherScrapsAmount.Rows.Clear();

            if (OtherScrapsSummary.Columns.Count == 0)
                return;

            foreach (DataGridViewColumn col in OtherScrapsSummary.Columns)
            {
                OtherScrapsAmount.Columns.Add(col.Name, col.HeaderText);
            }

            int totalRow = OtherScrapsAmount.Rows.Add();

            for (int colIdx = 0; colIdx < OtherScrapsSummary.Columns.Count; colIdx++)
            {
                decimal sum = 0m;
                foreach (DataGridViewRow row in OtherScrapsSummary.Rows)
                {
                    if (row.Cells[colIdx].Value != null &&
                        decimal.TryParse(row.Cells[colIdx].Value.ToString(), out decimal val))
                    {
                        sum += val;
                    }
                }
                OtherScrapsAmount.Rows[totalRow].Cells[colIdx].Value = sum == 0m ? "" : $"{sum}";
            }

          
            OtherScrapsAmount.Rows[totalRow].DefaultCellStyle.BackColor = Color.LightGreen;

            OtherScrapsAmount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            OtherScrapsAmount.ColumnHeadersVisible = false;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void RefreshForTransactionLists_Click(object sender, EventArgs e)
        {
           
            string previouslySelected = availableTransaction.SelectedItem?.ToString();

          
            LoadTransactionNumbers();

          
            if (!string.IsNullOrEmpty(previouslySelected) &&
                availableTransaction.Items.Contains(previouslySelected))
            {
                availableTransaction.SelectedItem = previouslySelected;
            }
            else if (availableTransaction.Items.Count > 0)
            {
                //select first item
                availableTransaction.SelectedIndex = 0;
            }

           
            availableTransaction_SelectedIndexChanged(
                availableTransaction,
                EventArgs.Empty
                
               
            );
        }


        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadCategoryColumnForWires()
        {
            if (availableTransaction.SelectedItem == null)
                return;

            string selectedTransactionNo = availableTransaction.SelectedItem.ToString();

            string itemQuery = @"
        SELECT im.ItemID, im.ItemDescription
        FROM ItemMasterfile im
        JOIN CategoryMasterfile cm ON im.CategoryID = cm.CategoryID
        WHERE cm.CategoryDescription = 'Wires'
        ORDER BY im.ItemID";

            string transactionQuery = @"
        SELECT td.TransactionDetailID, td.ItemID, td.Weight
        FROM TransactionDetails td
        JOIN TransactionMasterfile tm ON td.TransactionID = tm.Transaction_ID
        JOIN ItemMasterfile im ON td.ItemID = im.ItemID
        JOIN CategoryMasterfile cm ON im.CategoryID = cm.CategoryID
        WHERE cm.CategoryDescription = 'Wires'
          AND td.IsVoid = 0   
          AND tm.`Transaction_No.` = @TransactionNo
        ORDER BY td.TransactionDetailID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    WiresSummarydataGrid.Columns.Clear();
                    WiresSummarydataGrid.Rows.Clear();
                    var wireColumns = new Dictionary<int, int>();

                    // Add item columns
                    using (var itemCmd = new MySqlCommand(itemQuery, connection))
                    using (var itemReader = itemCmd.ExecuteReader())
                    {
                        while (itemReader.Read())
                        {
                            int itemId = itemReader.GetInt32("ItemID");
                            string itemDesc = itemReader.GetString("ItemDescription");

                            int colIdx = WiresSummarydataGrid.Columns.Add(itemDesc, itemDesc);
                            WiresSummarydataGrid.Columns[colIdx].HeaderCell.Style.BackColor = Color.LightYellow;
                            WiresSummarydataGrid.Columns[colIdx].HeaderCell.Style.Font =
                                new Font(WiresSummarydataGrid.Font, FontStyle.Bold);
                            WiresSummarydataGrid.Columns[colIdx].HeaderCell.Style.Alignment =
                                DataGridViewContentAlignment.MiddleCenter;

                            wireColumns[itemId] = colIdx;
                        }
                    }

              
                    using (var transCmd = new MySqlCommand(transactionQuery, connection))
                    {
                        transCmd.Parameters.AddWithValue("@TransactionNo", selectedTransactionNo);

                        using (var transReader = transCmd.ExecuteReader())
                        {
                            while (transReader.Read())
                            {
                                int itemId = transReader.GetInt32("ItemID");
                                decimal weight = transReader.IsDBNull(transReader.GetOrdinal("Weight")) ? 0m : transReader.GetDecimal("Weight");

                                var row = new DataGridViewRow();
                                row.CreateCells(WiresSummarydataGrid);

                                foreach (var kvp in wireColumns)
                                {
                                    int colIdx = kvp.Value;
                                    if (kvp.Key == itemId)
                                        row.Cells[colIdx].Value = weight == 0m ? "" : weight.ToString();
                                    else
                                        row.Cells[colIdx].Value = "";
                                }

                                WiresSummarydataGrid.Rows.Add(row);
                            }
                        }
                    }

                    WiresSummarydataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading Wires: " + ex.Message);
                }
            }
            TotalValueBottomAmountWires();
        }

        //===============The totalVAlueBottomAmountWires is used to view total,Price, and etc,. for each designated column from the SolidWasteSummarydataGrid=====================
        private void TotalValueBottomAmountWires()
        {
            bottomAmountList.Rows.Clear();
            bottomAmountList.Columns.Clear();

            if (WiresSummarydataGrid.Columns.Count == 0)
                return;

            var prices = GetItemPrices();

            foreach (DataGridViewColumn col in WiresSummarydataGrid.Columns)
            {
                bottomAmountList.Columns.Add(col.Name, col.HeaderText);
            }

            int totalRow = bottomAmountList.Rows.Add();   // Total KG
            int blankRow = bottomAmountList.Rows.Add();   // Blank
            int priceRow = bottomAmountList.Rows.Add();   // Price
            int netRow = bottomAmountList.Rows.Add();     // Net Amount
            int vatRow = bottomAmountList.Rows.Add();     // VAT
            int totalAmountRow = bottomAmountList.Rows.Add(); // TOTAL AMOUNT

            for (int colIdx = 0; colIdx < WiresSummarydataGrid.Columns.Count; colIdx++)
            {
                decimal sum = 0m;

                foreach (DataGridViewRow row in WiresSummarydataGrid.Rows)
                {
                    if (row.Cells[colIdx].Value != null &&
                        decimal.TryParse(row.Cells[colIdx].Value.ToString(), out decimal val))
                    {
                        sum += val;
                    }
                }

                bottomAmountList.Rows[totalRow].Cells[colIdx].Value =
                    sum == 0m ? "" : sum.ToString("N2");

                string itemName = WiresSummarydataGrid.Columns[colIdx].HeaderText;

                if (prices.ContainsKey(itemName))
                {
                    decimal price = prices[itemName];
                    decimal netAmount = decimal.Round(sum * price, 2, MidpointRounding.AwayFromZero);
                    decimal vatAmount = decimal.Round(netAmount * 0.12m, 2, MidpointRounding.AwayFromZero);
                    decimal totalAmount = netAmount + vatAmount;


                    bottomAmountList.Rows[priceRow].Cells[colIdx].Value = price.ToString("N2");
                    bottomAmountList.Rows[netRow].Cells[colIdx].Value = netAmount.ToString("N2");
                    bottomAmountList.Rows[vatRow].Cells[colIdx].Value = vatAmount.ToString("N2");
                    bottomAmountList.Rows[totalAmountRow].Cells[colIdx].Value = totalAmount.ToString("N2");
                }
            }

            bottomAmountList.Rows[totalRow].DefaultCellStyle.BackColor = Color.LightGray;
            bottomAmountList.Rows[blankRow].DefaultCellStyle.BackColor = Color.White;
            bottomAmountList.Rows[priceRow].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
            bottomAmountList.Rows[netRow].DefaultCellStyle.BackColor = Color.LightGreen;
            bottomAmountList.Rows[vatRow].DefaultCellStyle.BackColor = Color.LightPink;

            bottomAmountList.Rows[totalAmountRow].DefaultCellStyle.BackColor = Color.LightSkyBlue;
            bottomAmountList.Rows[totalAmountRow].DefaultCellStyle.Font =
                new Font(bottomAmountList.Font, FontStyle.Bold);

            bottomAmountList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            bottomAmountList.ColumnHeadersVisible = false;
        }



        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void availableTransaction_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (availableTransaction.SelectedItem != null)
            {
                string selectedTransactionNo = availableTransaction.SelectedItem.ToString();
                LoadCategoryColumns();
                LoadCategoryColumnForWires();
                LoadOtherScrapsSummary();
                LoadHaulingDateAndCustomerName(selectedTransactionNo);
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void downloadUpdatedDataToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult showDat = MessageBox.Show(
             "Download the database from the server to sync updated personnel, customers, box types, and scrap items.\r\n" +
             "Do not sync if a current transaction is on going in Digital Weighing Scraps System. Sync before starting new ones.\n" +
             "Use a flash drive for updates.\n\n" +
             "Proceed?",
             "Download Updated Database",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);


            if (showDat != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // AES Encryption Key & IV
                byte[] AES_KEY = Encoding.UTF8.GetBytes("A1B2C3D4E5F6G7H8");
                byte[] AES_IV = Encoding.UTF8.GetBytes("H8G7F6E5D4C3B2A1");

                string connectionStringServer =
                    ConfigurationManager.ConnectionStrings["ServerPathDbConnection"].ConnectionString;

                string[] tables = new string[]
                {
            "CustomerMasterfile",
            "AdminStaffMasterfile",
            "CustomerTypeMasterfile",
            "AccessTypeMasterfile",
            "InChargePersonnelDetails",
            "ItemMasterfile",
            "CategoryMasterfile",
            "BoxTypeMasterfile",
            "PriceList"
                };

                StringBuilder sqlDump = new StringBuilder();
                sqlDump.AppendLine("-- Encrypted Scrap Database Export");
                sqlDump.AppendLine("-- Generated: " + DateTime.Now);
                sqlDump.AppendLine("SET FOREIGN_KEY_CHECKS = 0;");
                sqlDump.AppendLine();

                using (var connection = new MySqlConnection(connectionStringServer))
                {
                    connection.Open();

                    foreach (var table in tables)
                    {
                        DataTable dt = new DataTable();
                        using (var cmd = new MySqlCommand($"SELECT * FROM {table}", connection))
                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }

                        sqlDump.AppendLine($"-- Table: {table}");
                        sqlDump.AppendLine(ConvertTableToSQL(dt, table));
                        sqlDump.AppendLine();
                    }
                }

                sqlDump.AppendLine("SET FOREIGN_KEY_CHECKS = 1;");

                //  Encrypt SQL
                byte[] encryptedData = EncryptAES(sqlDump.ToString(), AES_KEY, AES_IV);


                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Title = "Save Encrypted Scrap Database";
                    sfd.Filter = "Scrap Encrypted File (*.scrapenc)|*.scrapenc";
                    sfd.FileName = $"Scrap_UpdatedData_{DateTime.Now:yyyyMMdd_HHmmss}.scrapenc";
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, encryptedData);

                        MessageBox.Show(
                            $"Database downloaded and encrypted successfully:\n{sfd.FileName}",
                            "Download Complete",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information
                        );
                       
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error downloading data: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ---------------- Helper Functions ----------------
        private string ConvertTableToSQL(DataTable dt, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                sb.Append($"INSERT INTO `{tableName}` (");
                sb.Append(string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c => $"`{c.ColumnName}`")));
                sb.Append(") VALUES (");
                sb.Append(string.Join(", ", dt.Columns.Cast<DataColumn>().Select(c =>
                {
                    object val = row[c];
                    if (val == DBNull.Value)
                        return "NULL";
                    if (val is string || val is DateTime)
                        return $"'{MySqlHelper.EscapeString(val.ToString())}'";
                    return val.ToString().Replace(",", ".");
                })));
                sb.AppendLine(");");
            }
            return sb.ToString();
        }

        private byte[] EncryptAES(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var ms = new MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cryptoStream))
                {
                    writer.Write(plainText);
                    writer.Close();
                    return ms.ToArray();
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ShowControl(UserControl control)
        {

            HomePanel.Controls.Clear();
            control.Dock = DockStyle.Fill;
            HomePanel.Controls.Add(control);
        }
        //===================Below is Hookup Menubars to distinct User Control panel=========================================================================

        private void UpdateMasterfile_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "⚠️ WARNING:\n\nYou are about to open the Record Management Panel.\n" +
                "Changes made here (Add, Edit, or Delete) will directly modify the\n" +
                "Customer, Scrap Item, and Category records.\n\n" +
                "Proceed only if you understand these actions cannot be undone.",
                "Record Management Warning",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                HomePanel.Visible = true;          
                HomePanel.Controls.Clear();        
                HomePanel.Dock = DockStyle.Fill;
                ShowControl(new UpdateMasterfileControl1());
            }
        }

        //=========================================================
        private void inchargeUserAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult showDat = MessageBox.Show(
                      "You are about to open the 'In-Charge Account' settings.\n\n" +
                      "This tab is for managing accounts used by In-Charge personnel in the Digital Weighing System Windows application.\n\n" +
                      "Do you want to continue with this setting?",
                      "In-Charge Account Settings",
                         MessageBoxButtons.YesNo,
                         MessageBoxIcon.Warning);

            if (showDat == DialogResult.Yes)
            {
                HomePanel.Visible = true;
                HomePanel.Controls.Clear();
                ShowControl(new InChargeAccountManagementControl());
                HomePanel.Dock = DockStyle.Fill;

            }

        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void GenerateReport_Click(object sender, EventArgs e)
        {
   
            HomePanel.Visible = true;
            HomePanel.Controls.Clear();
            ShowControl(new GenerateReport());
            HomePanel.Dock = DockStyle.Fill;
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadTransactionNumbers()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT `Transaction_No.` FROM TransactionMasterfile WHERE IsCancel = 0";
                using (var cmd = new MySqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    availableTransaction.Items.Clear();
                    while (reader.Read())
                    {
                        availableTransaction.Items.Add(reader.GetString("Transaction_No."));
                    }
                    
                }
            }
            
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ImportTransactionData_Click(object sender, EventArgs e)
        {
            HomePanel.Visible = true;         
            HomePanel.Controls.Clear();       

            ImportTransactionPanel1 panel = new ImportTransactionPanel1();
            panel.LoadTransactionData();         
            ShowControl(panel);                  
            HomePanel.Dock = DockStyle.Fill;
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void homeToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            HomePanel.Visible = false;
            HomePanel.Controls.Clear();
            LoadTransactionNumbers();


            LoadCategoryColumns();
            LoadCategoryColumnForWires();

            UpdateMasterfile.Enabled = true;
            ImportTransactionData.Enabled = true;
            GenerateReport.Enabled = true;
            UpdateAndEditPrice.Enabled = true;


        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadHaulingDateAndCustomerName(string transactionNo)
        {
            string query = @"
        SELECT 
            tm.TransactionDate,
            cm.CustomerName
        FROM TransactionMasterfile tm
        JOIN CustomerMasterfile cm 
            ON tm.CustomerID = cm.Customer_ID
        WHERE tm.`Transaction_No.` = @TransactionNo
          AND tm.IsCancel = 0
        LIMIT 1;
    ";

            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TransactionNo", transactionNo);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                  
                                if (!reader.IsDBNull(reader.GetOrdinal("TransactionDate")))
                                {
                                    DateTime haulingDate = reader.GetDateTime("TransactionDate");
                                    HaulingDate.Text = haulingDate.ToString("yyyy-MM-dd");
                                    HaulingDate.ForeColor = Color.Blue;   
                                }
                                else
                                {
                                    HaulingDate.Text = string.Empty;
                                }

                                // Customer Name
                                CustomerName.Text = reader["CustomerName"].ToString();
                                CustomerName.ForeColor = Color.Blue;     
                            }
                            else
                            {
                                HaulingDate.Text = string.Empty;
                                CustomerName.Text = string.Empty;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "Error loading Hauling Date and Customer Name:\n" + ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
        }

        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void buttonRefresh_Click(object sender, EventArgs e) {

            RefreshAllTransactionData();
        

        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void HomePanel_Paint(object sender, PaintEventArgs e)
        {
            
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void UpdateAndEditPrice_Click(object sender, EventArgs e)
        {
            UpdatePriceForm1 UpdateValue = new UpdatePriceForm1();
            UpdateValue.ShowDialog();
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //Keep these voids for future functions
        private void About_Click(object sender, EventArgs e) { }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e){ }
        private void HaulingDate_Click(object sender, EventArgs e){ } 
        private void CustomerName_Click(object sender, EventArgs e){ }
        private void DRNumber_Click(object sender, EventArgs e){ }
        private void VAT_TextChanged(object sender, EventArgs e){ }
        private void GrandTotalKG_TextChanged(object sender, EventArgs e){ }
        private void NetVAT_TextChanged(object sender, EventArgs e){ }
        private void TotalAmount_Click(object sender, EventArgs e){ }
        private void EditResult_Click(object sender, EventArgs e){ }
        private void ExportToExcel_Click(object sender, EventArgs e){ }
        private void PrintReport_Click(object sender, EventArgs e){ }
        private void PreparedBy_TextChanged(object sender, EventArgs e){ }
        private void CheckedBy_TextChanged(object sender, EventArgs e){ }
        private void tabPage2_Click(object sender, EventArgs e){ }
        private void timer1_Tick(object sender, EventArgs e){ }
        private void HaulingDate_Click_1(object sender, EventArgs e){ }
        private void CustomerName_Click_1(object sender, EventArgs e){ }
        private void DRNumber_Click_1(object sender, EventArgs e){ }

    }
}
