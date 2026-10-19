using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class EditSubjectForm : Form
    {
        private TextBox txtSubjectCode;
        private TextBox txtSubjectName;
        private TextBox txtDescription;

        private Button btnSave;
        private Button btnCancel;

        private string originalSubjectCode;

        public EditSubjectForm(string subjectCode)
        {
            InitializeComponent();

            originalSubjectCode = subjectCode;

            BuildUI();
            LoadSubject();
        }

        private void BuildUI()
        {
            this.Text = "Edit Subject";
            this.Size = new Size(520, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // =========================
            // TITLE
            // =========================

            Label lblTitle = new Label();
            lblTitle.Text = "Edit Subject";
            lblTitle.Font = new Font(
                "Segoe UI",
                20,
                FontStyle.Bold
            );
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(35, 25);
            lblTitle.AutoSize = true;

            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text = "Update the subject information below.";
            lblSubtitle.Font = new Font(
                "Segoe UI",
                10
            );
            lblSubtitle.ForeColor = Color.LightGray;
            lblSubtitle.Location = new Point(38, 62);
            lblSubtitle.AutoSize = true;

            this.Controls.Add(lblSubtitle);

            // =========================
            // SUBJECT CODE
            // =========================

            Label lblCode = CreateLabel(
                "Subject Code",
                new Point(40, 110)
            );

            this.Controls.Add(lblCode);

            txtSubjectCode = CreateTextBox(
                new Point(40, 135),
                new Size(420, 35)
            );

            this.Controls.Add(txtSubjectCode);

            // =========================
            // SUBJECT NAME
            // =========================

            Label lblName = CreateLabel(
                "Subject Name",
                new Point(40, 185)
            );

            this.Controls.Add(lblName);

            txtSubjectName = CreateTextBox(
                new Point(40, 210),
                new Size(420, 35)
            );

            this.Controls.Add(txtSubjectName);

            // =========================
            // DESCRIPTION
            // =========================

            Label lblDescription = CreateLabel(
                "Description",
                new Point(40, 260)
            );

            this.Controls.Add(lblDescription);

            txtDescription = CreateTextBox(
                new Point(40, 285),
                new Size(420, 60)
            );

            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;

            this.Controls.Add(txtDescription);

            // =========================
            // SAVE BUTTON
            // =========================

            btnSave = new Button();

            btnSave.Text = "Save Changes";
            btnSave.Location = new Point(225, 390);
            btnSave.Size = new Size(120, 40);

            btnSave.BackColor = Color.FromArgb(0, 120, 215);
            btnSave.ForeColor = Color.White;

            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;

            btnSave.Font = new Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            );

            btnSave.Cursor = Cursors.Hand;

            btnSave.Click += BtnSave_Click;

            this.Controls.Add(btnSave);

            // =========================
            // CANCEL BUTTON
            // =========================

            btnCancel = new Button();

            btnCancel.Text = "Cancel";
            btnCancel.Location = new Point(355, 390);
            btnCancel.Size = new Size(105, 40);

            btnCancel.BackColor = Color.FromArgb(45, 55, 75);
            btnCancel.ForeColor = Color.White;

            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;

            btnCancel.Font = new Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            );

            btnCancel.Cursor = Cursors.Hand;

            btnCancel.Click += (sender, e) =>
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            };

            this.Controls.Add(btnCancel);
        }

        // =========================
        // CREATE LABEL
        // =========================

        private Label CreateLabel(
            string text,
            Point location)
        {
            Label label = new Label();

            label.Text = text;
            label.Location = location;
            label.AutoSize = true;

            label.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            label.ForeColor = Color.White;

            return label;
        }

        // =========================
        // CREATE TEXTBOX
        // =========================

        private TextBox CreateTextBox(
            Point location,
            Size size)
        {
            TextBox textBox = new TextBox();

            textBox.Location = location;
            textBox.Size = size;

            textBox.Font = new Font(
                "Segoe UI",
                10
            );

            textBox.BackColor =
                Color.FromArgb(30, 39, 58);

            textBox.ForeColor = Color.White;

            textBox.BorderStyle =
                BorderStyle.FixedSingle;

            return textBox;
        }

        // =========================
        // LOAD SUBJECT
        // =========================

        private void LoadSubject()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            subject_code,
                            subject_name,
                            description
                        FROM subjects
                        WHERE subject_code = @subject_code
                        LIMIT 1";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@subject_code",
                            originalSubjectCode
                        );

                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtSubjectCode.Text =
                                    reader["subject_code"].ToString();

                                txtSubjectName.Text =
                                    reader["subject_name"].ToString();

                                txtDescription.Text =
                                    reader["description"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading subject:\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // SAVE CHANGES
        // =========================

        private void BtnSave_Click(
            object sender,
            EventArgs e)
        {
            string subjectCode =
                txtSubjectCode.Text.Trim();

            string subjectName =
                txtSubjectName.Text.Trim();

            string description =
                txtDescription.Text.Trim();

            // =========================
            // VALIDATION
            // =========================

            if (string.IsNullOrWhiteSpace(subjectCode))
            {
                MessageBox.Show(
                    "Please enter the subject code.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtSubjectCode.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(subjectName))
            {
                MessageBox.Show(
                    "Please enter the subject name.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtSubjectName.Focus();
                return;
            }

            // =========================
            // UPDATE DATABASE
            // =========================

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        UPDATE subjects
                        SET
                            subject_code = @new_code,
                            subject_name = @subject_name,
                            description = @description
                        WHERE subject_code = @old_code";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@new_code",
                            subjectCode
                        );

                        command.Parameters.AddWithValue(
                            "@subject_name",
                            subjectName
                        );

                        command.Parameters.AddWithValue(
                            "@description",
                            description
                        );

                        command.Parameters.AddWithValue(
                            "@old_code",
                            originalSubjectCode
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Subject updated successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show(
                        "The subject code already exists.\n\n"
                        + "Please use a different subject code.",
                        "Duplicate Subject Code",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );

                    txtSubjectCode.Focus();
                }
                else
                {
                    MessageBox.Show(
                        "Database error:\n\n"
                        + ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error updating subject:\n\n"
                    + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}