using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;
using iTextSharp.text.pdf;
using PdfDocument = iTextSharp.text.Document;
using PdfFont = iTextSharp.text.Font;
using PdfParagraph = iTextSharp.text.Paragraph;
using PdfTable = iTextSharp.text.pdf.PdfPTable;
using PdfCell = iTextSharp.text.pdf.PdfPCell;
using System.Drawing.Printing;

namespace QRAttendanceSystem
{
    public partial class ReportsForm : Form
    {
        private DataGridView reportsGrid;

        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private TextBox txtSearch;
        private ComboBox cmbStatus;

        private Label lblTotal;
        private Label lblPresent;
        private Label lblLate;
        private Label lblAbsent;

        private Button btnGenerate;
        private Button btnRefresh;
        private Button btnExport;
        private Button btnPrintPreview;
        private Button btnPrint;

        private PrintDocument printDocument;
        private PrintPreviewDialog printPreviewDialog;

        private int printRowIndex = 0;

        public ReportsForm()
        {
            InitializeComponent();
            BuildUI();

            dtpFrom.Value = DateTime.Today;
            dtpTo.Value = DateTime.Today;

            LoadReport();
        }


        private void BuildUI()
        {
            this.Text = "Attendance Reports";
            this.Size = new Size(1450, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;

            // TITLE
            Label lblTitle = new Label();
            lblTitle.Text = "Attendance Reports";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Generate attendance summaries and view attendance records";
            lblSubtitle.Font = new Font("Segoe UI", 10);
            lblSubtitle.ForeColor = Color.FromArgb(160, 170, 190);
            lblSubtitle.Location = new Point(32, 65);
            lblSubtitle.AutoSize = true;
            this.Controls.Add(lblSubtitle);

            // FROM DATE
            Label lblFrom = new Label();
            lblFrom.Text = "From";
            lblFrom.ForeColor = Color.White;
            lblFrom.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblFrom.Location = new Point(30, 110);
            lblFrom.AutoSize = true;
            this.Controls.Add(lblFrom);

            dtpFrom = new DateTimePicker();
            dtpFrom.Location = new Point(30, 135);
            dtpFrom.Size = new Size(150, 30);
            dtpFrom.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dtpFrom);

            // TO DATE
            Label lblTo = new Label();
            lblTo.Text = "To";
            lblTo.ForeColor = Color.White;
            lblTo.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblTo.Location = new Point(200, 110);
            lblTo.AutoSize = true;
            this.Controls.Add(lblTo);

            dtpTo = new DateTimePicker();
            dtpTo.Location = new Point(200, 135);
            dtpTo.Size = new Size(150, 30);
            dtpTo.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dtpTo);

            // SEARCH
            Label lblSearch = new Label();
            lblSearch.Text = "Search";
            lblSearch.ForeColor = Color.White;
            lblSearch.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSearch.Location = new Point(370, 110);
            lblSearch.AutoSize = true;
            this.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(370, 135);
            txtSearch.Size = new Size(230, 30);
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.BackColor = Color.FromArgb(30, 39, 58);
            txtSearch.ForeColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtSearch);

            // STATUS
            Label lblStatus = new Label();
            lblStatus.Text = "Status";
            lblStatus.ForeColor = Color.White;
            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblStatus.Location = new Point(620, 110);
            lblStatus.AutoSize = true;
            this.Controls.Add(lblStatus);

            cmbStatus = new ComboBox();
            cmbStatus.Location = new Point(620, 135);
            cmbStatus.Size = new Size(140, 30);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbStatus.Items.Add("All");
            cmbStatus.Items.Add("Present");
            cmbStatus.Items.Add("Late");
            cmbStatus.Items.Add("Absent");

            cmbStatus.SelectedIndex = 0;

            this.Controls.Add(cmbStatus);

            // GENERATE BUTTON
            btnGenerate = new Button();
            btnGenerate.Text = "Generate";
            btnGenerate.Location = new Point(760, 133);
            btnGenerate.Size = new Size(120, 42);
            btnGenerate.BackColor = Color.FromArgb(45, 125, 210);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnGenerate.Cursor = Cursors.Hand;

