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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LogAppForms
{
    public partial class AdminPass : Form
    {
        private UserForm userForm;
        public AdminPass(UserForm userForm)
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            this.userForm = userForm;
        }

        private SqlConnection _conn = new SqlConnection(GlobalConfig.ConnectString("SearchCN"));

        private void button1_Click(object sender, EventArgs e)
        {
            string password;

            password = textBox2.Text;

            try
            {
                string querry = "SELECT * FROM AdminAcc Where password='"+textBox2.Text+"' ";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(querry, _conn);

                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    password = textBox2.Text;

                    UserModel user = new UserModel(
                        userForm.studentID_value.Text,
                        userForm.age_value.Text,
                        userForm.contactInfo_value.Text,
                        userForm.firstName_value.Text,
                        userForm.lastName_value.Text
                    );

                    GlobalConfig.DataConnections.CreateUser(user);
                    MessageBox.Show("Registered Successfuly");

                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid Login Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textBox2.Clear();

                    textBox2.Focus();
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
