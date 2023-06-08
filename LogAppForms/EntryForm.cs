using LogAppLibrary;
using System;
using System.Windows.Forms;

namespace LogAppForms
{
    public partial class EntryForm : Form
    {
        public EntryForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (IsValidForm() == true)
            {
                UserModel model = new UserModel(entryIDValue.Text);
                if (IsValidForm())
                {
                    GlobalConfig.DataConnections.CurrentTime(model);

                }
                PurposeForm purposeForm = new PurposeForm(this);
                purposeForm.Show();
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
    }
}