            btnGenerate.Click += (sender, e) =>
            {
                if (dtpFrom.Value.Date > dtpTo.Value.Date)
                {
                    MessageBox.Show(
                        "The From date cannot be later than the To date.",
                        "Invalid Date Range",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                LoadReport();
            };



            this.Controls.Add(btnGenerate);

            // REFRESH
            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(885, 133);
            btnRefresh.Size = new Size(100, 42);
            btnRefresh.BackColor = Color.FromArgb(45, 55, 75);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;

            btnRefresh.Click += (sender, e) =>
            {
                dtpFrom.Value = DateTime.Today;
                dtpTo.Value = DateTime.Today;
                txtSearch.Clear();
                cmbStatus.SelectedIndex = 0;

                LoadReport();
            };

            printDocument = new PrintDocument();

            printDocument.PrintPage += PrintDocument_PrintPage;

            printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;
            printPreviewDialog.Width = 1000;
            printPreviewDialog.Height = 700;

            this.Controls.Add(btnRefresh);


            // EXPORT CSV BUTTON
            btnExport = new Button();
            btnExport.Text = "Export CSV";
            btnExport.Location = new Point(995, 133);
            btnExport.Size = new Size(110, 42);
            btnExport.BackColor = Color.FromArgb(35, 145, 100);
            btnExport.ForeColor = Color.White;
            btnExport.FlatStyle = FlatStyle.Flat;
            btnExport.FlatAppearance.BorderSize = 0;
            btnExport.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnExport.Cursor = Cursors.Hand;

            this.Controls.Add(btnExport);




            btnExport.Click += (sender, e) =>
            {
                if (reportsGrid.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "There are no attendance records to export.",
                        "Export Report",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV Files (*.csv)|*.csv";
                    saveDialog.FileName =
                        "Attendance_Report_" +
                        DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                        ".csv";

                    saveDialog.Title = "Export Attendance Report";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (System.IO.StreamWriter writer =
                                new System.IO.StreamWriter(saveDialog.FileName, false,
                                    System.Text.Encoding.UTF8))
                            {
                                for (int i = 0; i < reportsGrid.Columns.Count; i++)
                                {
                                    if (i > 0) writer.Write(",");
                                    writer.Write(
                                        "\"" +
                                        reportsGrid.Columns[i].HeaderText.Replace("\"", "\"\"") +
                                        "\"");
                                }

                                writer.WriteLine();

                                foreach (DataGridViewRow row in reportsGrid.Rows)
                                {
                                    for (int i = 0; i < reportsGrid.Columns.Count; i++)
                                    {
                                        if (i > 0) writer.Write(",");

                                        string value =
                                            row.Cells[i].Value?.ToString() ?? "";

                                        writer.Write(
                                            "\"" + value.Replace("\"", "\"\"") + "\"");
                                    }

                                    writer.WriteLine();
                                }
                            }

                            MessageBox.Show(
                                "Attendance report exported successfully!",
                                "Export Complete",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                "Export failed:\n" + ex.Message,
                                "Export Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            };


            // EXPORT PDF BUTTON
            Button btnExportPDF = new Button();
            btnExportPDF.Text = "Export PDF";
            btnExportPDF.Location = new Point(1115, 133);
            btnExportPDF.Size = new Size(110, 42);
            btnExportPDF.BackColor = Color.FromArgb(180, 65, 65);
            btnExportPDF.ForeColor = Color.White;
            btnExportPDF.FlatStyle = FlatStyle.Flat;
            btnExportPDF.FlatAppearance.BorderSize = 0;
            btnExportPDF.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnExportPDF.Cursor = Cursors.Hand;

            btnExportPDF.Click += (sender, e) =>
            {
                ExportReportToPDF();
            };

            this.Controls.Add(btnExportPDF);


            // PRINT PREVIEW BUTTON
            btnPrintPreview = new Button();
            btnPrintPreview.Text = "Print Preview";
            btnPrintPreview.Location = new Point(1080, 190);
            btnPrintPreview.Size = new Size(140, 42);
            btnPrintPreview.BackColor = Color.FromArgb(100, 85, 180);
            btnPrintPreview.ForeColor = Color.White;
            btnPrintPreview.FlatStyle = FlatStyle.Flat;
            btnPrintPreview.FlatAppearance.BorderSize = 0;
            btnPrintPreview.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnPrintPreview.Cursor = Cursors.Hand;

            btnPrintPreview.Click += (sender, e) =>
            {
                if (reportsGrid.Rows.Count == 0)
                {
                    MessageBox.Show("There are no records to preview.");
                    return;
                }

                printRowIndex = 0;
                printPreviewDialog.ShowDialog();
                printRowIndex = 0;
            };

            this.Controls.Add(btnPrintPreview);

            // PRINT BUTTON
            btnPrint = new Button();
            btnPrint.Text = "Print";
            btnPrint.Location = new Point(1235, 133);
            btnPrint.Size = new Size(90, 42);
            btnPrint.BackColor = Color.FromArgb(35, 145, 100);
            btnPrint.ForeColor = Color.White;
            btnPrint.FlatStyle = FlatStyle.Flat;
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnPrint.Cursor = Cursors.Hand;

            btnPrint.Click += (sender, e) =>
            {
                if (reportsGrid.Rows.Count == 0)
                {
                    MessageBox.Show("There are no records to print.");
                    return;
                }

                using (PrintDialog dialog = new PrintDialog())
                {
                    dialog.Document = printDocument;

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        printRowIndex = 0;
                        printDocument.Print();
                        printRowIndex = 0;
                    }
                }
            };

            this.Controls.Add(btnPrint);





            // SUMMARY CARDS
            CreateSummaryCard("TOTAL RECORDS", new Point(30, 250), out lblTotal);
            CreateSummaryCard("PRESENT", new Point(300, 250), out lblPresent);
            CreateSummaryCard("LATE", new Point(570, 250), out lblLate);
            CreateSummaryCard("ABSENT", new Point(840, 250), out lblAbsent);

            // DATAGRIDVIEW
            reportsGrid = new DataGridView();

            reportsGrid.Location = new Point(30, 370);
            reportsGrid.Size = new Size(1170, 370);

            reportsGrid.BackgroundColor = Color.FromArgb(24, 32, 48);
            reportsGrid.BorderStyle = BorderStyle.None;

            reportsGrid.AllowUserToAddRows = false;
            reportsGrid.AllowUserToDeleteRows = false;
            reportsGrid.ReadOnly = true;

            reportsGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            reportsGrid.MultiSelect = false;

            reportsGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            reportsGrid.EnableHeadersVisualStyles = false;

            reportsGrid.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(35, 48, 70);

            reportsGrid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            reportsGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            reportsGrid.DefaultCellStyle.BackColor =
                Color.FromArgb(24, 32, 48);

            reportsGrid.DefaultCellStyle.ForeColor =
                Color.White;

            reportsGrid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(45, 80, 120);

            reportsGrid.DefaultCellStyle.SelectionForeColor =
                Color.White;

            reportsGrid.RowTemplate.Height = 35;

            reportsGrid.Columns.Add(
                "student_id",
                "Student ID");

            reportsGrid.Columns.Add(
                "student_name",
                "Student Name");

            reportsGrid.Columns.Add(
                "subject",
                "Subject");

            reportsGrid.Columns.Add(
                "section",
                "Section");

            reportsGrid.Columns.Add(
                "attendance_date",
                "Date");

            reportsGrid.Columns.Add(
                "time_in",
                "Time In");

            reportsGrid.Columns.Add(
                "status",
                "Status");

            this.Controls.Add(reportsGrid);
        }

        private void CreateSummaryCard(
            string title,
            Point location,
            out Label valueLabel)
        {
            Panel card = new Panel();

            card.Location = location;
            card.Size = new Size(240, 85);

            card.BackColor =
                Color.FromArgb(30, 39, 58);

            this.Controls.Add(card);

            Label lblTitle = new Label();

            lblTitle.Text = title;
            lblTitle.Font =
                new Font("Segoe UI", 8, FontStyle.Bold);

            lblTitle.ForeColor =
                Color.FromArgb(150, 160, 180);

            lblTitle.Location =
                new Point(15, 10);

            lblTitle.AutoSize = true;

            card.Controls.Add(lblTitle);

            valueLabel = new Label();

            valueLabel.Text = "0";
            valueLabel.Font =
                new Font("Segoe UI", 22, FontStyle.Bold);

            valueLabel.ForeColor = Color.White;

            valueLabel.Location =
                new Point(15, 30);

            valueLabel.AutoSize = true;

            card.Controls.Add(valueLabel);
        }

        private void LoadReport()
        {
            reportsGrid.Rows.Clear();

            lblTotal.Text = "0";
            lblPresent.Text = "0";
            lblLate.Text = "0";
            lblAbsent.Text = "0";

            string search = txtSearch.Text.Trim();
            string status = cmbStatus.SelectedItem?.ToString() ?? "All";

            int total = 0;
            int present = 0;
            int late = 0;
            int absent = 0;

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            st.student_id,
                            CONCAT(
                                st.first_name,
                                ' ',
                                st.last_name
                            ) AS student_name,

                            CONCAT(
                                sub.subject_code,
                                ' - ',
                                sub.subject_name
                            ) AS subject,

                            st.section,

                            a.attendance_date,

                            TIME_FORMAT(
                                a.time_in,
                                '%h:%i %p'
                            ) AS time_in,

                            a.status

                        FROM attendance a

                        INNER JOIN students st
                            ON a.student_id = st.id

                        INNER JOIN subjects sub
                            ON a.subject_id = sub.id

                        WHERE
                            a.attendance_date
                            BETWEEN @fromDate AND @toDate

                        AND
                        (
                            st.student_id LIKE @search
                            OR st.first_name LIKE @search
                            OR st.last_name LIKE @search
                             OR CONCAT(st.first_name, ' ', st.last_name) LIKE @search
                            OR st.section LIKE @search
                            OR sub.subject_code LIKE @search
                            OR sub.subject_name LIKE @search
                        )";

                    if (status != "All")
                    {
                        query +=
                            " AND a.status = @status";
                    }

                    query += @"
                        ORDER BY
                            a.attendance_date DESC,
                            a.time_in ASC";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@fromDate",
                            dtpFrom.Value.Date);

                        command.Parameters.AddWithValue(
                            "@toDate",
                            dtpTo.Value.Date);

                        command.Parameters.AddWithValue(
                            "@search",
                            "%" + search + "%");

                        if (status != "All")
                        {
                            command.Parameters.AddWithValue(
                                "@status",
                                status);
                        }

                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string recordStatus =
                                    reader["status"].ToString();

                                total++;

                                if (recordStatus == "Present")
                                    present++;

                                else if (recordStatus == "Late")
                                    late++;

                                else if (recordStatus == "Absent")
                                    absent++;

                                reportsGrid.Rows.Add(
                                    reader["student_id"].ToString(),

                                    reader["student_name"].ToString(),

                                    reader["subject"].ToString(),

                                    reader["section"].ToString(),

                                    Convert.ToDateTime(
                                        reader["attendance_date"])
                                        .ToString("MM/dd/yyyy"),

                                    reader["time_in"].ToString(),

                                    recordStatus
                                );
                            }
                        }
                    }
                }

                lblTotal.Text = total.ToString();
                lblPresent.Text = present.ToString();
                lblLate.Text = late.ToString();
                lblAbsent.Text = absent.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading report:\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    

    
