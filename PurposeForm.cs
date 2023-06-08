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
    public partial class PurposeForm : Form
    {
        private string timeIn = "Time In";
        private string timeOut = "Time Out";
        private EntryForm entryForm;

        public PurposeForm(EntryForm entryForm)
        {
            InitializeComponent();
            Size = new Size(257, 420);
            this.entryForm = entryForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(radioButton2.Checked == true)
            {
                //Equipment equipment = new Equipment();
                //equipment.Show();
                MessageBox.Show("Not Available Right Now!");
            }
            if(radioButton1.Checked == true)
            {
                string val = "";
                if(userControl11.radioButton1.Checked == true)
                {
                    val = timeIn;
                }
                if(userControl11.radioButton2.Checked == true)
                {
                    val = timeOut;
                }
                PurposeModel model = new PurposeModel(val);
                UserModel u_model = new UserModel(entryForm.entryIDValue.Text);

                GlobalConfig.DataConnections.CurrentTime(u_model, model);
                MessageBox.Show("Success, Mind the other students!");
                
                Close();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Size = new Size(462, 420);
                userControl11.Show();
                userControl21.Hide();
            }
            if (radioButton2.Checked == true)
            {
                Size = new Size(800, 420);
                userControl11.Hide();
                userControl21.Show();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Size = new Size(205, 357);
                userControl11.Show();
                userControl21.Hide();
            }
            if (radioButton2.Checked == true)
            {
                Size = new Size(800, 420);
                userControl11.Hide();
                userControl21.Show();
            }
        }

    }
}
