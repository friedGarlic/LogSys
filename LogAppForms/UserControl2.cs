using LogAppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
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

            int radius = 20; // Adjust the radius value as per your preference
            GraphicsPath path = new GraphicsPath();
            path.AddArc(0, 0, radius, radius, 180, 90);
            path.AddArc(Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90);
            path.AddArc(0, Height - radius, radius, radius, 90, 90);
            Region = new Region(path);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PurposeForm purposeForm = new PurposeForm();
            if (!purposeForm.IsItemAvailable(comboBox1.Text))
            {
                label4.Text = "Not Available";
                label4.ForeColor = Color.Red;
            }
            else
            {
                label4.Text = "Available";
                label4.ForeColor = Color.Green;
            }
        }
    }
}