private void ExportReportToPDF()
        {
            if (reportsGrid.Rows.Count == 0)
            {
                MessageBox.Show("There are no attendance records to export.");
                return;
            }

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PDF Files (*.pdf)|*.pdf";
                saveDialog.FileName = "Attendance_Report.pdf";

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (System.IO.FileStream stream =
                        new System.IO.FileStream(
                            saveDialog.FileName,
                            System.IO.FileMode.Create))
                    {
                        PdfDocument document = new PdfDocument(
                            iTextSharp.text.PageSize.A4);

                        iTextSharp.text.pdf.PdfWriter.GetInstance(
                            document, stream);

                        document.Open();

                        document.Add(new PdfParagraph(
                            "Attendance Report",
                            new PdfFont(
                                PdfFont.FontFamily.HELVETICA,
                                18,
                                PdfFont.BOLD)));

                        document.Add(new PdfParagraph(
                            "Total Records: " + lblTotal.Text +
                            " | Present: " + lblPresent.Text +
                            " | Late: " + lblLate.Text +
                            " | Absent: " + lblAbsent.Text));

                        document.Add(new PdfParagraph(" "));

                        PdfTable table = new PdfTable(
                            reportsGrid.Columns.Count);

                        foreach (DataGridViewColumn column in reportsGrid.Columns)
                            table.AddCell(column.HeaderText);

                        foreach (DataGridViewRow row in reportsGrid.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                                table.AddCell(cell.Value?.ToString() ?? "");
                        }

                        table.WidthPercentage = 100;
                        document.Add(table);
                        document.Close();
                    }

                    MessageBox.Show("PDF exported successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("PDF export failed:\n" + ex.Message);
                }
            }
        }



        private void PrintDocument_PrintPage(
    object sender,
    PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;

            using (System.Drawing.Font titleFont =
                new System.Drawing.Font(
                    "Arial", 16, FontStyle.Bold))
            using (System.Drawing.Font headerFont =
                new System.Drawing.Font(
                    "Arial", 9, FontStyle.Bold))
            using (System.Drawing.Font bodyFont =
                new System.Drawing.Font(
                    "Arial", 8))
            {
                float x = e.MarginBounds.Left;
                float y = e.MarginBounds.Top;

                graphics.DrawString(
                    "Attendance Report",
                    titleFont,
                    Brushes.Black,
                    x,
                    y);

                y += 35;

                graphics.DrawString(
                    "Total: " + lblTotal.Text +
                    " | Present: " + lblPresent.Text +
                    " | Late: " + lblLate.Text +
                    " | Absent: " + lblAbsent.Text,
                    bodyFont,
                    Brushes.Black,
                    x,
                    y);

                y += 30;

                float[] widths = { 65, 100, 150, 75, 75, 65, 60 };
                float rowHeight = 25;
                float currentX = x;

                for (int i = 0; i < reportsGrid.Columns.Count; i++)
                {
                    graphics.DrawRectangle(
                        Pens.Black, currentX, y, widths[i], rowHeight);

                    graphics.DrawString(
                        reportsGrid.Columns[i].HeaderText,
                        headerFont,
                        Brushes.Black,
                        new RectangleF(
                            currentX + 3, y + 3, widths[i] - 6, rowHeight - 6));

                    currentX += widths[i];
                }

                y += rowHeight;

                while (printRowIndex < reportsGrid.Rows.Count)
                {
                    if (y + rowHeight > e.MarginBounds.Bottom)
                    {
                        e.HasMorePages = true;
                        return;
                    }

                    DataGridViewRow row =
                        reportsGrid.Rows[printRowIndex];

                    currentX = x;

                    for (int i = 0; i < reportsGrid.Columns.Count; i++)
                    {
                        string value =
                            row.Cells[i].Value?.ToString() ?? "";

                        graphics.DrawRectangle(
                            Pens.Black, currentX, y, widths[i], rowHeight);

                        graphics.DrawString(
                            value,
                            bodyFont,
                            Brushes.Black,
                            new RectangleF(
                                currentX + 3, y + 3,
                                widths[i] - 6, rowHeight - 6));

                        currentX += widths[i];
                    }

                    y += rowHeight;
                    printRowIndex++;
                }

                e.HasMorePages = false;
                printRowIndex = 0;
            }
        }

    }
}

        

