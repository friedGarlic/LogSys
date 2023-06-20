using LogAppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogAppForms
{
    public partial class UserForm : Form
    {

        private bool drag = false;
        private Point start_point = new Point(0, 0);
        EntryForm entryForm = new EntryForm();

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
                    UserModel m1 = new UserModel(studentID_value.textBox1.Text);
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
                    entryForm.Visible = true;
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
                        studentID_value.textBox1.Text,
                        age_value.textBox1.Text,
                        contactInfo_value.textBox1.Text,
                        firstName_value.textBox1.Text,
                        lastName_value.textBox1.Text
                    );

            GlobalConfig.DataConnections.CreateUser(user);
            MessageBox.Show("Registered Successfuly");
            this.Close();

        }
        public bool validateForm()
        {
            if (studentID_value.textBox1.TextLength == 0)
            {
                return false;
            }
            if (age_value.textBox1.TextLength == 0 || age_value.textBox1.TextLength > 3)
            {
                return false;
            }
            if (firstName_value.textBox1.TextLength == 0)
            {
                return false;
            }
            if (lastName_value.textBox1.TextLength == 0)
            {
                return false;
            }
            if (contactInfo_value.textBox1.TextLength == 0)
            {
                return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            entryForm.Visible = true;
            this.Close();
        }

        private void UserForm_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void UserForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y); ;
            }
        }

        private void UserForm_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }
    }
}
