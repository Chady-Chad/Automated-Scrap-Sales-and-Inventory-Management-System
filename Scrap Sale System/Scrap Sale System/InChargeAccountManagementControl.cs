using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Tls;
using System.Configuration;
namespace Scrap_Sale_System
{

    public partial class InChargeAccountManagementControl : UserControl
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ServerPathDbConnection"].ConnectionString;
     //   string connectionString = "server=10.0.253.60;user=root;password=Windows7;database=Scraps";
        // string connectionString = "server=localhost;user=root;password=Windows7;database=Scraps";
       // string connectionString = "server=localhost;user=root;password=masterx;database=Scraps";
        public InChargeAccountManagementControl()
        {
            InitializeComponent();
            
          
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPass.UseSystemPasswordChar = true;
            SectionBox.DropDownStyle = ComboBoxStyle.DropDownList;
            dataGridInchargeAccountViewer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
        }
        private void InChargeAccountManagementControl_Load(object sender, EventArgs e)
        {
            LoadAccessTypes();
            LoadAccounts();

        }
        private void RefreshButton_Click(object sender, EventArgs e) {
            LoadAccessTypes();
            LoadAccounts();
        }
        //===============================================================================================================================================================
        private void hidePass1_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        private void hidePass1_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
        }

        private void hideConfirmPass1_MouseDown(object sender, MouseEventArgs e)
        {
            txtConfirmPass.UseSystemPasswordChar = false;
        }

        private void hideConfirmPass1_MouseUp(object sender, MouseEventArgs e)
        {
            txtConfirmPass.UseSystemPasswordChar = true;
        }

        //===============================================================================================================================================================
        private void LoadAccessTypes()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT AccessType_ID, AccessTypeName FROM AccessTypeMasterfile";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                SectionBox.DataSource = dt;
                SectionBox.DisplayMember = "AccessTypeName";  
                SectionBox.ValueMember = "AccessType_ID";    
            }
        }

        //===============================================================================================================================================================
        private void LoadAccounts()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT FirstName, Surname, IDNumber, EmployeeSection FROM InChargePersonnelDetails";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dataGridInchargeAccountViewer.DataSource = dt;
            }

        }
        //===============================================================================================================================================================
        private void hidePass1_Click(object sender, EventArgs e)
        {
            if (txtPassword.UseSystemPasswordChar)
            {
                txtPassword.UseSystemPasswordChar = false;
                hidePass1.Enabled = false;
            }
        }

        private void hideConfirmPass1_Click(object sender, EventArgs e)
        {
            if (txtConfirmPass.UseSystemPasswordChar)
            {
                txtConfirmPass.UseSystemPasswordChar = false;
                hideConfirmPass1.Enabled = false;
            }
        }
        //===============================================================================================================================================================
        private void SaveAccount_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string Sur = txtSurname.Text.Trim();
            string username = txtUsername.Text.Trim();
            string ID = txtID.Text.Trim();
            string accessTypeName = SectionBox.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPass = txtConfirmPass.Text.Trim();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(Sur) ||
                string.IsNullOrEmpty(username) || string.IsNullOrEmpty(ID) ||
                string.IsNullOrEmpty(accessTypeName) ||
                string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPass))
            {
                MessageBox.Show("Textboxes cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPass)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlTransaction transaction = connection.BeginTransaction();

                try
                {

                    using (MySqlCommand checkCmd = new MySqlCommand(
                        @"SELECT COUNT(*) 
                  FROM AdminStaffMasterfile a
                  INNER JOIN InChargePersonnelDetails  e ON a.AdminStaff_ID = e.AdminStaffID
                  WHERE a.AdminStaffName=@username 
                     OR e.IDNumber=@idNumber 
                     OR (e.FirstName=@firstName AND e.Surname=@surname)",
                        connection, transaction))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        checkCmd.Parameters.AddWithValue("@idNumber", ID);
                        checkCmd.Parameters.AddWithValue("@firstName", name);
                        checkCmd.Parameters.AddWithValue("@surname", Sur);

                        int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (exists > 0)
                        {
                            MessageBox.Show("This account already exists. Please enter different account.",
                                            "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            transaction.Rollback();
                            return;
                        }
                    }


                    int accessTypeID = Convert.ToInt32(SectionBox.SelectedValue);


                    int adminStaffID;
                    using (MySqlCommand cmd = new MySqlCommand(
                        "INSERT INTO AdminStaffMasterfile (AdminStaffName, AccessTypeID, Password) VALUES (@username, @accessTypeID, @password); SELECT LAST_INSERT_ID();",
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@accessTypeID", accessTypeID);
                        cmd.Parameters.AddWithValue("@password", password);

                        adminStaffID = Convert.ToInt32(cmd.ExecuteScalar());
                    }


                    using (MySqlCommand cmd = new MySqlCommand(
                        "INSERT INTO InChargePersonnelDetails (AdminStaffID, FirstName, Surname, IDNumber, EmployeeSection) VALUES (@adminStaffID, @firstName, @surname, @idNumber, @section)",
                        connection, transaction))
                    {
                        cmd.Parameters.AddWithValue("@adminStaffID", adminStaffID);
                        cmd.Parameters.AddWithValue("@firstName", name);
                        cmd.Parameters.AddWithValue("@surname", Sur);
                        cmd.Parameters.AddWithValue("@idNumber", ID);
                        cmd.Parameters.AddWithValue("@section", accessTypeName);

                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Account saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Clear();
                    txtSurname.Clear();
                    txtUsername.Clear();
                    txtID.Clear();
                    txtPassword.Clear();
                    txtConfirmPass.Clear();
                    LoadAccounts();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Error saving account: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        //===============================================================================================================================================================
        private void DiscardChanges_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Leaving without saving will discard the changes you made.", "Discard changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                Main mainForm = this.FindForm() as Main;

                if (mainForm != null)
                {
                    mainForm.HomePanel.Controls.Clear();
                    mainForm.HomePanel.Visible = false;
                }
            }

        }
        //===============================================================================================================================================================
        private void DeleteAccountButton_Click(object sender, EventArgs e)
        {
            DeleteAccount delAccount = new DeleteAccount();
            delAccount.ShowDialog();
        }
        //===============================================================================================================================================================
        private void txtName_TextChanged(object sender, EventArgs e){ }
        private void txtSurname_TextChanged(object sender, EventArgs e){ }
        private void txtUsername_TextChanged(object sender, EventArgs e){ }
        private void txtID_TextChanged(object sender, EventArgs e){ }
        private void SectionBox_SelectedIndexChanged(object sender, EventArgs e){ }
        private void txtPassword_TextChanged(object sender, EventArgs e){ }
        private void txtConfirmPass_TextChanged(object sender, EventArgs e){ }
        private void dataGridInchargeAccountViewer_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        //===============================================================================================================================================================
        //Isolate these voides for future functions

    }
}
