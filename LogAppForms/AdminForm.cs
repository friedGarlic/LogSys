﻿using LogAppLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Drawing.Imaging;
using System.IO;

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

        bool drag = false;
        Point start_point = new Point(0, 0);

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                button1_Click(sender, e); //events for using the enter key for submiting
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RemoveItem removeItem = new RemoveItem();
            removeItem.Show();
        }

        public AdminForm()
        {
            InitializeComponent();
            printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadChart();
            OpenConnectionFilter();
            HashSet<string> uniqueItems = new HashSet<string>();//stores address

            foreach (DataRow dr3 in dataView3.ToTable().Rows)
            {
                foreach (DataRow dr in dataView2.ToTable().Rows)
                {
                    string studentIdNumber = dr[1].ToString(); //store the value of what column StudentIdNumber of each row on above 'foreach'
                    DataRow[] relatedRows = dataView.ToTable().Select($"StudentId = '{studentIdNumber}'"); //combine StudentID and StudentIdNumber of 2 different table with the same value

                    foreach (DataRow dr2 in relatedRows)
                    {
                        string itemText = $"{dr[1]} {dr[2]}";

                        if (dr[3].ToString().Contains("Borrow") || dr[3].ToString().Contains("Return"))
                        {
                            if (!uniqueItems.Contains(itemText)) //check if theres unique adress in that item if not unique it wont do anything
                            {
                                listView1.Items.Add(new ListViewItem(new string[]
                                {
                                    dr[1].ToString(),
                                    dr[2].ToString(),
                                    dr[3].ToString(),
                                    dr2[3].ToString(),
                                    dr2[4].ToString(),
                                    dr2[5].ToString(),
                                    dr[4].ToString(),
                                    dr[5].ToString()
                                }));

                                uniqueItems.Add(itemText); // add to array if unique
                            }
                        }
                        else
                        {
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
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            drag = true;
            start_point = new Point(e.X, e.Y);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - start_point.X, p.Y - start_point.Y); ;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PrintListView();
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            int x = 10;
            int y = 10;
            int cellPadding = 20;
            int cellHeight = 40;

            int rowCount = listView1.Items.Count;
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
                int currentColumnCount = item.SubItems.Count; // Get the number of columns for the current row

                for (int i = 0; i < currentColumnCount; i++)
                {
                    string cellText = item.SubItems[i].Text;
                    e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, columnWidths[i], cellHeight));
                    e.Graphics.DrawString(cellText, Font, Brushes.Black, new Rectangle(x + cellPadding, y, columnWidths[i] - cellPadding * 2, cellHeight));
                    x += columnWidths[i];
                }

                // Handle extra columns for rows with fewer columns
                if (currentColumnCount < columnCount)
                {
                    int remainingColumns = columnCount - currentColumnCount;
                    for (int i = 0; i < remainingColumns; i++)
                    {
                        e.Graphics.DrawRectangle(Pens.Black, new Rectangle(x, y, columnWidths[currentColumnCount + i], cellHeight));
                        x += columnWidths[currentColumnCount + i];
                    }
                }

                y += cellHeight;
            }

            // Check if there are more pages to print
            e.HasMorePages = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LogSysReport.pdf";
            MessageBox.Show("Saved as PDF to Desktop");
            SaveAsPDF(filePath);
        }

        private void PrintListView()
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                PrintDocument printDocument = new PrintDocument();
                printDocument.PrintPage += PrintDocument_PrintPage;
                printDocument.Print();
            }
        }

        private void LoadChart()
        {
            using (SqlConnection connection = new SqlConnection(GlobalConfig.ConnectString("SearchCN")))
            {
                connection.Open();

                string sqlQuery = "SELECT DISTINCT StudentIdNumber FROM DateTimeTable";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string studentId = reader.GetString(0); // Assuming StudentIdNumber is of type VARCHAR

                            UserModel model = new UserModel(studentId);
                            TimeSpan totalDuration = GetTotalDuration(model);

                            chart1.Series["Hours"].Points.AddXY(studentId, totalDuration.Minutes);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\EkisReport.pdf";
            SaveAsAnalytics(filePath, this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            if (textBox1.TextLength == 0)
            {
                LoadChart();
            }
            else
            {
                UserModel model = new UserModel(textBox1.Text);
                TimeSpan totalDuration = GetTotalDuration(model);

                chart1.Series["Hours"].Points.AddXY(textBox1.Text, totalDuration.Minutes);
            }
            OpenConnectionFilter();


            filter();
        }
        private void OpenConnectionFilter()
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
        }
        private void filter()
        {
            listView1.Items.Clear();

            var value = textBox1.Text.Trim();

            if (string.IsNullOrEmpty(value))
            {
                
                dataView2.RowFilter = string.Format("Convert([StudentIdNumber], System.String) LIKE '%{0}%'", value);
                HashSet<string> uniqueItems = new HashSet<string>();//stores address

                foreach (DataRow dr3 in dataView3.ToTable().Rows)
                {
                    foreach (DataRow dr in dataView2.ToTable().Rows)
                    {
                        string studentIdNumber = dr[1].ToString(); //store the value of what column StudentIdNumber of each row on above 'foreach'
                        DataRow[] relatedRows = dataView.ToTable().Select($"StudentId = '{studentIdNumber}'"); //combine StudentID and StudentIdNumber of 2 different table with the same value

                        foreach (DataRow dr2 in relatedRows)
                        {
                            string itemText = $"{dr[1]} {dr[2]}";

                            if (dr[3].ToString().Contains("Borrow") || dr[3].ToString().Contains("Return"))
                            {
                                if (!uniqueItems.Contains(itemText)) //check if theres unique adress in that item if not unique it wont do anything
                                {
                                    listView1.Items.Add(new ListViewItem(new string[]
                                    {
                                    dr[1].ToString(),
                                    dr[2].ToString(),
                                    dr[3].ToString(),
                                    dr2[3].ToString(),
                                    dr2[4].ToString(),
                                    dr2[5].ToString(),
                                    dr[4].ToString(),
                                    dr[5].ToString()
                                    }));

                                    uniqueItems.Add(itemText); // add to array if unique
                                }
                            }
                            else
                            {
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
                }
            }
            else if(value.Contains("/")) //if the textbox contains / as a ways to search for date
            {
                dataView2.RowFilter = string.Format("Convert([CurDateTime], System.String) LIKE '%{0}%'", value);
                HashSet<string> uniqueItems = new HashSet<string>();

                foreach (DataRow dr3 in dataView3.ToTable().Rows)
                {
                    foreach (DataRow dr in dataView2.ToTable().Rows)
                    {
                        string studentIdNumber = dr[1].ToString(); //store the value of what column StudentIdNumber of each row on above 'foreach'
                        DataRow[] relatedRows = dataView.ToTable().Select($"StudentId = '{studentIdNumber}'"); //combine StudentID and StudentIdNumber of 2 different table with the same value

                        foreach (DataRow dr2 in relatedRows)
                        {
                            string itemText = $"{dr[1]} {dr[2]}";

                            if (dr[3].ToString().Contains("Borrow"))
                            {
                                if (!uniqueItems.Contains(itemText) || dr[3].ToString().Contains("Return")) //check if theres unique adress in that item if not unique it wont do anything
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
                            else
                            {
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
                }
            }
            else
            {
                dataView.RowFilter = string.Format("Convert([StudentID], System.String) LIKE '%{0}%' OR " +
                    "Convert([Age], System.String) LIKE '%{0}%' OR LastName LIKE '%{0}%' OR FirstName LIKE '%{0}%'", value);
                dataView2.RowFilter = string.Format("Convert([StudentIdNumber], System.String) LIKE '%{0}%'", value);

                HashSet<string> uniqueItems = new HashSet<string>();

                foreach (DataRow dr3 in dataView3.ToTable().Rows)
                {
                    foreach (DataRow dr in dataView2.ToTable().Rows)
                    {
                        foreach (DataRow dr2 in dataView.ToTable().Rows)
                        {
                            string itemText = $"{dr[1]} {dr[2]}";

                            if (dr[3].ToString().Contains("Borrow") || dr[3].ToString().Contains("Return"))
                            {
                                if (!uniqueItems.Contains(itemText)) //check if there's a unique address in that item if not unique it won't do anything
                                {
                                    listView1.Items.Add(new ListViewItem(new string[]
                                    {
                                dr[1].ToString(),
                                dr[2].ToString(),
                                dr[3].ToString(),
                                dr2[3].ToString(),
                                dr2[4].ToString(),
                                dr2[5].ToString(),
                                dr[4].ToString(),
                                dr[5].ToString()
                                    }));

                                    uniqueItems.Add(itemText); // add to array if unique
                                }
                            }
                            else
                            {
                                if (!uniqueItems.Contains(itemText)) //check if there's a unique address in that item if not unique it won't do anything
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
                }
            }
        }

        //inventory ---------------------------------------------------

        private void button2_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            PopulateListView();
        }
        private void PopulateListView()
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

                dataView = new DataView(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred on opening a connection saying: " + ex.Message);
            }

            foreach (DataRow dr in dataView.ToTable().Rows)
            {
                listView2.Items.Add(new ListViewItem(new string[]
                                    {
                                        dr[1].ToString(),
                                        dr[2].ToString()
                                    })) ;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            AddItem addItem = new AddItem();
            addItem.Show();
        }
        public UserModel GetTimeDate(UserModel userModel)
        {
            using (SqlConnection connection = new SqlConnection(GlobalConfig.ConnectString("SearchCN")))
            {
                connection.Open();

                string sqlQuery = "SELECT CurDateTime FROM DateTimeTable WHERE StudentIdNumber = @StudentIdNumber";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentIdNumber", userModel.student_ID);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime dateTimeValue = reader.GetDateTime(0); // Assuming CurDateTime is of type DATETIME
                            Console.WriteLine(dateTimeValue);
                        }
                    }
                }

                connection.Close();

                return userModel;
            }
        }

        public TimeSpan GetTotalDuration(UserModel userModel)
        {
            TimeSpan totalDuration = TimeSpan.Zero;
            DateTime? previousDateTime = null;
            bool isInsideTimeRange = false;

            using (SqlConnection connection = new SqlConnection(GlobalConfig.ConnectString("SearchCN")))
            {
                connection.Open();

                string sqlQuery = "SELECT CurDateTime, TimeInOut FROM DateTimeTable WHERE StudentIdNumber = @StudentIdNumber ORDER BY CurDateTime";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@StudentIdNumber", userModel.student_ID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime currentDateTime = reader.GetDateTime(0); // Assuming CurDateTime is of type DATETIME
                            string eventType = reader.GetString(1); // Assuming TimeInOut is of type VARCHAR

                            if (eventType == "Time In")
                            {
                                if (!isInsideTimeRange)
                                {
                                    previousDateTime = currentDateTime;
                                    isInsideTimeRange = true;
                                }
                            }
                            else if (eventType == "Time Out")
                            {
                                if (isInsideTimeRange && previousDateTime.HasValue)
                                {
                                    TimeSpan duration = currentDateTime - previousDateTime.Value;
                                    totalDuration += duration;
                                    isInsideTimeRange = false;
                                }
                            }
                        }
                    }
                }

                connection.Close();
            }

            Console.WriteLine(totalDuration);
            return totalDuration;
        }
        private void SaveAsPDF(string filePath)
        {
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 7);

            int x = 10;
            int y = 10;
            int cellPadding = 20;
            int cellHeight = 40;

            int rowCount = listView1.Items.Count;
            int columnCount = listView1.Columns.Count;
            int[] columnWidths = new int[columnCount];
            int tableWidth = 0;

            // Calculate the width of each column
            for (int i = 0; i < columnCount; i++)
            {
                columnWidths[i] = (int)gfx.MeasureString(listView1.Columns[i].Text, font).Width + cellPadding * 2;
                tableWidth += columnWidths[i];
            }

            // Draw table header
            for (int i = 0; i < columnCount; i++)
            {
                gfx.DrawRectangle(XPens.Black, x, y, columnWidths[i], cellHeight);
                gfx.DrawString(listView1.Columns[i].Text, font, XBrushes.Black, new XRect(x + cellPadding, y, columnWidths[i] - cellPadding * 2, cellHeight), XStringFormats.Center);
                x += columnWidths[i];
            }

            y += cellHeight;

            // Draw table rows
            foreach (ListViewItem item in listView1.Items)
            {
                x = 10;
                int currentColumnCount = item.SubItems.Count; // Get the number of columns for the current row

                for (int i = 0; i < currentColumnCount; i++)
                {
                    string cellText = item.SubItems[i].Text;
                    gfx.DrawRectangle(XPens.Black, x, y, columnWidths[i], cellHeight);
                    gfx.DrawString(cellText, font, XBrushes.Black, new XRect(x + cellPadding, y, columnWidths[i] - cellPadding * 2, cellHeight), XStringFormats.Center);
                    x += columnWidths[i];
                }

                // Handle extra columns for rows with fewer columns
                if (currentColumnCount < columnCount)
                {
                    int remainingColumns = columnCount - currentColumnCount;
                    for (int i = 0; i < remainingColumns; i++)
                    {
                        gfx.DrawRectangle(XPens.Black, x, y, columnWidths[currentColumnCount + i], cellHeight);
                        x += columnWidths[currentColumnCount + i];
                    }
                }

                y += cellHeight;
            }

            document.Save(filePath);
        }
        public void SaveAsAnalytics(string filePath, Form form)
        {
            PdfDocument pdfDocument = new PdfDocument();

            PdfPage pdfPage = pdfDocument.AddPage();

            XGraphics pdfGraphics = XGraphics.FromPdfPage(pdfPage);

            using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));

                using (MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Png);

                    XImage xImage = XImage.FromStream(ms);

                    pdfGraphics.DrawImage(xImage, 150, 150);
                }
            }

            pdfDocument.Save(filePath);
        }
    }
}
