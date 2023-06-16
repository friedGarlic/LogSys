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
                if (toggle_Switch1.Checked || !toggle_Switch1.Checked)
                {
                    string val = "";
                    if (toggle_Switch1.Checked == true)
                    {
                        val = timeIn;
                        label2.Text = val;
                    }
                    if (toggle_Switch1.Checked == false)
                    {
                        val = timeOut;
                        label2.Text = val;
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

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                Size = new Size(625, 420);
                userControl21.Show();
            }
        }
        public bool ValidateForm()
        {
            if (toggle_Switch1.Checked == true || toggle_Switch1.Checked == false)
            {
                return true;
            }
            if (userControl21.numericUpDown1.Value <= 0 || userControl21.comboBox1.Text.Length <= 0)
            {
                return false;
            }
            if (userControl21.radioButton1.Checked || userControl21.radioButton2.Checked)
            {
                return true;
            }
            return true;
        }

        private void toggle_Switch1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void InitializeToggleSwitch(string studentID)
        {
            using (SqlConnection connection = new SqlConnection(GlobalConfig.ConnectString("SearchCN")))
            {
                connection.Open();

                string sqlQuery = "SELECT TOP 1 TimeInOut FROM DateTimeTable WHERE StudentIdNumber = @StudentIdNumber ORDER BY CurDateTime DESC";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentIdNumber", studentID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string lastTimeInOut = reader.GetString(0);
                            toggle_Switch1.Checked = (lastTimeInOut == "Time In");
                            label2.Text = lastTimeInOut;
                        }
                        else
                        {
                            // No TimeIn/TimeOut records found, default state
                            toggle_Switch1.Checked = false;
                        }
                    }
                }

                connection.Close();
            }
        }

        private void PurposeForm_Load(object sender, EventArgs e)
        {
            InitializeToggleSwitch(entryForm.entryIDValue.Text);
        }
    }
}
