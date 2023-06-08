using LogAppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogAppForms
{
    public partial class AdminLogIn : Form
    {
        public AdminLogIn()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
        }

        private SqlConnection _conn = new SqlConnection(GlobalConfig.ConnectString("SearchCN"));

        private void button1_Click(object sender, EventArgs e)
        {
            string username, password;

            username = textBox1.Text;
            password = textBox2.Text;

            try
            {
                string querry = "SELECT * FROM AdminAcc Where Username='" + textBox1.Text + "' AND password='" + textBox2.Text + "' ";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(querry, _conn);

                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    username = textBox1.Text;
                    password = textBox2.Text;

                    AdminForm adminForm = new AdminForm();
                    adminForm.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Login Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textBox1.Clear();
                    textBox2.Clear();

                    textBox1.Focus();
                }
            }
            catch
            {
                MessageBox.Show("error");
            }
            finally
            {
                _conn.Close();
            }
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e);
            }
        }
    }
}
