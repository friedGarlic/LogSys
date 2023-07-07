using LogAppLibrary;
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

        private bool drag = false;
        private Point start_point = new Point(0, 0);

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
            OpenConnectionFilter();
            LoadChart();
            LoadPieChart();
            filter();
            PopulateListView();
            NotifWnd(); 
            GetUnreturnedItem();
            GetOverAllItems();
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
            PrintedBy printedBy = new PrintedBy(this);
            printedBy.Show();
                
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

                            chart1.Series["Minutes"].Points.AddXY(studentId, totalDuration.Minutes);
                        }
                    }
                }

                connection.Close();
            }
        }
        private void LoadChartByDate()
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
                            TimeSpan totalDuration = GetTotalDurationByDate(dateTimePicker1.Value,dateTimePicker2.Value,model);

                            chart1.Series["Minutes"].Points.AddXY(studentId, totalDuration.Minutes);
                        }
                    }
                }

                connection.Close();
            }
        }
        private void LoadPieChart()
        {
            using (SqlConnection connection = new SqlConnection(GlobalConfig.ConnectString("SearchCN")))
            {
                connection.Open();

                string sqlQuery = "SELECT ItemName, Quantity FROM Items";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string itemName = reader.GetString(0);
                            int quantity = reader.GetInt32(1);

                            chart2.Series["NumberOfItems"].Points.AddXY(itemName, quantity);
                        }
                    }
                }

                connection.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\LogReport.pdf";
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

                chart1.Series["Minutes"].Points.AddXY(textBox1.Text, totalDuration.Minutes);
            }
            OpenConnectionFilter();


            filter();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear(); 
            foreach (var series in chart2.Series)
            {
                series.Points.Clear();
            }
            PopulateListView();
            LoadPieChart();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddItem addItem = new AddItem();
            addItem.Show();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            LoadChartByDate();
            OpenConnectionFilter();
            FilterListView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PrintListView();
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
        private void FilterListView()
        {
            DateTime startDate = dateTimePicker1.Value.Date;
            DateTime endDate = dateTimePicker2.Value.Date;
            listView1.Items.Clear();
            HashSet<string> uniqueItems = new HashSet<string>();

            foreach (DataRow dr3 in dataView3.ToTable().Rows)
            {
                foreach (DataRow dr in dataView2.ToTable().Rows)
                {
                    foreach (DataRow dr2 in dataView.ToTable().Rows)
                    {
                        //filtering logic here
                        DateTime itemDate = dr.Field<DateTime>("CurDateTime");
                        string itemText = $"{dr[1]} {dr[2]}";

                        if (itemDate.Date >= startDate && itemDate.Date <= endDate && !uniqueItems.Contains(itemText))
                        {
                            // add item to the listview
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

                            uniqueItems.Add(itemText);
                        }
                    }
                }
            }
        }
        //inventory ---------------------------------------------------


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
                                        dr[2].ToString(),
                                        dr[6].ToString()
                                    }));
            }
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
                            DateTime dateTimeValue = reader.GetDateTime(0);
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
                            DateTime currentDateTime = reader.GetDateTime(0); 
                            string eventType = reader.GetString(1); 

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

            return totalDuration;
        }
        public void SaveAsPDF(string filePath, string printedBy) /////////public method careful
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
            int pageCount = 1;

            // calculate the width of each column
            for (int i = 0; i < columnCount; i++)
            {
                columnWidths[i] = (int)gfx.MeasureString(listView1.Columns[i].Text, font).Width + cellPadding * 2;
                tableWidth += columnWidths[i];
            }

            y += cellHeight;

            Bitmap logoBitmap = Properties.Resources.prmsu_logo;

            // Convert the Bitmap to a MemoryStream
            MemoryStream memoryStream = new MemoryStream();
            logoBitmap.Save(memoryStream, ImageFormat.Png);

            // Create the XImage from the MemoryStream
            XImage logoImage = XImage.FromStream(memoryStream);

            // Determine the size and position of the logo
            int logoWidth = 70;
            int logoHeight = 70;
            int logoX = 80;
            int logoY = y - 25; // Adjust the Y position based on the desired alignment

            // Draw the logo on the page
            gfx.DrawImage(logoImage, logoX, logoY, logoWidth, logoHeight);

            // Draw header
            string headerText = "President Ramon Magsaysay State University";
            gfx.DrawString(headerText, new XFont("Arial", 12, XFontStyle.Bold), XBrushes.Black, 
                new XPoint(page.Width / 2 - gfx.MeasureString(headerText, new XFont("Arial", 12, XFontStyle.Bold)).Width / 2, y));

            y += 10;
            string AdditionalText = "Nagbunga, San Marcelino, Zambales 2207";
            XSize additionalTextSize = gfx.MeasureString(AdditionalText, font);
            XPoint additionalTextPosition = new XPoint(page.Width / 2 - additionalTextSize.Width / 2, y);
            gfx.DrawString(AdditionalText, font, XBrushes.Black, additionalTextPosition);

            y += 10;
            string AdditionalText1 = $"CCIT Laboratory Log Report ({DateTime.Now.Year})";
            XSize additionalTextSize1 = gfx.MeasureString(AdditionalText1, font);
            XPoint additionalTextPosition1 = new XPoint(page.Width / 2 - additionalTextSize1.Width / 2, y);
            gfx.DrawString(AdditionalText1, font, XBrushes.Black, additionalTextPosition1);
            //printed by

            y += 10;
            string AdditionalText2 = $"{printedBy}";
            XSize additionalTextSize2 = gfx.MeasureString(AdditionalText2, font);
            XPoint additionalTextPosition2 = new XPoint(page.Width / 2 - additionalTextSize2.Width / 2, y);
            gfx.DrawString(AdditionalText2, font, XBrushes.Black, additionalTextPosition2);

            int availableHeight = (int)page.Height - y - cellHeight; // calculate the available height for table content

            y += cellHeight;
            // draw table header
            for (int i = 0; i < columnCount; i++)
            {
                gfx.DrawRectangle(XPens.Black, x, y, columnWidths[i], cellHeight);
                gfx.DrawString(listView1.Columns[i].Text, font, XBrushes.Black, 
                    new XRect(x + cellPadding, y, columnWidths[i] - cellPadding * 2, cellHeight), XStringFormats.Center);
                x += columnWidths[i];
            }

            y += cellHeight;

            // draw table rows
            int rowIndex = 0;
            bool isNewPage = false;

            while (rowIndex < rowCount)
            {
                x = 10;
                int currentColumnCount = listView1.Items[rowIndex].SubItems.Count; // get the number of columns for the current row

                // check if the remaining height is not enough for the current row
                if (y + cellHeight > page.Height - cellHeight)
                {
                    // Add a new page
                    page = document.AddPage();
                    gfx = XGraphics.FromPdfPage(page);
                    y = 10;
                    isNewPage = true;
                }

                for (int i = 0; i < currentColumnCount; i++)
                {
                    string cellText = listView1.Items[rowIndex].SubItems[i].Text;
                    gfx.DrawRectangle(XPens.Black, x, y, columnWidths[i], cellHeight);
                    gfx.DrawString(cellText, font, XBrushes.Black, 
                        new XRect(x + cellPadding, y, columnWidths[i] - cellPadding * 2, cellHeight), XStringFormats.Center);
                    x += columnWidths[i];
                }

                // handle extra columns for rows with fewer columns
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
                rowIndex++;

                // check if a new page was added and adjust the y position accordingly
                if (isNewPage)
                {
                    y = 10;
                    isNewPage = false;
                    pageCount += 1;
                }
                gfx.DrawString($"Printed By: {DateTime.Now.ToString()} {printedBy}", font, XBrushes.Black, new XPoint(10, page.Height - cellHeight));
                gfx.DrawString($"Page: {pageCount}", font, XBrushes.Black, new XPoint(page.Width - 150, page.Height - cellHeight));
            }

            // draw the chart
            PdfPage chartPage = document.AddPage();
            XGraphics chartGfx = XGraphics.FromPdfPage(chartPage);
            XImage xImage = null;
            using (Bitmap chartImage = new Bitmap(chart1.Width, chart1.Height))
            {
                chart1.DrawToBitmap(chartImage, new Rectangle(0, 0, chart1.Width, chart1.Height));
                xImage = XImage.FromGdiPlusImage(chartImage);
                chartGfx.DrawImage(xImage, 
                    new XRect(chartPage.Width - chart1.Width - 10, chartPage.Height - chart1.Height - 10, chart1.Width, chart1.Height));
            }

            document.Save(filePath);
            document.Close();
        }
        private void NotifWnd()
        {
            popupNotifier1.TitleText = "Click to Enter Security Window";
            popupNotifier1.ContentText = "There are many undesired instances that happened!";
            popupNotifier1.Click += Popup_Clicked;
            popupNotifier1.Popup();
        }
        private void Popup_Clicked(object sender, EventArgs e)
        {
            Security security = new Security();
            security.Show();
        }
        private TimeSpan GetTotalDurationByDate(DateTime startDatePicker, DateTime endDatePicker, UserModel userModel)
        {
            DateTime startDate = startDatePicker;
            DateTime endDate = endDatePicker;

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
                            DateTime currentDateTime = reader.GetDateTime(0); // pag CurDateTime is of type DATETIME
                            string eventType = reader.GetString(1); // pag TimeInOut is of type VARCHAR

                            if (currentDateTime >= startDate && currentDateTime <= endDate)
                            {
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
                }

                connection.Close();
            }

            return totalDuration;
        }
        public void GetUnreturnedItem()
        {
            _conn.Open();
            cmd = new SqlCommand("SELECT SUM(UnreturnedItems) FROM dbo.Items", _conn);
            int overallUnreturnedItems = (int)cmd.ExecuteScalar();
            _conn.Close();

            label6.Text = overallUnreturnedItems.ToString();
        }
        public void GetOverAllItems()
        {
            _conn.Open();
            cmd = new SqlCommand("SELECT SUM(Quantity) FROM dbo.Items", _conn);
            int overallUnreturnedItems = (int)cmd.ExecuteScalar();
            _conn.Close();

            label9.Text = overallUnreturnedItems.ToString();
        }
    }
}
