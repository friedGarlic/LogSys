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

namespace LogAppForms
{
    public partial class UserControl2 : UserControl
    {
        private SqlCommand cmd;
        private SqlDataAdapter adapter;
        private DataSet ds;

        public UserControl2()
        {
            InitializeComponent();
        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(GlobalConfig.ConnectString("SearchCN"));

            conn.Open();
            cmd = new SqlCommand("SELECT * FROM Items", conn);
            adapter = new SqlDataAdapter(cmd);
            ds = new DataSet();
            adapter.Fill(ds, "dbo.Items");
            conn.Close();

            DataTable dt = ds.Tables["dbo.Items"];

            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "ItemName";
        }

    }
}
