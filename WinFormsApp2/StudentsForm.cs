using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class StudentsForm : Form
    {
        private DataGridView studentsGrid;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnRefresh;
        private Button btnEdit;
        private Button btnDelete;

        private Button btnSendEmail;

        public StudentsForm()
        {
            InitializeComponent();
            BuildStudentsForm();
            LoadStudents();
        }

        private void BuildStudentsForm()
        {
            this.Text = "QR Attendance System - Students";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1100, 650);
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;

            // ==============================
            // TITLE
            // ==============================

            Label lblTitle = new Label();

            lblTitle.Text = "Students Management";

            lblTitle.Font = new Font(
                "Segoe UI",
                22,
                FontStyle.Bold
            );

            lblTitle.ForeColor = Color.White;

            lblTitle.Location = new Point(35, 30);
            lblTitle.Size = new Size(500, 45);

            this.Controls.Add(lblTitle);

            // ==============================
            // SEARCH
            // ==============================

            txtSearch = new TextBox();

            txtSearch.PlaceholderText =
                "Search student...";

            txtSearch.Font = new Font(
                "Segoe UI",
                10
            );

            txtSearch.Location =
                new Point(35, 95);

            txtSearch.Size =
                new Size(300, 35);

            txtSearch.BackColor =
                Color.FromArgb(30, 38, 55);

            txtSearch.ForeColor =
                Color.White;

            txtSearch.BorderStyle =
                BorderStyle.FixedSingle;

            txtSearch.TextChanged +=
                TxtSearch_TextChanged;

            this.Controls.Add(txtSearch);

            // ==============================
            // ADD BUTTON
            // ==============================

            btnAdd = new Button();

            btnAdd.Text = "+  Add Student";

            btnAdd.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnAdd.ForeColor = Color.White;

            btnAdd.BackColor =
                Color.FromArgb(55, 115, 220);

            btnAdd.FlatStyle =
                FlatStyle.Flat;

            btnAdd.FlatAppearance.BorderSize =
                0;

            btnAdd.Location =
                new Point(750, 90);

            btnAdd.Size =
                new Size(140, 40);

            btnAdd.Cursor =
                Cursors.Hand;

            btnAdd.Click +=
                BtnAdd_Click;

            this.Controls.Add(btnAdd);

            // ==============================
            // REFRESH BUTTON
            // ==============================

            btnRefresh = new Button();

            btnRefresh.Text = "↻  Refresh";

            btnRefresh.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnRefresh.ForeColor =
                Color.White;

            btnRefresh.BackColor =
                Color.FromArgb(42, 51, 70);

            btnRefresh.FlatStyle =
                FlatStyle.Flat;

            btnRefresh.FlatAppearance.BorderSize =
                0;

            btnRefresh.Location =
                new Point(900, 90);

            btnRefresh.Size =
                new Size(120, 40);

            btnRefresh.Cursor =
                Cursors.Hand;

            btnRefresh.Click +=
                BtnRefresh_Click;

            this.Controls.Add(btnRefresh);

            // ==============================
            // DELETE BUTTON
            // ==============================

            btnDelete = new Button();

            btnDelete.Text = "✕  Delete";

            btnDelete.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnDelete.ForeColor = Color.White;

            btnDelete.BackColor =
                Color.FromArgb(150, 55, 65);

            btnDelete.FlatStyle =
                FlatStyle.Flat;

            btnDelete.FlatAppearance.BorderSize =
                0;

            btnDelete.Location =
                new Point(475, 90);

            btnDelete.Size =
                new Size(120, 40);

            btnDelete.Cursor =
                Cursors.Hand;

            btnDelete.Click +=
                BtnDelete_Click;

                

            this.Controls.Add(btnDelete);


            // ==============================
            // EDIT BUTTON
            // ==============================

            btnEdit = new Button();

            btnEdit.Text = "✎  Edit";

            btnEdit.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnEdit.ForeColor =
                Color.White;

            btnEdit.BackColor =
                Color.FromArgb(42, 51, 70);

            btnEdit.FlatStyle =
                FlatStyle.Flat;

            btnEdit.FlatAppearance.BorderSize =
                0;

            btnEdit.Location =
                new Point(610, 90);

            btnEdit.Size =
                new Size(120, 40);

            btnEdit.Cursor =
                Cursors.Hand;

            btnEdit.Click +=
                BtnEdit_Click;

            this.Controls.Add(btnEdit);


            // ==============================
            // STUDENTS TABLE
            // ==============================

            studentsGrid =
                new DataGridView();

            studentsGrid.Location =
                new Point(35, 155);

            studentsGrid.Size =
                new Size(985, 420);

            studentsGrid.BackgroundColor =
                Color.FromArgb(30, 38, 55);

            studentsGrid.BorderStyle =
                BorderStyle.None;

            studentsGrid.GridColor =
                Color.FromArgb(50, 60, 80);

            studentsGrid.RowHeadersVisible =
                false;

            studentsGrid.AllowUserToAddRows =
                false;

            studentsGrid.AllowUserToResizeRows =
                false;

            studentsGrid.ReadOnly = true;

            studentsGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            studentsGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            studentsGrid.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(42, 51, 70),

                    ForeColor = Color.White,

                    Font = new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold
                    )
                };

            studentsGrid.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(30, 38, 55),

                    ForeColor =
                        Color.FromArgb(220, 225, 235),

                    Font = new Font(
                        "Segoe UI",
                        9
                    ),

                    SelectionBackColor =
                        Color.FromArgb(55, 115, 220),

                    SelectionForeColor =
                        Color.White
                };

            studentsGrid.Columns.Add(
                "student_id",
                "Student ID"
            );

            studentsGrid.Columns.Add(
                "name",
                "Name"
            );

            studentsGrid.Columns.Add(
                "section",
                "Section"
            );

            studentsGrid.Columns.Add(
                "email",
                "Email"
            );

            studentsGrid.Columns.Add(
                "status",
                "Status"
            );


            // ==============================
            // SEND EMAIL BUTTON
            // ==============================

            btnSendEmail = new Button();

            btnSendEmail.Text = "✉ Send Email";
            btnSendEmail.Font = new Font(
                "Segoe UI", 10, FontStyle.Bold);

            btnSendEmail.ForeColor = Color.White;
            btnSendEmail.BackColor = Color.FromArgb(20, 150, 120);
            btnSendEmail.FlatStyle = FlatStyle.Flat;
            btnSendEmail.FlatAppearance.BorderSize = 0;

            btnSendEmail.Location = new Point(750, 40);
            btnSendEmail.Size = new Size(140, 40);
            btnSendEmail.Cursor = Cursors.Hand;

            btnSendEmail.Click += BtnSendEmail_Click;

            this.Controls.Add(btnSendEmail);


            this.Controls.Add(studentsGrid);
        }

        private void LoadStudents()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "SELECT student_id, " +
                        "CONCAT(first_name, ' ', last_name) AS name, " +
                        "section, email, status " +
                        "FROM students " +
                        "ORDER BY last_name ASC";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            studentsGrid.Rows.Clear();

                            while (reader.Read())
                            {
                                studentsGrid.Rows.Add(
                                    reader["student_id"].ToString(),
                                    reader["name"].ToString(),
                                    reader["section"].ToString(),
                                    reader["email"].ToString(),
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
                    "Unable to load students.\n\n" +
                    ex.Message,
                    "Students Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void TxtSearch_TextChanged(
            object sender,
            EventArgs e)
        {
            string search =
                txtSearch.Text.Trim();

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "SELECT student_id, " +
                        "CONCAT(first_name, ' ', last_name) AS name, " +
                        "section, email, status " +
                        "FROM students " +
                        "WHERE student_id LIKE @search " +
                        "OR first_name LIKE @search " +
                        "OR last_name LIKE @search " +
                        "OR section LIKE @search " +
                        "ORDER BY last_name ASC";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "@search",
                            "%" + search + "%"
                        );

                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            studentsGrid.Rows.Clear();

                            while (reader.Read())
                            {
                                studentsGrid.Rows.Add(
                                    reader["student_id"].ToString(),
                                    reader["name"].ToString(),
                                    reader["section"].ToString(),
                                    reader["email"].ToString(),
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
                    ex.Message,
                    "Search Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnRefresh_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Clear();
            LoadStudents();
        }

        private void BtnAdd_Click(
    object sender,
    EventArgs e)
        {
            AddStudentForm addStudentForm =
                new AddStudentForm();

            if (addStudentForm.ShowDialog() ==
                DialogResult.OK)
            {
                LoadStudents();
            }
        }

        private void BtnEdit_Click(
    object sender,
    EventArgs e)
        {
            if (studentsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a student first.",
                    "No Student Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string studentId =
                studentsGrid.SelectedRows[0]
                .Cells["student_id"]
                .Value
                .ToString();

            EditStudentForm editStudentForm =
                new EditStudentForm(studentId);

            if (editStudentForm.ShowDialog() ==
                DialogResult.OK)
            {
                LoadStudents();
            }
        }

        private void BtnDelete_Click(
    object sender,
    EventArgs e)
        {
            if (studentsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a student first.",
                    "No Student Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }



            string studentId =
                studentsGrid.SelectedRows[0]
                .Cells["student_id"]
                .Value
                .ToString();

            string studentName =
                studentsGrid.SelectedRows[0]
                .Cells["name"]
                .Value
                .ToString();

            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to delete\n\n" +
                    studentName +
                    " (" +
                    studentId +
                    ")?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

            if (result != DialogResult.Yes)
            {
                return;
            }

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "DELETE FROM students " +
                        "WHERE student_id = @student_id";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "@student_id",
                            studentId
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Student deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete student.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }


        private void BtnSendEmail_Click(object sender, EventArgs e)
        {
            if (studentsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a student first.",
                    "No Student Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            string studentName = studentsGrid.SelectedRows[0]
                .Cells["name"].Value?.ToString() ?? "";

            string studentEmail = studentsGrid.SelectedRows[0]
                .Cells["email"].Value?.ToString() ?? "";

            if (string.IsNullOrWhiteSpace(studentEmail))
            {
                MessageBox.Show(
                    "This student does not have an email address registered.",
                    "Missing Email",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            using (EmailComposeForm emailForm =
                new EmailComposeForm(studentName, studentEmail))
            {
                emailForm.ShowDialog(this);
            }
        }

    }

}

