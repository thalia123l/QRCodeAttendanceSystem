using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class AttendanceForm : Form
    {
        private DataGridView attendanceGrid;

        private TextBox txtSearch;
        private DateTimePicker dtpDate;
        private ComboBox cmbStatus;

        private Button btnSearch;
        private Button btnRefresh;

        public AttendanceForm()
        {
            InitializeComponent();
            BuildUI();
            LoadAttendance();
        }

        private void BuildUI()
        {
            this.Text = "Attendance Management";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;

            // TITLE
            Label lblTitle = new Label();
            lblTitle.Text = "Attendance Management";
            lblTitle.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "View and manage classroom attendance records";
            lblSubtitle.Font = new Font("Segoe UI", 10);
            lblSubtitle.ForeColor = Color.FromArgb(160, 170, 190);
            lblSubtitle.Location = new Point(32, 65);
            lblSubtitle.AutoSize = true;
            this.Controls.Add(lblSubtitle);

            // SEARCH LABEL
            Label lblSearch = new Label();
            lblSearch.Text = "Search";
            lblSearch.ForeColor = Color.White;
            lblSearch.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSearch.Location = new Point(30, 110);
            lblSearch.AutoSize = true;
            this.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(30, 135);
            txtSearch.Size = new Size(250, 30);
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.BackColor = Color.FromArgb(30, 39, 58);
            txtSearch.ForeColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(txtSearch);

            // DATE
            Label lblDate = new Label();
            lblDate.Text = "Date";
            lblDate.ForeColor = Color.White;
            lblDate.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblDate.Location = new Point(300, 110);
            lblDate.AutoSize = true;
            this.Controls.Add(lblDate);

            dtpDate = new DateTimePicker();
            dtpDate.Location = new Point(300, 135);
            dtpDate.Size = new Size(180, 30);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Value = DateTime.Today;
            this.Controls.Add(dtpDate);

            // STATUS
            Label lblStatus = new Label();
            lblStatus.Text = "Status";
            lblStatus.ForeColor = Color.White;
            lblStatus.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblStatus.Location = new Point(500, 110);
            lblStatus.AutoSize = true;
            this.Controls.Add(lblStatus);

            cmbStatus = new ComboBox();
            cmbStatus.Location = new Point(500, 135);
            cmbStatus.Size = new Size(150, 30);
            cmbStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStatus.Items.Add("All");
            cmbStatus.Items.Add("Present");
            cmbStatus.Items.Add("Late");
            cmbStatus.Items.Add("Absent");
            cmbStatus.SelectedIndex = 0;
            this.Controls.Add(cmbStatus);

            // SEARCH BUTTON
            btnSearch = new Button();
            btnSearch.Text = "Search";
            btnSearch.Location = new Point(670, 133);
            btnSearch.Size = new Size(100, 34);
            btnSearch.BackColor = Color.FromArgb(45, 125, 210);
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.FlatAppearance.BorderSize = 0;
            btnSearch.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnSearch.Cursor = Cursors.Hand;
            btnSearch.Click += (sender, e) => LoadAttendance();
            this.Controls.Add(btnSearch);

            // REFRESH BUTTON
            btnRefresh = new Button();
            btnRefresh.Text = "Refresh";
            btnRefresh.Location = new Point(785, 133);
            btnRefresh.Size = new Size(100, 34);
            btnRefresh.BackColor = Color.FromArgb(45, 55, 75);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnRefresh.Cursor = Cursors.Hand;

            btnRefresh.Click += (sender, e) =>
            {
                txtSearch.Clear();
                dtpDate.Value = DateTime.Today;
                cmbStatus.SelectedIndex = 0;
                LoadAttendance();
            };

            this.Controls.Add(btnRefresh);

            // DATAGRIDVIEW
            attendanceGrid = new DataGridView();
            attendanceGrid.Location = new Point(30, 232);
            attendanceGrid.Size = new Size(1100, 450);

            attendanceGrid.BackgroundColor = Color.FromArgb(24, 32, 48);
            attendanceGrid.BorderStyle = BorderStyle.None;
            attendanceGrid.AllowUserToAddRows = false;
            attendanceGrid.AllowUserToDeleteRows = false;
            attendanceGrid.ReadOnly = true;
            attendanceGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            attendanceGrid.MultiSelect = false;
            attendanceGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            attendanceGrid.EnableHeadersVisualStyles = false;
            attendanceGrid.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(35, 48, 70);
            attendanceGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            attendanceGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            attendanceGrid.DefaultCellStyle.BackColor =
                Color.FromArgb(24, 32, 48);
            attendanceGrid.DefaultCellStyle.ForeColor = Color.White;
            attendanceGrid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(45, 80, 120);
            attendanceGrid.DefaultCellStyle.SelectionForeColor = Color.White;

            attendanceGrid.RowTemplate.Height = 35;

            attendanceGrid.Columns.Add("student_id", "Student ID");
            attendanceGrid.Columns.Add("student_name", "Student Name");
            attendanceGrid.Columns.Add("subject", "Subject");
            attendanceGrid.Columns.Add("section", "Section");
            attendanceGrid.Columns.Add("attendance_date", "Date");
            attendanceGrid.Columns.Add("time_in", "Time In");
            attendanceGrid.Columns.Add("status", "Status");

            this.Controls.Add(attendanceGrid);
        }

        private void LoadAttendance()
        {
            attendanceGrid.Rows.Clear();

            string search = txtSearch.Text.Trim();
            string status = cmbStatus.SelectedItem?.ToString() ?? "All";

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            st.student_id,
                            CONCAT(st.first_name, ' ', st.last_name) AS student_name,
                            CONCAT(sub.subject_code, ' - ', sub.subject_name) AS subject,
                            st.section,
                            a.attendance_date,
                            TIME_FORMAT(a.time_in, '%h:%i %p') AS time_in,
                            a.status
                        FROM attendance a
                        INNER JOIN students st
                            ON a.student_id = st.id
                        INNER JOIN subjects sub
                            ON a.subject_id = sub.id
                        WHERE a.attendance_date = @date
                        AND (
                            st.student_id LIKE @search
                            OR st.first_name LIKE @search
                            OR st.last_name LIKE @search
                            OR st.section LIKE @search
                            OR sub.subject_code LIKE @search
                            OR sub.subject_name LIKE @search
                        )";

                    if (status != "All")
                    {
                        query += " AND a.status = @status";
                    }

                    query += @"
                        ORDER BY
                            CASE
                                WHEN a.status = 'Late' THEN 1
                                WHEN a.status = 'Present' THEN 2
                                WHEN a.status = 'Absent' THEN 3
                                ELSE 4
                            END,
                            a.time_in ASC";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@date",
                            dtpDate.Value.Date);

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
                                attendanceGrid.Rows.Add(
                                    reader["student_id"].ToString(),
                                    reader["student_name"].ToString(),
                                    reader["subject"].ToString(),
                                    reader["section"].ToString(),
                                    Convert.ToDateTime(
                                        reader["attendance_date"])
                                        .ToString("MM/dd/yyyy"),
                                    reader["time_in"].ToString(),
                                    reader["status"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading attendance:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}