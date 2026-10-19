using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class TeachersForm : Form
    {
        private DataGridView teachersGrid;
        private TextBox txtSearch;

        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public TeachersForm()
        {
            InitializeComponent();
            BuildTeachersForm();
            LoadTeachers();
        }

        private void BuildTeachersForm()
        {
            this.Text = "QR Attendance System - Teachers";
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

            lblTitle.Text = "Teachers Management";

            lblTitle.Font = new Font(
                "Segoe UI",
                22,
                FontStyle.Bold
            );

            lblTitle.ForeColor = Color.White;

            lblTitle.Location =
                new Point(35, 30);

            lblTitle.Size =
                new Size(500, 45);

            this.Controls.Add(lblTitle);

            // ==============================
            // SEARCH
            // ==============================

            txtSearch = new TextBox();

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
            // DELETE BUTTON
            // ==============================

            btnDelete = new Button();

            btnDelete.Text = "✕  Delete";

            btnDelete.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnDelete.ForeColor =
                Color.White;

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
            // ADD BUTTON
            // ==============================

            btnAdd = new Button();

            btnAdd.Text = "+  Add Teacher";

            btnAdd.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnAdd.ForeColor =
                Color.White;

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
            // TEACHERS TABLE
            // ==============================

            teachersGrid =
                new DataGridView();

            teachersGrid.Location =
                new Point(35, 155);

            teachersGrid.Size =
                new Size(985, 420);

            teachersGrid.BackgroundColor =
                Color.FromArgb(30, 38, 55);

            teachersGrid.BorderStyle =
                BorderStyle.None;

            teachersGrid.GridColor =
                Color.FromArgb(50, 60, 80);

            teachersGrid.RowHeadersVisible =
                false;

            teachersGrid.AllowUserToAddRows =
                false;

            teachersGrid.AllowUserToResizeRows =
                false;

            teachersGrid.ReadOnly = true;

            teachersGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            teachersGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            teachersGrid.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(42, 51, 70),

                    ForeColor =
                        Color.White,

                    Font = new Font(
                        "Segoe UI",
                        9,
                        FontStyle.Bold
                    )
                };

            teachersGrid.DefaultCellStyle =
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

            teachersGrid.Columns.Add(
                "teacher_id",
                "Teacher ID"
            );

            teachersGrid.Columns.Add(
                "name",
                "Name"
            );

            teachersGrid.Columns.Add(
                "email",
                "Email"
            );

            teachersGrid.Columns.Add(
                "status",
                "Status"
            );

            this.Controls.Add(teachersGrid);
        }

        // ==============================
        // LOAD TEACHERS
        // ==============================

        private void LoadTeachers()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "SELECT teacher_id, " +
                        "CONCAT(first_name, ' ', last_name) AS name, " +
                        "email, status " +
                        "FROM teachers " +
                        "ORDER BY last_name ASC";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            teachersGrid.Rows.Clear();

                            while (reader.Read())
                            {
                                teachersGrid.Rows.Add(
                                    reader["teacher_id"].ToString(),
                                    reader["name"].ToString(),
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
                    "Unable to load teachers.\n\n" +
                    ex.Message,
                    "Teachers Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ==============================
        // SEARCH
        // ==============================

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
                        "SELECT teacher_id, " +
                        "CONCAT(first_name, ' ', last_name) AS name, " +
                        "email, status " +
                        "FROM teachers " +
                        "WHERE teacher_id LIKE @search " +
                        "OR first_name LIKE @search " +
                        "OR last_name LIKE @search " +
                        "OR email LIKE @search " +
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
                            teachersGrid.Rows.Clear();

                            while (reader.Read())
                            {
                                teachersGrid.Rows.Add(
                                    reader["teacher_id"].ToString(),
                                    reader["name"].ToString(),
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

        // ==============================
        // ADD
        // ==============================

        private void BtnAdd_Click(
            object sender,
            EventArgs e)
        {
            AddTeacherForm addTeacherForm =
                new AddTeacherForm();

            if (addTeacherForm.ShowDialog() ==
                DialogResult.OK)
            {
                LoadTeachers();
            }
        }

        // ==============================
        // EDIT
        // ==============================

        private void BtnEdit_Click(
            object sender,
            EventArgs e)
        {
            if (teachersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a teacher first.",
                    "No Teacher Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string teacherId =
                teachersGrid.SelectedRows[0]
                .Cells["teacher_id"]
                .Value
                .ToString();

            EditTeacherForm editTeacherForm =
                new EditTeacherForm(teacherId);

            if (editTeacherForm.ShowDialog() ==
                DialogResult.OK)
            {
                LoadTeachers();
            }
        }

        // ==============================
        // DELETE
        // ==============================

        private void BtnDelete_Click(
            object sender,
            EventArgs e)
        {
            if (teachersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a teacher first.",
                    "No Teacher Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string teacherId =
                teachersGrid.SelectedRows[0]
                .Cells["teacher_id"]
                .Value
                .ToString();

            string teacherName =
                teachersGrid.SelectedRows[0]
                .Cells["name"]
                .Value
                .ToString();

            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to delete\n\n" +
                    teacherName +
                    " (" +
                    teacherId +
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
                        "DELETE FROM teachers " +
                        "WHERE teacher_id = @teacher_id";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "@teacher_id",
                            teacherId
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Teacher deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadTeachers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to delete teacher.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // ==============================
        // REFRESH
        // ==============================

        private void BtnRefresh_Click(
            object sender,
            EventArgs e)
        {
            txtSearch.Clear();
            LoadTeachers();
        }
    }
}