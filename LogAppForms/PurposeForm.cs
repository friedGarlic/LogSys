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
        private string Borrow = "Borrow";
        private string Return = "Return";
        private EntryForm entryForm;

        public PurposeForm(EntryForm entryForm)
        {
            InitializeComponent();
            Size = new Size(230, 420);
            this.entryForm = entryForm;
        }
        public PurposeForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateForm()) 
            {
                if (radioButton2.Checked == true)
                {
                    string val = "";
                    decimal q = 0;
                    string cmb = "";
                    if (userControl21.radioButton1.Checked == true)
                    {
                        val = Borrow;
                        q = userControl21.numericUpDown1.Value;
                        cmb = userControl21.comboBox1.Text;
                    }
                    if (userControl21.radioButton2.Checked == true)
                    {
                        val = Return;
                        q = userControl21.numericUpDown1.Value;
                        cmb = userControl21.comboBox1.Text;
                    }
                    PurposeModel model = new PurposeModel(q, val, cmb);
                    UserModel u_model = new UserModel(entryForm.entryIDValue.Text);
                    GlobalConfig.DataConnections.CreatePurpose(u_model, model);
                    MessageBox.Show("Success, Thank you for Borrowing/Returning the Item!");
                    Close();

                }
                if (radioButton1.Checked == true)
                {
                    string val = "";
                    if (userControl11.radioButton1.Checked == true)
                    {
                        val = timeIn;
                    }
                    if (userControl11.radioButton2.Checked == true)
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
            else
            {
                MessageBox.Show("Fill up properly");
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
                Size = new Size(625, 420);
                userControl11.Hide();
                userControl21.Show();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Size = new Size(462, 420);
                userControl11.Show();
                userControl21.Hide();
            }
            if (radioButton2.Checked == true)
            {
                Size = new Size(625, 420);
                userControl11.Hide();
                userControl21.Show();
            }
        }
        public bool ValidateForm()
        {
            if(userControl21.numericUpDown1.Value == 0)
            {
                return false;
            }
            if (userControl21.comboBox1.Text.Length == 0)
            {
                return false;
            }
            if(!userControl21.radioButton1.Checked)
            {
                return false;
            }
            if(!userControl21.radioButton2.Checked)
            {
                return false;
            }
            return true;
        }
    }
}
