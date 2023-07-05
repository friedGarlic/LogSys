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
    public partial class PrintedBy : Form
    {
        private AdminForm admin;
        public PrintedBy()
        {
            InitializeComponent();
        }
        public PrintedBy(AdminForm admin)
        {
            this.admin = admin;
            InitializeComponent();
        }
        private void printby_Load(object sender, EventArgs e)
        {

        }

        private void register_button_Click(object sender, EventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LogSysReport.pdf"; //file path straight to desktop path
            MessageBox.Show("Saved as PDF to Desktop");
            admin.SaveAsPDF(filePath, printby.textBox1.Text);
            Close();
        }
    }
}
