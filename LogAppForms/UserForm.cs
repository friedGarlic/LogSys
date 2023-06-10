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
    public partial class UserForm : Form
    {

        public UserForm()
        {
            InitializeComponent();
        }

        private void register_button_Click(object sender, EventArgs e)
        {
            if(validateForm())
            {
                AdminPass adminPass = new AdminPass();

                if (adminPass.ShowDialog() == DialogResult.OK)
                {
                    UserModel m1 = new UserModel(studentID_value.Text);
                    if (!GlobalConfig.DataConnections.IsStudentIdDuplicate(m1))
                    {
                        CreateUser();
                    }
                    else
                    {
                        MessageBox.Show("Your ID is already registered, Ask the Admin for more information","Get an Administrator for more information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Fill In The Forms Properly!");
            }
        }
        public void CreateUser()
        {
            UserModel user = new UserModel(
                        studentID_value.Text,
                        age_value.Text,
                        contactInfo_value.Text,
                        firstName_value.Text,
                        lastName_value.Text
                    );

            GlobalConfig.DataConnections.CreateUser(user);
            MessageBox.Show("Registered Successfuly");

        }
        private bool IsStudentIdDuplicate(UserModel user)
        {
            user = new UserModel(studentID_value.Text);
            return GlobalConfig.DataConnections.IsStudentIdDuplicate(user);
        }
        public bool validateForm()
        {
            if (studentID_value.TextLength == 0)
            {
                return false;
            }
            if (age_value.TextLength == 0 || age_value.TextLength > 3)
            {
                return false;
            }
            if (firstName_value.TextLength == 0)
            {
                return false;
            }
            if (lastName_value.TextLength == 0)
            {
                return false;
            }
            if (contactInfo_value.TextLength == 0)
            {
                return false;
            }

            return true;
        }
    }
}
