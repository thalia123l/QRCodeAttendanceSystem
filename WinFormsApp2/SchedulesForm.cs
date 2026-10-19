using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class SchedulesForm : Form
    {
        private DataGridView schedulesGrid;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public SchedulesForm()
        {
            InitializeComponent();
            BuildSchedulesUI();
            LoadSchedules();
        }

        private void BuildSchedulesUI()
        {
            this.Text = "Schedules Management";
            this.Size = new Size(1150, 680);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.ForeColor = Color.White;

            // =========================
            // TITLE
            // =========================

            Label lblTitle = new Label();
            lblTitle.Text = "Schedules Management";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(35, 25);
            lblTitle.AutoSize = true;

            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Manage class schedules, subjects, teachers, and rooms";
            lblSubtitle.Font = new Font("Segoe UI", 10);
            lblSubtitle.ForeColor = Color.LightGray;
            lblSubtitle.Location = new Point(38, 62);
            lblSubtitle.AutoSize = true;

            this.Controls.Add(lblSubtitle);

            // =========================
            // SEARCH
            // =========================

            txtSearch = new TextBox();
            txtSearch.Location = new Point(35, 105);
            txtSearch.Size = new Size(300, 35);
            txtSearch.Font = new Font("Segoe UI", 10);
            txtSearch.BackColor = Color.FromArgb(30, 39, 58);
            txtSearch.ForeColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.PlaceholderText = "Search schedule...";

            txtSearch.TextChanged += (sender, e) =>
            {
                LoadSchedules(txtSearch.Text.Trim());
            };

            this.Controls.Add(txtSearch);

            // =========================
            // ADD
            // =========================

            btnAdd = CreateButton(
                "＋ Add",
                new Point(355, 103),
                Color.FromArgb(0, 120, 215)
            );

            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            // =========================
            // EDIT
            // =========================

            btnEdit = CreateButton(
                "✎ Edit",
                new Point(465, 103),
                Color.FromArgb(45, 55, 75)
            );

            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            // =========================
            // DELETE
            // =========================

            btnDelete = CreateButton(
                "✕ Delete",
                new Point(570, 103),
                Color.FromArgb(150, 45, 45)
            );

            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // =========================
            // REFRESH
            // =========================

            btnRefresh = CreateButton(
                "↻ Refresh",
                new Point(675, 103),
                Color.FromArgb(45, 55, 75)
            );

            btnRefresh.Click += (sender, e) =>
            {
                txtSearch.Clear();
                LoadSchedules();
            };

            this.Controls.Add(btnRefresh);

            // =========================
            // DATA GRID
            // =========================

            schedulesGrid = new DataGridView();

            schedulesGrid.Location = new Point(35, 160);
            schedulesGrid.Size = new Size(1065, 430);

            schedulesGrid.BackgroundColor =
                Color.FromArgb(24, 32, 48);

            schedulesGrid.BorderStyle =
                BorderStyle.None;

            schedulesGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            schedulesGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            schedulesGrid.MultiSelect = false;
            schedulesGrid.ReadOnly = true;
            schedulesGrid.AllowUserToAddRows = false;
            schedulesGrid.AllowUserToDeleteRows = false;

            schedulesGrid.RowHeadersVisible = false;

            schedulesGrid.EnableHeadersVisualStyles = false;

            schedulesGrid.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 42, 62);

            schedulesGrid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            schedulesGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            schedulesGrid.DefaultCellStyle.BackColor =
                Color.FromArgb(24, 32, 48);

            schedulesGrid.DefaultCellStyle.ForeColor =
                Color.White;

            schedulesGrid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(45, 85, 125);

            schedulesGrid.DefaultCellStyle.SelectionForeColor =
                Color.White;

            schedulesGrid.RowTemplate.Height = 35;

            schedulesGrid.Columns.Add(
                "schedule_id",
                "ID"
            );

            schedulesGrid.Columns.Add(
                "subject",
                "Subject"
            );

            schedulesGrid.Columns.Add(
                "teacher",
                "Teacher"
            );

            schedulesGrid.Columns.Add(
                "section",
                "Section"
            );

            schedulesGrid.Columns.Add(
                "day",
                "Day"
            );

            schedulesGrid.Columns.Add(
                "start_time",
                "Start"
            );

            schedulesGrid.Columns.Add(
                "end_time",
                "End"
            );

            schedulesGrid.Columns.Add(
                "room",
                "Room"
            );

            this.Controls.Add(schedulesGrid);
        }

        // =========================
        // BUTTON CREATOR
        // =========================

        private Button CreateButton(
            string text,
            Point location,
            Color backColor)
        {
            Button button = new Button();

            button.Text = text;
            button.Location = location;
            button.Size = new Size(100, 36);

            button.BackColor = backColor;
            button.ForeColor = Color.White;

            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            button.Font = new Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            );

            button.Cursor = Cursors.Hand;

            return button;
        }

        // =========================
        // LOAD SCHEDULES
        // =========================

        private void LoadSchedules(string search = "")
        {
            try
            {
                schedulesGrid.Rows.Clear();

                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            s.id,
                            CONCAT(
                                sub.subject_code,
                                ' - ',
                                sub.subject_name
                            ) AS subject,
                            CONCAT(
                                t.teacher_id,
                                ' - ',
                                t.first_name,
                                ' ',
                                t.last_name
                            ) AS teacher,
                            s.section,
                            s.day,
                            TIME_FORMAT(
                                s.start_time,
                                '%h:%i %p'
                            ) AS start_time,
                            TIME_FORMAT(
                                s.end_time,
                                '%h:%i %p'
                            ) AS end_time,
                            s.room
                        FROM schedules s
                        INNER JOIN subjects sub
                            ON s.subject_id = sub.id
                        INNER JOIN teachers t
                            ON s.teacher_id = t.id
                        WHERE
                            sub.subject_code LIKE @search
                            OR sub.subject_name LIKE @search
                            OR t.teacher_id LIKE @search
                            OR t.first_name LIKE @search
                            OR t.last_name LIKE @search
                            OR s.section LIKE @search
                            OR s.day LIKE @search
                            OR s.room LIKE @search
                        ORDER BY
                            FIELD(
                                s.day,
                                'Monday',
                                'Tuesday',
                                'Wednesday',
                                'Thursday',
                                'Friday',
                                'Saturday',
                                'Sunday'
                            ),
                            s.start_time";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@search",
                            "%" + search + "%"
                        );

                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                schedulesGrid.Rows.Add(
                                    reader["id"].ToString(),
                                    reader["subject"].ToString(),
                                    reader["teacher"].ToString(),
                                    reader["section"].ToString(),
                                    reader["day"].ToString(),
                                    reader["start_time"].ToString(),
                                    reader["end_time"].ToString(),
                                    reader["room"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading schedules:\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // ADD SCHEDULE
        // =========================

        private void BtnAdd_Click(
            object sender,
            EventArgs e)
        {
            AddScheduleForm form =
                new AddScheduleForm();

            if (form.ShowDialog() ==
                DialogResult.OK)
            {
                LoadSchedules(
                    txtSearch.Text.Trim()
                );
            }
        }

        // =========================
        // EDIT SCHEDULE
        // =========================

        private void BtnEdit_Click(
            object sender,
            EventArgs e)
        {
            if (schedulesGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a schedule first.",
                    "No Schedule Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            int scheduleId = Convert.ToInt32(
                schedulesGrid.SelectedRows[0]
                .Cells["schedule_id"]
                .Value
            );

            EditScheduleForm form =
                new EditScheduleForm(scheduleId);

            if (form.ShowDialog() ==
                DialogResult.OK)
            {
                LoadSchedules(
                    txtSearch.Text.Trim()
                );
            }
        }

        // =========================
        // DELETE SCHEDULE
        // =========================

        private void BtnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (schedulesGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a schedule first.",
                    "No Schedule Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            int scheduleId = Convert.ToInt32(
                schedulesGrid.SelectedRows[0]
                .Cells["schedule_id"]
                .Value
            );

            string subject =
                schedulesGrid.SelectedRows[0]
                .Cells["subject"]
                .Value
                .ToString();

            string teacher =
                schedulesGrid.SelectedRows[0]
                .Cells["teacher"]
                .Value
                .ToString();

            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to delete this schedule?\n\n"
                    + subject + "\n"
                    + teacher,
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        DELETE FROM schedules
                        WHERE id = @id";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "@id",
                            scheduleId
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Schedule deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadSchedules(
                    txtSearch.Text.Trim()
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error deleting schedule:\n\n"
                    + ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}