
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class StudentAttendanceForm : Form
    {
        private readonly int userId;
        private DataGridView dgvAttendance;

        public StudentAttendanceForm(int userId)
        {
            this.userId = userId;
            BuildForm();
            LoadAttendance();
        }

        private void BuildForm()
        {
            Text = "My Attendance";
            Size = new Size(1000, 650);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(5, 17, 45);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10);

            Label title = new Label
            {
                Text = "My Attendance Records",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 25),
                Size = new Size(650, 50)
            };
            Controls.Add(title);

            Label subtitle = new Label
            {
                Text = "Review your recorded attendance.",
                ForeColor = Color.FromArgb(174, 199, 239),
                Location = new Point(35, 80),
                Size = new Size(500, 30)
            };
            Controls.Add(subtitle);

            dgvAttendance = new DataGridView
            {
                Location = new Point(30, 130),
                Size = new Size(920, 400),
                BackgroundColor = Color.FromArgb(14, 36, 82),
                ForeColor = Color.Black,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                RowHeadersVisible = false
            };

            Controls.Add(dgvAttendance);

            Button btnRefresh = new Button
            {
                Text = "Refresh",
                Location = new Point(30, 550),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(39, 99, 244),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += (s, e) => LoadAttendance();
            Controls.Add(btnRefresh);

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(165, 550),
                Size = new Size(120, 40),
                BackColor = Color.FromArgb(40, 55, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);
        }

        private void LoadAttendance()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            a.attendance_date AS 'Date',
                            a.time_in AS 'Time In',
                            a.status AS 'Status',
                            a.subject_id AS 'Subject ID',
                            a.schedule_id AS 'Schedule ID'
                        FROM attendance a
                        INNER JOIN students s
                            ON a.student_id = s.id
                        WHERE s.user_id = @userId
                        ORDER BY a.attendance_date DESC, a.time_in DESC";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (MySqlDataAdapter adapter =
                            new MySqlDataAdapter(command))
                        {
                            System.Data.DataTable table =
                                new System.Data.DataTable();

                            adapter.Fill(table);
                            dgvAttendance.DataSource = table;
                        }
                    }
                }

                if (dgvAttendance.Rows.Count == 0)
                {
                    MessageBox.Show(
                        "No attendance records were found for your account.",
                        "My Attendance",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load attendance records:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
