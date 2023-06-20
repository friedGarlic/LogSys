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
using System.Threading;
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

        private string timeIn = "Time In";
        private string timeOut = "Time Out";
        private string Borrow = "Borrow";
        private string Return = "Return";
        private EntryForm entryForm;
        private bool toggleSwitchState = false;
        string val = "";
        decimal q = 0;
        string cmb = "";

        public PurposeForm(EntryForm entryForm)
        {
            InitializeComponent();
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
                if (userControl21.radioButton1.Checked == true)
                {
                    val = Borrow;
                    q = userControl21.numericUpDown1.Value;
                    cmb = userControl21.comboBox1.Text;
                    if (IsItemAvailable(cmb)) //if itemname quantity is > 0
                    {
                        PurposeModel model = new PurposeModel(q, val, cmb);
                        UserModel u_model = new UserModel(entryForm.entryIDValue.Text);

                        RemoveQuantity();
                        GlobalConfig.DataConnections.CreatePurpose(u_model, model);
                        GlobalConfig.DataConnections.AddUnreturnedItem(cmb,q);
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
                        val = Return;
                        q = userControl21.numericUpDown1.Value;
                        cmb = userControl21.comboBox1.Text;

                        PurposeModel model = new PurposeModel(q, val, cmb);
                        UserModel u_model = new UserModel(entryForm.entryIDValue.textBox1.Text);

                        AddQuantity();
                        GlobalConfig.DataConnections.CreatePurpose(u_model, model);
                        GlobalConfig.DataConnections.SubUnreturnedItem(cmb,q);
                        MessageBox.Show("Success, Thank you for Returning the Item!");
                        Close();
                }

                if (toggle_Switch1.Checked == true || toggle_Switch1.Checked == false)
                {
                    string val = "";
                    if (toggle_Switch1.Checked == true)
                    {
                        val = timeIn;
                    }
                    if (toggle_Switch1.Checked == false)
                    {
                        val = timeOut;
                    }
                    PurposeModel model = new PurposeModel(val);
                    UserModel u_model = new UserModel(entryForm.entryIDValue.textBox1.Text);

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
            if(toggle_Switch1.Checked == true && userControl21.radioButton1.Checked)
            {
                MessageBox.Show("You're already Timed In and trying to borrow/return Items, Time Out first!!");
                return false;
            }
            if (toggle_Switch1.Checked == true && userControl21.radioButton2.Checked)
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
            InitializeToggleSwitch(entryForm.entryIDValue.textBox1.Text);
        }

        private void toggle_Switch1_MouseClick(object sender, MouseEventArgs e)
        {
            toggleSwitchState = true;
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
                    foundString = true;
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            WinAPI.AnimateWindow(this.Handle, 4000, WinAPI.HOR_POSITIVE);
            this.Close();
        }
    }
}
