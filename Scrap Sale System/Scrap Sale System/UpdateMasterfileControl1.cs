using System.Data;
using System.Windows.Forms;
using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Scrap_Sale_System
{
    public partial class UpdateMasterfileControl1 : UserControl
    {
        private int selectedCustomerID = 0;
        private int selectedItemID = 0;
        private int selectedBoxTypeID = 0;
        
        private readonly MySqlConnection conn;
        // private MySqlConnection conn = new MySqlConnection("server=10.0.253.60;user=root;password=Windows7;database=Scraps");
        //  private MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=Windows7;database=Scraps");
        // private MySqlConnection conn = new MySqlConnection("server=localhost;user=root;password=masterx;database=Scraps");


       
       

        public UpdateMasterfileControl1()
        {
            InitializeComponent();
       
            dataGridViewCustomer.ReadOnly = true;
            dataGridViewScrap.ReadOnly = true;
            dataGridBoxWeight.ReadOnly = true;
            dataGridViewCustomer.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridViewScrap.SelectionChanged += dataGridViewScrap_SelectionChanged;
            dataGridBoxWeight.SelectionChanged += dataGridBoxWeight_SelectionChanged;

            string connectionString = ConfigurationManager.ConnectionStrings["ServerPathDbConnection"].ConnectionString;

            conn = new MySqlConnection(connectionString);
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void HomeControl1_Load(object sender, EventArgs e)
        {
            conn.Open();
            LoadCustomerList();
            LoadScrapItems();
            LoadBoxTypes();
            LoadCategories();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void EnsureConnectionOpen()
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void dataGridBoxWeight_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridBoxWeight.CurrentRow == null)
                return;

            object idValue = dataGridBoxWeight.CurrentRow.Cells["BoxTypeID"].Value;

            if (idValue == null || idValue == DBNull.Value)
            {
                selectedBoxTypeID = 0;
                txtBoxType.Clear();
                txtBoxWeight.Clear();
                return;
            }

            selectedBoxTypeID = Convert.ToInt32(idValue);

            txtBoxType.Text = dataGridBoxWeight.CurrentRow.Cells["BoxTypeDescription"].Value?.ToString() ?? "";
            txtBoxWeight.Text = dataGridBoxWeight.CurrentRow.Cells["BoxWeight"].Value?.ToString() ?? "";
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            if (dataGridViewCustomer.CurrentRow != null)
            {

                object idValue = dataGridViewCustomer.CurrentRow.Cells["Customer_ID"].Value;
                if (idValue == null || idValue == DBNull.Value)
                {

                    selectedCustomerID = 0;
                    txtboxCustomerName.Clear();
                    txtboxAddress.Clear();
                    radCompany.Checked = false;
                    radEmployee.Checked = false;
                    return;
                }

                selectedCustomerID = Convert.ToInt32(idValue);
                txtboxCustomerName.Text = dataGridViewCustomer.CurrentRow.Cells["CustomerName"].Value?.ToString() ?? "";
                txtboxAddress.Text = dataGridViewCustomer.CurrentRow.Cells["CustomerAddress"].Value?.ToString() ?? "";

                object typeValue = dataGridViewCustomer.CurrentRow.Cells["CustomerTypeID"].Value;
                if (typeValue != null && typeValue != DBNull.Value)
                {
                    int typeID = Convert.ToInt32(typeValue);

                    string q = "SELECT CustomerTypeName FROM CustomerTypeMasterfile WHERE CustomerType_ID=@id";
                    MySqlCommand cmd = new MySqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@id", typeID);
                    EnsureConnectionOpen();
                    object typeNameObj = cmd.ExecuteScalar();
                    string typeName = typeNameObj?.ToString() ?? "";

                    radCompany.Checked = (typeName == "Company");
                    radEmployee.Checked = (typeName == "Employee");
                }
                else
                {
                    radCompany.Checked = false;
                    radEmployee.Checked = false;
                }
            }
        }
  
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadCustomerList()
        {

            string query = "SELECT Customer_ID, CustomerName, CustomerAddress, CustomerTypeID FROM CustomerMasterfile";


            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridViewCustomer.DataSource = dt;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int GetCustomerTypeID(string typeName)
        {
            string query = "SELECT CustomerType_ID FROM CustomerTypeMasterfile WHERE CustomerTypeName = @name";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", typeName);
            EnsureConnectionOpen();
            object result = cmd.ExecuteScalar();

            return result != null ? Convert.ToInt32(result) : 0;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadScrapItems()
        {
            string query = @"SELECT 
                        i.ItemID,
                        i.ItemCode,
                        i.ItemDescription,
                        c.CategoryDescription AS Category
                     FROM ItemMasterfile i
                     JOIN CategoryMasterfile c
                     ON i.CategoryID = c.CategoryID";

            EnsureConnectionOpen();
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridViewScrap.DataSource = dt;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadBoxTypes()
        {
            string query = @"SELECT 
                        BoxTypeID,
                        BoxTypeDescription,
                        BoxWeight
                     FROM BoxTypeMasterfile";

            EnsureConnectionOpen();

            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            dataGridBoxWeight.DataSource = dt;
            dataGridBoxWeight.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void LoadCategories()
        {
            EnsureConnectionOpen();
            string query = "SELECT CategoryID, CategoryDescription FROM CategoryMasterfile";
            MySqlDataAdapter da = new MySqlDataAdapter(query, conn);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryType.DataSource = dt;
            CategoryType.DisplayMember = "CategoryDescription"; 
            CategoryType.ValueMember = "CategoryID";            
            CategoryType.SelectedIndex = -1; 
        }
        private void CategoryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CategoryType.SelectedIndex != -1)
            {
               
                string selectedDescription = CategoryType.Text;
          
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int GetOrCreateCategoryID(string categoryName)
        {
            EnsureConnectionOpen();


            string selectQuery = "SELECT CategoryID FROM CategoryMasterfile WHERE CategoryDescription = @name";
            MySqlCommand checkCmd = new MySqlCommand(selectQuery, conn);
            checkCmd.Parameters.AddWithValue("@name", categoryName);

            object result = checkCmd.ExecuteScalar();

            if (result != null)
            {
                return Convert.ToInt32(result);
            }


            string insertQuery = "INSERT INTO CategoryMasterfile (CategoryDescription) VALUES (@name)";
            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
            insertCmd.Parameters.AddWithValue("@name", categoryName);
            insertCmd.ExecuteNonQuery();


            return (int)insertCmd.LastInsertedId;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private int GetCurrentCategoryID(int itemID)
        {
            string query = "SELECT CategoryID FROM ItemMasterfile WHERE ItemID=@id";
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", itemID);
            EnsureConnectionOpen();
            object result = cmd.ExecuteScalar();
            return result != null ? Convert.ToInt32(result) : 0;
        }
      
    
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void dataGridViewScrap_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewScrap.CurrentRow == null)
                return;

            object idValue = dataGridViewScrap.CurrentRow.Cells["ItemID"].Value;

            if (idValue == null || idValue == DBNull.Value)
            {
                selectedItemID = 0;
                txtBoxItemCode.Clear();
                textBoxDescription.Clear();
              
                return;
            }

            selectedItemID = Convert.ToInt32(idValue);

            txtBoxItemCode.Text = dataGridViewScrap.CurrentRow.Cells["ItemCode"].Value?.ToString() ?? "";
            textBoxDescription.Text = dataGridViewScrap.CurrentRow.Cells["ItemDescription"].Value?.ToString() ?? "";
          
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            string name = txtboxCustomerName.Text;
            string address = txtboxAddress.Text;
            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Please enter both name and address.");
                return;
            }
            else if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("No customer name is found in the box. Please fill it");
                return;
            }
            else if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Please input the address");
                return;
            }
            
            string selectedType = radCompany.Checked ? "Company" :
                          radEmployee.Checked ? "Employee" : "";
            if (selectedType == "")
            {
                MessageBox.Show("Please select a customer type (Company or Employee).");
                return;
            }
            int customerTypeID = GetCustomerTypeID(selectedType);

            string query = "INSERT INTO CustomerMasterfile (CustomerName, CustomerAddress, CustomerTypeID) " +
                   "VALUES (@name, @address, @typeID)";
            EnsureConnectionOpen();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@typeID", customerTypeID);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Customer added successfully!");
            LoadCustomerList();

        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
             if (selectedCustomerID == 0)
            {
                MessageBox.Show("Please select a customer from the list provided above.");
                return;
            }

            string name = txtboxCustomerName.Text;
            string address = txtboxAddress.Text;

            string selectedType = radCompany.Checked ? "Company" :
                                  radEmployee.Checked ? "Employee" : "";

            if (selectedType == "")
            {
                MessageBox.Show("Please select a customer type (Company or Employee).");
                return;
            }

            int customerTypeID = GetCustomerTypeID(selectedType);

            string query = "UPDATE CustomerMasterfile SET CustomerName=@name, CustomerAddress=@address, CustomerTypeID=@typeID " +
                           "WHERE Customer_ID=@id";

            EnsureConnectionOpen();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@address", address);
            cmd.Parameters.AddWithValue("@typeID", customerTypeID);
            cmd.Parameters.AddWithValue("@id", selectedCustomerID);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Updated successfully!");
            LoadCustomerList();
        
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            int id = selectedCustomerID;
            if (selectedCustomerID == 0)
            {
                MessageBox.Show("Please select a customer from the list provided above.");
                return;
            }

            string check = "SELECT COUNT(*) FROM TransactionMasterfile WHERE CustomerID=@id";
            EnsureConnectionOpen();
            MySqlCommand chk = new MySqlCommand(check, conn);
            chk.Parameters.AddWithValue("@id", id);
            int count = Convert.ToInt32(chk.ExecuteScalar());

            if (count > 0)
            {
                MessageBox.Show("This customer has existing transaction history and cannot be deleted.");
                return;
            }


            string query = "DELETE FROM CustomerMasterfile WHERE Customer_ID=@id";
            EnsureConnectionOpen();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();

            MessageBox.Show("Customer Deleted!");
            LoadCustomerList();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnClear_Click_1(object sender, EventArgs e)
        {
            txtboxCustomerName.Clear();
            txtboxAddress.Clear();
            selectedCustomerID = 0;
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void buttonAdd2_Click_1(object sender, EventArgs e)
        {
            string itemCode = txtBoxItemCode.Text.Trim();
            string description = textBoxDescription.Text.Trim();
        
           

            if (string.IsNullOrWhiteSpace(itemCode) &&
                string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill out Item Code, Description, and Category.");
                return;
            }
           
            else if (string.IsNullOrWhiteSpace(itemCode))
            {
                MessageBox.Show("No item Code. Please include the item code for each item");
                return;
            }
            else if (string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please enter the type description");
                return;
            }
            if (CategoryType.SelectedIndex == -1) { MessageBox.Show("Please select a category."); return; } 
            int categoryID = Convert.ToInt32(CategoryType.SelectedValue);

            string query = @"INSERT INTO ItemMasterfile (ItemCode, ItemDescription, CategoryID)
                     VALUES (@code, @desc, @catID)";

            EnsureConnectionOpen();
            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@code", itemCode);
            cmd.Parameters.AddWithValue("@desc", description);
            cmd.Parameters.AddWithValue("@catID", categoryID);

            cmd.ExecuteNonQuery();

            MessageBox.Show("Scrap item added successfully!");

            LoadScrapItems();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void buttonUpdate2_Click_1(object sender, EventArgs e)
        {
            if (selectedItemID == 0)
            {
                MessageBox.Show("Please select a scrap item from the list.");
                return;
            }

            string itemCode = txtBoxItemCode.Text.Trim();
            string description = textBoxDescription.Text.Trim();

            if (string.IsNullOrWhiteSpace(itemCode) || string.IsNullOrWhiteSpace(description))
            {
                MessageBox.Show("Please fill out Item Code and Description.");
                return;
            }

            if (CategoryType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a category.");
                return;
            }

         
            int categoryID = Convert.ToInt32(CategoryType.SelectedValue);

            EnsureConnectionOpen();

            string query = @"UPDATE ItemMasterfile 
                     SET ItemCode=@code, ItemDescription=@desc, CategoryID=@catID
                     WHERE ItemID=@id";

            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@code", itemCode);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@catID", categoryID);
                cmd.Parameters.AddWithValue("@id", selectedItemID);

                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Scrap item updated successfully!");
            LoadScrapItems();
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void buttonDelete2_Click_1(object sender, EventArgs e)
        {
            if (selectedItemID == 0)
            {
                MessageBox.Show("Please select a scrap item from the list.");
                return;
            }

            EnsureConnectionOpen();

            string checkTransactions = "SELECT COUNT(*) FROM TransactionDetails WHERE ItemID=@id";
            using (MySqlCommand chkTrans = new MySqlCommand(checkTransactions, conn))
            {
                chkTrans.Parameters.AddWithValue("@id", selectedItemID);
                int countTrans = Convert.ToInt32(chkTrans.ExecuteScalar());

                if (countTrans > 0)
                {
                    MessageBox.Show("This scrap item is used in transactions and cannot be deleted.");
                    return;
                }
            }


           
            int categoryID = GetCurrentCategoryID(selectedItemID);

    
            string deleteItemQuery = "DELETE FROM ItemMasterfile WHERE ItemID=@id";
            using (MySqlCommand cmdDeleteItem = new MySqlCommand(deleteItemQuery, conn))
            {
                cmdDeleteItem.Parameters.AddWithValue("@id", selectedItemID);
                cmdDeleteItem.ExecuteNonQuery();
            }

       
            string checkCategoryUsage = "SELECT COUNT(*) FROM ItemMasterfile WHERE CategoryID=@catID";
            using (MySqlCommand cmdCheckCat = new MySqlCommand(checkCategoryUsage, conn))
            {
                cmdCheckCat.Parameters.AddWithValue("@catID", categoryID);
                int countCat = Convert.ToInt32(cmdCheckCat.ExecuteScalar());

                if (countCat == 0)
                {
                    string deleteCategoryQuery = "DELETE FROM CategoryMasterfile WHERE CategoryID=@catID";
                    using (MySqlCommand cmdDeleteCat = new MySqlCommand(deleteCategoryQuery, conn))
                    {
                        cmdDeleteCat.Parameters.AddWithValue("@catID", categoryID);
                        cmdDeleteCat.ExecuteNonQuery();
                    }
                }
            }

            MessageBox.Show("Scrap item deleted successfully!");
            LoadScrapItems();

         
            selectedItemID = 0;
            txtBoxItemCode.Clear();
            textBoxDescription.Clear();
            CategoryType.SelectedIndex = -1; 
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void buttonClear2_Click_1(object sender, EventArgs e)
        {
            txtBoxItemCode.Clear();
            textBoxDescription.Clear();
          
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void buttonOK_Click_1(object sender, EventArgs e)
        {
            string boxTypeDesc = "No Box Included";
            decimal boxWeight = 0.00m;

            try
            {
                EnsureConnectionOpen();


                string checkQuery = "SELECT COUNT(*) FROM BoxTypeMasterfile WHERE BoxTypeDescription=@desc";
                MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@desc", boxTypeDesc);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("'No Box Included' already exists in the list.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


                string insertQuery = "INSERT INTO BoxTypeMasterfile (BoxTypeDescription, BoxWeight) VALUES (@desc, @weight)";
                MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                insertCmd.Parameters.AddWithValue("@desc", boxTypeDesc);
                insertCmd.Parameters.AddWithValue("@weight", boxWeight);
                insertCmd.ExecuteNonQuery();

                MessageBox.Show("'No Box Included' has been added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                LoadBoxTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding 'No Box Included': " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnClearBox_Click_1(object sender, EventArgs e)
        {
            txtBoxType.Clear();
            txtBoxWeight.Clear();
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnDeleteBox_Click_1(object sender, EventArgs e)
        {
            if (selectedBoxTypeID == 0)
            {
                MessageBox.Show("Please select a box type from the list.");
                return;
            }

            EnsureConnectionOpen();

            string checkUsageQuery = @"SELECT COUNT(*) FROM TransactionDetails WHERE BoxTypeID=@id";


            MySqlCommand checkCmd = new MySqlCommand(checkUsageQuery, conn);
            checkCmd.Parameters.AddWithValue("@id", selectedBoxTypeID);

            int usageCount = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (usageCount > 0)
            {
                MessageBox.Show("This Box Type is used in transactions and cannot be deleted.");
                return;
            }


            string deleteQuery = "DELETE FROM BoxTypeMasterfile WHERE BoxTypeID=@id";

            try
            {
                MySqlCommand delCmd = new MySqlCommand(deleteQuery, conn);
                delCmd.Parameters.AddWithValue("@id", selectedBoxTypeID);
                delCmd.ExecuteNonQuery();

                MessageBox.Show("Box type deleted successfully!");

                LoadBoxTypes();

                selectedBoxTypeID = 0;
                txtBoxType.Clear();
                txtBoxWeight.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting box type: " + ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnUpdateBox_Click_1(object sender, EventArgs e)
        {
            if (selectedBoxTypeID == 0)
            {
                MessageBox.Show("Please select a box type from the list.");
                return;
            }

            string boxTypeDesc = txtBoxType.Text.Trim();
            string boxWeightText = txtBoxWeight.Text.Trim();

            if (string.IsNullOrWhiteSpace(boxTypeDesc))
            {
                MessageBox.Show("Please enter a Box Type description.");
                return;
            }

            if (!decimal.TryParse(boxWeightText, out decimal boxWeight))
            {
                MessageBox.Show("Box Weight must be a valid number (e.g., 10.00).");
                return;
            }

            string query = @"UPDATE BoxTypeMasterfile 
                     SET BoxTypeDescription=@desc, BoxWeight=@weight
                     WHERE BoxTypeID=@id";

            try
            {
                EnsureConnectionOpen();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", boxTypeDesc);
                    cmd.Parameters.AddWithValue("@weight", boxWeight);
                    cmd.Parameters.AddWithValue("@id", selectedBoxTypeID);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Box type updated successfully!");
                LoadBoxTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating box type: " + ex.Message);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void btnAddBox_Click_1(object sender, EventArgs e)
        {
            string boxTypeDesc = txtBoxType.Text.Trim();
            string boxWeightText = txtBoxWeight.Text.Trim();


            if (string.IsNullOrEmpty(boxTypeDesc))
            {
                MessageBox.Show("Please enter a Box Type description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(boxWeightText, out decimal boxWeight))
            {
                MessageBox.Show("Box Weight must be a valid number (e.g., 10.00).", "Invalid Weight", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }



            string query = @"INSERT INTO BoxTypeMasterfile (BoxTypeDescription, BoxWeight)
                     VALUES (@desc, @weight)";

            try
            {
                EnsureConnectionOpen();

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@desc", boxTypeDesc);
                    cmd.Parameters.AddWithValue("@weight", boxWeight);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Box type added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);


                LoadBoxTypes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding box type: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ReturnToMain_Click(object sender, EventArgs e)
        {
            Main mainForm = this.FindForm() as Main;

            if (mainForm != null)
            {
                mainForm.HomePanel.Controls.Clear();
                mainForm.HomePanel.Visible = false;
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void dataGridViewCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        private void txtboxCustomerName_TextChanged(object sender, EventArgs e){ }
        private void txtboxAddress_TextChanged(object sender, EventArgs e){ }
        private void radEmployee_CheckedChanged(object sender, EventArgs e){ }
        private void radCompany_CheckedChanged(object sender, EventArgs e){ }
        private void dataGridViewScrap_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        private void dataGridBoxWeight_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        private void txtBoxType_TextChanged(object sender, EventArgs e){ }
        private void txtBoxWeight_TextChanged(object sender, EventArgs e){ }
        private void textBoxDescription_TextChanged(object sender, EventArgs e){ }
        private void txtBoxItemCode_TextChanged(object sender, EventArgs e){ }
        private void tabPage1_Click(object sender, EventArgs e){ }
       



    }
}
