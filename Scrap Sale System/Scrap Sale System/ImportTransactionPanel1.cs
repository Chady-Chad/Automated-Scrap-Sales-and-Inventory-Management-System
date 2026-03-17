using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrap_Sale_System
{
    public partial class ImportTransactionPanel1 : UserControl
    {
        private static readonly byte[] AES_KEY = Encoding.UTF8.GetBytes("A1B2C3D4E5F6G7H8"); // SAME KEY
        private static readonly byte[] AES_IV = Encoding.UTF8.GetBytes("H8G7F6E5D4C3B2A1"); // SAME IV
        public ImportTransactionPanel1()
        {
            InitializeComponent();
        }
        private string DecryptAES(byte[] cipherData)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = AES_KEY;
                aes.IV = AES_IV;

                using (var ms = new MemoryStream(cipherData))
                using (var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var reader = new StreamReader(cryptoStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public void LoadTransactionData()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Encrypted Scrap File|*.scrapenc";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                // Read encrypted file
                byte[] encryptedBytes = File.ReadAllBytes(ofd.FileName);

                // Decrypt back to SQL text
                string sqlText = DecryptAES(encryptedBytes);


                // Parse SQL INSERT statements 
                string[] lines = sqlText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

                foreach (string line in lines)
                {
                    if (!line.StartsWith("INSERT INTO")) continue;

                    int startTable = line.IndexOf("`") + 1;
                    int endTable = line.IndexOf("`", startTable);
                    string tableName = line.Substring(startTable, endTable - startTable);

                    int startCols = line.IndexOf("(") + 1;
                    int endCols = line.IndexOf(")", startCols);
                    string colString = line.Substring(startCols, endCols - startCols);
                    string[] columns = colString.Replace("`", "").Split(',');

                    if (!tables.ContainsKey(tableName))
                    {

                        DataTable dt = new DataTable(tableName);
                        foreach (var col in columns)
                            dt.Columns.Add(col.Trim());
                        tables[tableName] = dt;
                    }

                    int startVals = line.IndexOf("VALUES (") + 7;
                    int endVals = line.LastIndexOf(")");
                    string valString = line.Substring(startVals, endVals - startVals);

                    string[] values = valString.Split(',')
                                               .Select(v => v.Trim().Trim('\''))
                                               .ToArray();

                    tables[tableName].Rows.Add(values);
                }


                tabControl1.TabPages.Clear();
                foreach (var kvp in tables)
                {
                    TabPage tab = new TabPage(kvp.Key);
                    DataGridView dgv = new DataGridView
                    {
                        Dock = DockStyle.Fill,
                        DataSource = kvp.Value,
                        ReadOnly = true,
                        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
                    };
                    tab.Controls.Add(dgv);
                    tabControl1.TabPages.Add(tab);
                }

                MessageBox.Show("Encrypted file decrypted and loaded successfully.",
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Invalid or corrupted file.\nDecryption failed.",
                                "Security Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //==========================================================================================================================================================================
            private void ReturnToMain_Click(object sender, EventArgs e)
            {
                Main mainForm = this.FindForm() as Main;

                if (mainForm != null)
                {
                    mainForm.HomePanel.Controls.Clear();
                    mainForm.HomePanel.Visible = false;
                }
            }
        
        //==========================================================================================================================================================================
        //Keep these voided for future function
        private void Con_Click(object sender, EventArgs e){ }
        private void ImportTransactionPanel1_Load(object sender, EventArgs e){ }

        private void SyncMainServer_Click(object sender, EventArgs e)
        {

        }
    }
}
