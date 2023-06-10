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
            ItemModel item = new ItemModel(
                                textBox1.Text,
                                numericUpDown1.Value
                            );
            GlobalConfig.DataConnections.CreateItem(item);
            MessageBox.Show("Added Successfuly");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You will Create an Item to DataBase. Do you want to proceed?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (validateForm())
                {
                    ItemModel item = new ItemModel(textBox1.Text);
                    if (!GlobalConfig.DataConnections.IsItemDuplicate(item))
                    {
                        CreateItem();
                    }
                    else
                    {
                        MessageBox.Show("The Item is already on the Database!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Fill the boxes properly!", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("You will Add a Quantity of an Item. Do you want to proceed?",
                "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ItemModel item = new ItemModel(
                            textBox1.Text,
                            numericUpDown1.Value
                        );
                GlobalConfig.DataConnections.AddQuantityItem(item);
            }
        }
    }
}
