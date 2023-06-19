using LogAppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LogAppForms
{
    public partial class PurposeForm : Form
    {
        private SqlConnection _conn = new SqlConnection(GlobalConfig.ConnectString("SearchCN"));
        private SqlCommand cmd;
        private DataTable dt;
        private SqlDataAdapter adapter;
        private DataSet ds;
        private DataView dataView;

        private string timeIn = "Time In";
        private string timeOut = "Time Out";
        private string Borrow = "Borrow";
        private string Return = "Return";
        private EntryForm entryForm;
        private bool toggleSwitchState = false;

        public PurposeForm(EntryForm entryForm)
        {
            InitializeComponent();
            Size = new Size(207, 445);
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
                    if (toggle_Switch2.Checked == true)
                    {
                        string val = "";
                        decimal q = 0;
                        string cmb = "";
                        if (userControl21.radioButton1.Checked == true)
                        {
                            val = Borrow;
                            q = userControl21.numericUpDown1.Value;
                            cmb = userControl21.comboBox1.Text;
                            if (IsItemAvailable(cmb))
                            {
                                PurposeModel model = new PurposeModel(q, val, cmb);
                                UserModel u_model = new UserModel(entryForm.entryIDValue.Text);

                                RemoveQuantity();
                                GlobalConfig.DataConnections.CreatePurpose(u_model, model);
                                MessageBox.Show("Success, Please return the borrowed Item!");
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("Item is not available!!");
                            }
                        }

                        if (userControl21.radioButton2.Checked == true)
                        {
                            if (!CheckPreviousPurpose(entryForm.entryIDValue.Text)) // if theres a borrow string in purpose, then you can return otherwise, stop user
                            {
                                val = Return;
                                q = userControl21.numericUpDown1.Value;
                                cmb = userControl21.comboBox1.Text;

                                PurposeModel model = new PurposeModel(q, val, cmb);
                                UserModel u_model = new UserModel(entryForm.entryIDValue.Text);

                                AddQuantity();
                                GlobalConfig.DataConnections.CreatePurpose(u_model, model);
                                MessageBox.Show("Success, Thank you for Returning the Item!");
                                Close();
                            }
                            else
                            {
                                MessageBox.Show("You Didnt Borrow any items!!, Please check the administrator");
                            }
                        }
                    }
                    if (toggle_Switch1.Checked == true)
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

        public bool ValidateForm()
        {
            if(toggle_Switch1.Checked == true && toggle_Switch2.Checked == true)
            {
                MessageBox.Show("You're already Timed In and trying to borrow/return Items, Time Out first!!");
                return false;
            }
            if (userControl21.numericUpDown1.Value <= 0 || userControl21.comboBox1.Text.Length <= 0)
            {
                if (toggleSwitchState == false)
                {
                    return false;
                }
                if (toggleSwitchState == true)
                {
                    return true;
                }
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
            //return flag as default cause of auto change
            toggleSwitchState = false;
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
                            //for togglestate
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

        private void toggle_Switch1_MouseClick(object sender, MouseEventArgs e)
        {
            toggleSwitchState = true;
        }

        private void toggle_Switch2_CheckedChanged(object sender, EventArgs e)
        {
            if (toggle_Switch2.Checked == true)
            {
                Size = new Size(566, 445);
                userControl21.Show();
            }
            if (toggle_Switch2.Checked == false)
            {
                Size = new Size(207, 445);
                userControl21.Show();
            }
        }

        private void RemoveQuantity()
        {
            ItemModel item = new ItemModel(
                            userControl21.comboBox1.Text,
                            userControl21.numericUpDown1.Value
                        );

            GlobalConfig.DataConnections.RemoveItem(item);
        }
        private void AddQuantity()
        {
            ItemModel item = new ItemModel(
                            userControl21.comboBox1.Text,
                            userControl21.numericUpDown1.Value
                        );

            GlobalConfig.DataConnections.AddQuantityItem(item);
        }
        private bool CheckPreviousPurpose(string studentID)
        {
            string previousTimeInOut = null;
            string previousStudentIdNumber = null; 
            bool foundString= false;
            DataTable dataTable = GetDateTimeTable();
            dataTable.DefaultView.Sort = "TimeInOut DESC";

            foreach (DataRowView rowView in dataTable.DefaultView)
            {
                DataRow row = rowView.Row;

                string currentTimeInOut = row["TimeInOut"].ToString();
                string currentStudentIdNumber = row["StudentIdNumber"].ToString();

                if (currentStudentIdNumber == studentID && previousTimeInOut == "Return")
                {
                    // Do something if the previous "TimeInOut" was "Borrow" for the same "StudentIdNumber"
                    // For example: 
                    foundString = true; // Set the flag to true
                    break;
                }

                previousTimeInOut = currentTimeInOut;
                previousStudentIdNumber = currentStudentIdNumber;
            }
            return foundString;
        }
        private DataTable GetDateTimeTable() //On table DateTimeTable
        {
            _conn.Open();
            cmd = new SqlCommand("SELECT * FROM DateTimeTable", _conn);

            adapter = new SqlDataAdapter(cmd);

            ds = new DataSet();

            adapter.Fill(ds, "dbo.DateTimeTable");

            _conn.Close();

            dt = ds.Tables["dbo.DateTimeTable"];

            return dt;
        }
        private DataTable GetItemsTable() // returns Items table
        {
            _conn.Open();
            cmd = new SqlCommand("SELECT * FROM Items", _conn);

            adapter = new SqlDataAdapter(cmd);

            ds = new DataSet();

            adapter.Fill(ds, "dbo.Items");

            _conn.Close();

            dt = ds.Tables["dbo.Items"];

            return dt;
        }
        public bool IsItemAvailable(string itemName)
        {
            bool avail = false;
            foreach (DataRow row in GetItemsTable().Rows)
            {
                string tableItemName = row["ItemName"].ToString();
                int quantity = Convert.ToInt32(row["Quantity"]);

                if (quantity > 0 && tableItemName == itemName)
                {
                    avail = true;
                }
            }
            return avail;
        }
    }
}
