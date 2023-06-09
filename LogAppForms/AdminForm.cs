using LogAppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace LogAppForms
{
    public partial class AdminForm : Form
    {
        private SqlConnection _conn = new SqlConnection(GlobalConfig.ConnectString("SearchCN"));
        private SqlCommand cmd, cmd2, cmd3;
        private DataTable dt, dt2, dt3;
        private SqlDataAdapter adapter,adapter2, adapter3;
        private DataSet ds, ds2, ds3;
        private DataView dataView,dataView2, dataView3;
        private PrintDocument printDocument;

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e); //events for using the enter key for submiting
            }
        }

        public AdminForm()
        {
            InitializeComponent();

            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PrintListView();
            //Print();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 10;
            int y = 10;
            int cellPadding = 25;
            int cellHeight = 50;

            int columnCount = listView1.Columns.Count;
            int[] columnWidths = new int[columnCount];
            int tableWidth = 0;

            // Calculate the width of each column
            for (int i = 0; i < columnCount; i++)
            {
                columnWidths[i] = e.Graphics.MeasureString(listView1.Columns[i].Text, Font).ToSize().Width + cellPadding * 2;
                tableWidth += columnWidths[i];
            }

            // Draw table header
            for (int i = 0; i < columnCount; i++)
            {
                e.Graphics.FillRectangle(Brushes.LightGray, new Rectangle(x, y, columnWidths[i], cellHeight));
                e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, columnWidths[i], cellHeight));
                e.Graphics.DrawString(listView1.Columns[i].Text, Font, Brushes.Black, new Rectangle(x + cellPadding, y, columnWidths[i] - cellPadding * 2, cellHeight));
                x += columnWidths[i];
            }

            y += cellHeight;

            // Draw table rows
            foreach (ListViewItem item in listView1.Items)
            {
                x = 10;
                for (int i = 0; i < columnCount; i++)
                {
                    string cellText = item.SubItems[i].Text;
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, columnWidths[i], cellHeight));
                    e.Graphics.DrawString(cellText, Font, Brushes.Black, new Rectangle(x + cellPadding, y, columnWidths[i] - cellPadding * 2, cellHeight));
                    x += columnWidths[i];
                }

                y += cellHeight;
            }

            // Check if there are more pages to print
            e.HasMorePages = false;
        }

        private void PrintListView()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        void Print()
        {
            /*
            PrintDocument PrintDocument = new PrintDocument();
            PrintDocument.PrintPage += (object sender, PrintPageEventArgs e) =>
            {
                Font font = new Font("Arial", 12);
                float locY = 150.0f;
                float startLocLeft = e.MarginBounds.Left;
                float startLocRight = e.MarginBounds.Right;
                foreach (DataRow dataRow in dt.Rows)
                {
                    foreach (DataRow dr2 in dataView2.ToTable().Rows)
                    {
                        locY += 20.0f;   
                        PointF locationX = new System.Drawing.PointF(startLocLeft, 150);
                        PointF locationY = new System.Drawing.PointF(e.MarginBounds.Right, 150);
                        e.Graphics.DrawLine(Pens.Black, locationX, locationY);
                        e.Graphics.DrawString(dataRow[1].ToString(), font, Brushes.Black, startLocLeft, locY);
                    }
                }
            };
            PrintDocument.Print();
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _conn.Open();
            cmd = new SqlCommand("SELECT * FROM Students", _conn);
            cmd2 = new SqlCommand("SELECT * FROM DateTimeTable", _conn);
            cmd3 = new SqlCommand("SELECT * FROM Items", _conn);

            adapter = new SqlDataAdapter(cmd);
            adapter2 = new SqlDataAdapter(cmd2);
            adapter3 = new SqlDataAdapter(cmd3);

            ds = new DataSet();
            ds2 = new DataSet();
            ds3 = new DataSet();

            adapter.Fill(ds, "dbo.Students");
            adapter2.Fill(ds2, "dbo.DateTimeTable");
            adapter3.Fill(ds3, "dbo.Items");
            
            _conn.Close();

            dt = ds.Tables["dbo.Students"];
            dt2 = ds2.Tables["dbo.DateTimeTable"];
            dt3 = ds3.Tables["dbo.Items"];

            dataView = new DataView(dt);
            dataView2 = new DataView(dt2);
            dataView3 = new DataView(dt3);

            filter();
        }

        private void filter()
        {
            listView1.Items.Clear();

            var value = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(value))
            {
                dataView2.RowFilter = string.Format("Convert([StudentIdNumber], System.String) LIKE '%{0}%'", value);
                HashSet<string> uniqueItems = new HashSet<string>();//stores address

                foreach (DataRow dr in dataView2.ToTable().Rows)
                {
                    string studentIdNumber = dr[1].ToString(); //store the value of what column StudentIdNumber of each row on above 'foreach'
                    DataRow[] relatedRows = dataView.ToTable().Select($"StudentId = '{studentIdNumber}'"); //combine StudentID and StudentIdNumber of 2 different table with the same value

                    foreach (DataRow dr2 in relatedRows)
                    {
                        string itemText = $"{dr[1]} {dr[2]}";

                        if (!uniqueItems.Contains(itemText)) //check if theres unique adress in that item if not unique it wont do anything
                        {
                            listView1.Items.Add(new ListViewItem(new string[]
                            {
                                dr[1].ToString(),
                                dr[2].ToString(),
                                dr[3].ToString(),
                                dr2[3].ToString(),
                                dr2[4].ToString(),
                                dr2[5].ToString()
                            }));

                            uniqueItems.Add(itemText); // add to array if unique
                        }
                    }
                }
            }
            else if(value.Contains("/")) //if the textbox contains / as a ways to search for date
            {
                dataView2.RowFilter = string.Format("Convert([CurDateTime], System.String) LIKE '%{0}%'", value);
                HashSet<string> uniqueItems = new HashSet<string>();

                foreach (DataRow dr in dataView2.ToTable().Rows)
                {
                    string studentIdNumber = dr[1].ToString();
                    DataRow[] relatedRows = dataView.ToTable().Select($"StudentId = '{studentIdNumber}'");

                    foreach (DataRow dr2 in relatedRows)
                    {
                        string itemText = $"{dr[1]} {dr[2]}";

                        if (!uniqueItems.Contains(itemText)) 
                        {
                            listView1.Items.Add(new ListViewItem(new string[]
                            {
                                dr[1].ToString(),
                                dr[2].ToString(),
                                dr[3].ToString(),
                                dr2[3].ToString(),
                                dr2[4].ToString(),
                                dr2[5].ToString()
                            }));

                            uniqueItems.Add(itemText);
                        }
                    }
                }
            }
            else
            {
                dataView.RowFilter = string.Format("Convert([StudentID], System.String) LIKE '%{0}%' OR " +
                    "Convert([Age], System.String) LIKE '%{0}%' OR LastName LIKE '%{0}%' OR FirstName LIKE '%{0}%'", value);
                dataView2.RowFilter = string.Format("Convert([StudentIdNumber], System.String) LIKE '%{0}%'", value);

                HashSet<string> uniqueItems = new HashSet<string>();

                foreach (DataRow dr in dataView2.ToTable().Rows)
                {
                    foreach (DataRow dr2 in dataView.ToTable().Rows)
                    {
                        string itemText = $"{dr[1]} {dr[2]}";

                        if (!uniqueItems.Contains(itemText))
                        {
                            listView1.Items.Add(new ListViewItem(new string[]
                            {
                                dr[1].ToString(),
                                dr[2].ToString(),
                                dr[3].ToString(),
                                dr2[3].ToString(),
                                dr2[4].ToString(),
                                dr2[5].ToString()
                            }));

                            uniqueItems.Add(itemText);
                        }
                    }
                }
            }
        }

        //inventory ---------------------------------------------------

        private void button2_Click(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }
        private void PopulateDataGridView()
        {
            try
            {
                _conn.Open();

                cmd = new SqlCommand("SELECT * FROM Items", _conn);
                adapter = new SqlDataAdapter(cmd);
                ds = new DataSet();
                adapter.Fill(ds, "dbo.Items");
                _conn.Close();
                dt = ds.Tables["dbo.Items"];

                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred on opening a connection saying: " + ex.Message);
            }
            // add columns to the DataGridView
            foreach (DataColumn column in dt.Columns)
            {
                dataGridView1.Columns.Add(column.ColumnName, column.ColumnName);
            }

            // add rows to the DataGridView
            foreach (DataRow row in dt.Rows)
            {
                dataGridView1.Rows.Add(row.ItemArray);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            AddItem addItem = new AddItem();
            addItem.Show();
        }
    }
}
