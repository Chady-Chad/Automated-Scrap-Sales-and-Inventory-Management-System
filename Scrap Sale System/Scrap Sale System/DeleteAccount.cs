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
using System.Configuration;
namespace Scrap_Sale_System
{
    public partial class DeleteAccount : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ServerPathDbConnection"].ConnectionString;
       // string connectionString = "server=10.0.253.60;user=root;password=Windows7;database=Scraps";
        // string connectionString = "server=localhost;user=root;password=Windows7;database=Scraps";
       // string connectionString = "server=localhost;user=root;password=masterx;database=Scraps";
        public DeleteAccount()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            AccountRecorded.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            password.UseSystemPasswordChar = true;
            confirmPassword.UseSystemPasswordChar = true;

        }
        //===========================================================================================================================================================================\
        private void hidePass1_MouseUp(object sender, MouseEventArgs e)
        {
            password.UseSystemPasswordChar = true;
        }

        private void hidePass1_MouseDown(object sender, MouseEventArgs e)
        {
            password.UseSystemPasswordChar = false;
        }

        private void conPass_MouseUp(object sender, MouseEventArgs e)
        {
            confirmPassword.UseSystemPasswordChar = true;

        }

        private void conPass_MouseDown(object sender, MouseEventArgs e)
        {
            confirmPassword.UseSystemPasswordChar = false;

        }
        //===========================================================================================================================================================================\
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            string user = AccountConfirm.Text.Trim();
            string pass = password.Text.Trim();
            string conpass = confirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(conpass))
            {
                MessageBox.Show("Please fill in all fields before deleting.");
                return;
            }

            if (pass != conpass)
            {
                MessageBox.Show("Passwords do not match!");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string getIdQuery = @"SELECT AdminStaff_ID 
                              FROM AdminStaffMasterfile 
                              WHERE AdminStaffName = @user AND Password = @pass";

                int adminId = -1;
                using (MySqlCommand cmdGetId = new MySqlCommand(getIdQuery, connection))
                {
                    cmdGetId.Parameters.AddWithValue("@user", user);
                    cmdGetId.Parameters.AddWithValue("@pass", pass);

                    object result = cmdGetId.ExecuteScalar();
                    if (result != null)
                        adminId = Convert.ToInt32(result);
                }

                if (adminId == -1)
                {
                    MessageBox.Show("No matching account found.");
                    return;
                }

                string checkQuery = @"SELECT COUNT(*) 
                              FROM TransactionMasterfile 
                              WHERE AdminStaffID = @adminId
                              UNION ALL
                              SELECT COUNT(*) 
                              FROM CancelledTransaction 
                              WHERE CancelledByID = @adminId";

                int referenceCount = 0;
                using (MySqlCommand cmdCheck = new MySqlCommand(checkQuery, connection))
                {
                    cmdCheck.Parameters.AddWithValue("@adminId", adminId);
                    using (MySqlDataReader reader = cmdCheck.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            referenceCount += reader.GetInt32(0);
                        }
                    }
                }

                if (referenceCount > 0)
                {
                    MessageBox.Show("This account has existing transaction history and cannot be deleted.");
                    return;
                }

            
                string deleteQuery = @"DELETE FROM AdminStaffMasterfile 
                               WHERE AdminStaff_ID = @adminId";

                using (MySqlCommand cmdDelete = new MySqlCommand(deleteQuery, connection))
                {
                    cmdDelete.Parameters.AddWithValue("@adminId", adminId);
                    int rowsAffected = cmdDelete.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Account deleted successfully.");
                        LoadAccounts();
                        password.Clear();
                        confirmPassword.Clear();
                        AccountConfirm.Clear();
                    }
                    else
                    {
                        MessageBox.Show("No matching account found.");
                    }
                }
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        private void LoadAccounts()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT FirstName, Surname, IDNumber FROM InChargePersonnelDetails";
              
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                AccountRecorded.DataSource = dt;
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        private void DeleteAccount_Load(object sender, EventArgs e){

            LoadAccounts();
        
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        private void SearchBar_TextChanged(object sender, EventArgs e) {
            string search = SearchBar.Text.Trim();
            SearchAccounts(search);
        }
        private void SearchAccounts(string search)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT FirstName, Surname, IDNumber FROM InChargePersonnelDetails WHERE FirstName LIKE @search OR Surname LIKE @search OR IDNumber LIKE @search";
                //string query = @" SELECT a.AdminStaffName, CONCAT(p.FirstName, ' ', p.Surname) 
                //AS FullName, p.IDNumber, a.AccessTypeID FROM AdminStaffMasterfile a LEFT JOIN InChargePersonnelDetails p 
                //ON a.AdminStaff_ID = p.AdminStaffID WHERE a.AdminStaffName LIKE @search OR p.FirstName 
                //LIKE @search OR p.Surname LIKE @search OR p.IDNumber LIKE @search";

                MySqlDataAdapter adp = new MySqlDataAdapter(query, connection);
                adp.SelectCommand.Parameters.AddWithValue("@search", "%" +search + "%");
                DataTable dataSearch = new DataTable();
                adp.Fill(dataSearch);
                AccountRecorded.DataSource = dataSearch;
            }

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------
        private void AccountConfirm_TextChanged(object sender, EventArgs e){ }
        private void password_TextChanged(object sender, EventArgs e){ }
        private void AccountRecorded_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        private void confirmPassword_TextChanged(object sender, EventArgs e){ }

    
    }
}
