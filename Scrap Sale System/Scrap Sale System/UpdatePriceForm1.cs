using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Configuration;

namespace Scrap_Sale_System
{
    public partial class UpdatePriceForm1 : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ServerPathDbConnection"].ConnectionString;

        //string connectionString = "server=10.0.253.60;user=root;password=Windows7;database=Scraps";
        // string connectionString = "server=localhost;user=root;password=Windows7;database=Scraps";
        //  string connectionString = "server=localhost;user=root;password=Windows7;database=Scraps";

        private DataTable _itemsTable;          
        private BindingSource _bs = new BindingSource();




        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public UpdatePriceForm1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;

            PriceList.ReadOnly = false;
            radSolidWaste.CheckedChanged += radSolidWaste_CheckedChanged;
            radWires.CheckedChanged += radWires_CheckedChanged;

            PriceList.AllowUserToResizeColumns = false;
            PriceList.AllowUserToResizeRows = false;
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void UpdatePriceForm1_Load(object sender, EventArgs e)
        {
            radSolidWaste.Checked = true;
            RefreshListFromRadios();
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void radSolidWaste_CheckedChanged(object sender, EventArgs e) {

            if (radSolidWaste.Checked)
            {
                RefreshListFromRadios();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void radWires_CheckedChanged(object sender, EventArgs e) {

            if (radWires.Checked)
            {
                RefreshListFromRadios();
            }
        
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void RefreshListFromRadios()
        {
            bool showOnlyWires = radWires.Checked;
            LoadItemList(showOnlyWires);
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadItemList(bool showOnlyWires)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    string query = @"
                SELECT 
                    i.ItemID,
                    i.ItemDescription,
                    COALESCE(p.Price, 0.00) AS Price,
                    c.CategoryDescription
                FROM ItemMasterfile i
                JOIN CategoryMasterfile c ON i.CategoryID = c.CategoryID
                LEFT JOIN PriceList p ON i.ItemID = p.ItemID
                WHERE c.CategoryDescription " + (showOnlyWires ? "= @cat" : "<> @cat") + @"
                ORDER BY i.ItemDescription;";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@cat", "Wires");

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            _itemsTable = new DataTable();
                            adapter.Fill(_itemsTable);

                            
                            _bs.DataSource = _itemsTable;
                            PriceList.DataSource = _bs;

                           
                            PriceList.Columns["ItemID"].HeaderText = "Item ID";
                            PriceList.Columns["ItemDescription"].HeaderText = "Item Description";
                            PriceList.Columns["Price"].HeaderText = "Price";

                            PriceList.Columns["CategoryDescription"].Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading item list. Please call I.T programmer: " + ex.Message);
                }
            }
        }


        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void PriceList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < PriceList.Rows.Count)
            {
                var row = PriceList.Rows[e.RowIndex];
                var itemDesc = row.Cells["ItemDescription"].Value?.ToString() ?? "";
                var price = row.Cells["Price"].Value?.ToString() ?? "0";
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void SearchItem_TextChanged(object sender, EventArgs e)
        {
            if (_itemsTable == null || _bs == null) return;

            string searchText = SearchItem.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(searchText))
            {
               
                _bs.RemoveFilter();
                
                if (PriceList.Rows.Count > 0)
                    PriceList.ClearSelection();
                return;
            }

           
            string escaped = searchText.Replace("'", "''");

    
            _bs.Filter = $"Convert(ItemDescription, 'System.String') LIKE '%{escaped}%'";

       
            if (PriceList.Rows.Count > 0)
            {
                PriceList.ClearSelection();
                PriceList.Rows[0].Selected = true;
                if (PriceList.Rows[0].Cells.Count > 0)
                {
                    PriceList.CurrentCell = PriceList.Rows[0].Cells[0];
                    var args = new DataGridViewCellEventArgs(PriceList.CurrentCell.ColumnIndex, PriceList.CurrentCell.RowIndex);
                    PriceList_CellContentClick(PriceList, args);
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                try
                {
                    foreach (DataGridViewRow row in PriceList.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int itemId = Convert.ToInt32(row.Cells["ItemID"].Value);
                        decimal price = 0;

                      
                        if (row.Cells["Price"].Value != DBNull.Value && row.Cells["Price"].Value != null)
                        {
                            decimal.TryParse(row.Cells["Price"].Value.ToString(), out price);
                        }

                       
                        string query = @"
                         UPDATE PriceList 
                         SET Price = @price 
                         WHERE ItemID = @itemId;

                         INSERT INTO PriceList (ItemID, Price)
                         SELECT @itemId, @price
                         WHERE NOT EXISTS (SELECT 1 FROM PriceList WHERE ItemID = @itemId);
";


                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@itemId", itemId);
                            cmd.Parameters.AddWithValue("@price", price);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Prices updated successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating prices. Please call I.T programmer: " + ex.Message);
                }
            }
        }



    }
}
