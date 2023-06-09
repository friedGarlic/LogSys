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
    public partial class AddItem : Form
    {
        public AddItem()
        {
            InitializeComponent();
        }

        public void CreateItem()
        {
            if (validateForm())
            {
                ItemModel item = new ItemModel(
                            textBox1.Text,
                            numericUpDown1.Value
                        );

                GlobalConfig.DataConnections.CreateItem(item);
                MessageBox.Show("Registered Successfuly");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateItem();
        }
        public bool validateForm()
        {
            if (textBox1.TextLength == 0)
            {
                return false;
            }
            if (numericUpDown1.Value <= 0) 
            { 
                return false; 
            }

            return true;
        }
    }
}
