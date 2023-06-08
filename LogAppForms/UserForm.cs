using LogAppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
                AdminPass adminPass = new AdminPass(this);
                adminPass.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Fill In The Forms Properly!");
            }
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
