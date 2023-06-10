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
    public partial class RemoveItem : Form
    {
        private SqlConnection _conn = new SqlConnection(GlobalConfig.ConnectString("SearchCN"));
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataSet ds;

        public RemoveItem()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You will decrease a Quantity of an Item. Do you want to proceed?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ItemModel item = new ItemModel(
                            comboBox1.Text,
                            numericUpDown1.Value
                        );

                GlobalConfig.DataConnections.RemoveItem(item);
                MessageBox.Show("Subtracted Quantity");
                this.Close();
            }
        }

        private void RemoveItem_Load(object sender, EventArgs e)
        {
            _conn.Open();
            cmd = new SqlCommand("SELECT * FROM Items", _conn);
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "dbo.Items");
            _conn.Close();

            DataTable dt = ds.Tables["dbo.Items"];

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "ItemName";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You will remove an item from the database. Do you want to proceed?", 
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes) 
            {
                ItemModel item = new ItemModel(
                            comboBox1.Text,
                            numericUpDown1.Value
                        );

                GlobalConfig.DataConnections.RemoveItemName(item);
                MessageBox.Show("Removed Item");
                this.Close();
            }
        }
    }
}
