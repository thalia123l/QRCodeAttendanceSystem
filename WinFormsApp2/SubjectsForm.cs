using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class SubjectsForm : Form
    {
        private DataGridView subjectsGrid;
        private TextBox txtSearch;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnDelete;
        private Button btnRefresh;

        public SubjectsForm()
        {
            InitializeComponent();
            BuildSubjectsUI();
            LoadSubjects();
        }

        private void BuildSubjectsUI()
        {
            this.Text = "Subjects Management";
            this.Size = new Size(1050, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.ForeColor = Color.White;

            // =========================
            // TITLE
            // =========================

            Label lblTitle = new Label();
            lblTitle.Text = "Subjects Management";
            lblTitle.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(35, 25);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Manage subjects and course information";
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
            txtSearch.PlaceholderText = "Search subject...";
            txtSearch.TextChanged += (sender, e) =>
            {
                LoadSubjects(txtSearch.Text.Trim());
            };

            this.Controls.Add(txtSearch);

            // =========================
            // ADD BUTTON
            // =========================

            btnAdd = CreateButton(
                "＋ Add Subject",
                new Point(355, 103),
                Color.FromArgb(0, 120, 215)
            );

            btnAdd.Click += BtnAdd_Click;
            this.Controls.Add(btnAdd);

            // =========================
            // EDIT BUTTON
            // =========================

            btnEdit = CreateButton(
                "✎ Edit",
                new Point(490, 103),
                Color.FromArgb(45, 55, 75)
            );

            btnEdit.Click += BtnEdit_Click;
            this.Controls.Add(btnEdit);

            // =========================
            // DELETE BUTTON
            // =========================

            btnDelete = CreateButton(
                "✕ Delete",
                new Point(595, 103),
                Color.FromArgb(150, 45, 45)
            );

            btnDelete.Click += BtnDelete_Click;
            this.Controls.Add(btnDelete);

            // =========================
            // REFRESH BUTTON
            // =========================

            btnRefresh = CreateButton(
                "↻ Refresh",
                new Point(700, 103),
                Color.FromArgb(45, 55, 75)
            );

            btnRefresh.Click += (sender, e) =>
            {
                txtSearch.Clear();
                LoadSubjects();
            };

            this.Controls.Add(btnRefresh);

            // =========================
            // DATA GRID
            // =========================

            subjectsGrid = new DataGridView();

            subjectsGrid.Location = new Point(35, 160);
            subjectsGrid.Size = new Size(960, 410);

            subjectsGrid.BackgroundColor = Color.FromArgb(24, 32, 48);
            subjectsGrid.BorderStyle = BorderStyle.None;

            subjectsGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            subjectsGrid.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            subjectsGrid.MultiSelect = false;
            subjectsGrid.ReadOnly = true;
            subjectsGrid.AllowUserToAddRows = false;
            subjectsGrid.AllowUserToDeleteRows = false;

            subjectsGrid.RowHeadersVisible = false;

            subjectsGrid.EnableHeadersVisualStyles = false;

            subjectsGrid.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(30, 42, 62);

            subjectsGrid.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            subjectsGrid.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            subjectsGrid.DefaultCellStyle.BackColor =
                Color.FromArgb(24, 32, 48);

            subjectsGrid.DefaultCellStyle.ForeColor =
                Color.White;

            subjectsGrid.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(45, 85, 125);

            subjectsGrid.DefaultCellStyle.SelectionForeColor =
                Color.White;

            subjectsGrid.RowTemplate.Height = 35;

            subjectsGrid.Columns.Add("subject_code", "Subject Code");
            subjectsGrid.Columns.Add("subject_name", "Subject Name");
            subjectsGrid.Columns.Add("description", "Description");
            subjectsGrid.Columns.Add("status", "Status");

            this.Controls.Add(subjectsGrid);
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
        // LOAD SUBJECTS
        // =========================

        private void LoadSubjects(string search = "")
        {
            try
            {
                subjectsGrid.Rows.Clear();

                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            subject_code,
                            subject_name,
                            description,
                            status
                        FROM subjects
                        WHERE
                            subject_code LIKE @search
                            OR subject_name LIKE @search
                            OR description LIKE @search
                        ORDER BY subject_code ASC";

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
                                subjectsGrid.Rows.Add(
                                    reader["subject_code"].ToString(),
                                    reader["subject_name"].ToString(),
                                    reader["description"].ToString(),
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
                    "Error loading subjects:\n\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // ADD SUBJECT
        // =========================

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            AddSubjectForm form = new AddSubjectForm();

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSubjects(txtSearch.Text.Trim());
            }
        }

        // =========================
        // EDIT SUBJECT
        // =========================

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (subjectsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a subject first.",
                    "No Subject Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string subjectCode =
                subjectsGrid.SelectedRows[0]
                .Cells["subject_code"]
                .Value
                .ToString();

            EditSubjectForm form =
                new EditSubjectForm(subjectCode);

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadSubjects(txtSearch.Text.Trim());
            }
        }

        // =========================
        // DELETE SUBJECT
        // =========================

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (subjectsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show(
                    "Please select a subject first.",
                    "No Subject Selected",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                return;
            }

            string subjectCode =
                subjectsGrid.SelectedRows[0]
                .Cells["subject_code"]
                .Value
                .ToString();

            string subjectName =
                subjectsGrid.SelectedRows[0]
                .Cells["subject_name"]
                .Value
                .ToString();

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this subject?\n\n"
                + subjectCode + " - " + subjectName,
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
                        DELETE FROM subjects
                        WHERE subject_code = @subject_code";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@subject_code",
                            subjectCode
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Subject deleted successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                LoadSubjects(txtSearch.Text.Trim());
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Unable to delete this subject.\n\n"
                    + "It may already be used in schedules or attendance records.\n\n"
                    + ex.Message,
                    "Delete Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error deleting subject:\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}