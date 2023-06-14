using LogAppLibrary;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace LogAppForms
{
    public partial class EntryForm : Form
    {
        bool drag = false;
        Point start_point = new Point(0, 0);
        
        public EntryForm()
        {
            InitializeComponent();
        }

        private void LogIn()
        {
            UserModel model = new UserModel(entryIDValue.Text);

            GlobalConfig.DataConnections.CurrentTime(model);
            PurposeForm purposeForm = new PurposeForm(this);
            purposeForm.Show();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValidForm() == true)
            {
                if (IsValidForm())
                {
                    UserModel m1 = new UserModel(entryIDValue.Text);
                    if (GlobalConfig.DataConnections.IsStudentIdDuplicate(m1))
                    {
                        LogIn();
                    }
                    else
                    {
                        MessageBox.Show("Your ID is not registered, Ask the Admin for more information", "Get an Administrator for more information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }
            }
            else
            {
                MessageBox.Show("INVALID INPUT VALUE");
            }
        }

        private void entryIDValue_TextChanged(object sender, EventArgs e)
        {
            
        }

        private bool IsValidForm()
        {
            bool isValid = true;
            if(entryIDValue.Text.Length == 0)
            {
                isValid = false;
            }

            return isValid;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdminLogIn adminLogIn = new AdminLogIn();
            adminLogIn.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UserForm userForm = new UserForm();
            userForm.Show();
        }

        private void entryIDValue_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e);
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y); ;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
