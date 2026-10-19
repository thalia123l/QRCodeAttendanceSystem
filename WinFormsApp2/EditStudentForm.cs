using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class EditStudentForm : Form
    {
        private TextBox txtStudentId;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtSection;
        private TextBox txtEmail;

        private Button btnSave;
        private Button btnCancel;

        private string originalStudentId;

        public EditStudentForm(string studentId)
        {
            InitializeComponent();

            originalStudentId = studentId;

            BuildForm();
            LoadStudent();
        }

        private void BuildForm()
        {
            this.Text = "Edit Student";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 470);
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;

            Label lblTitle = new Label();

            lblTitle.Text = "Edit Student";
            lblTitle.Font = new Font(
                "Segoe UI",
                20,
                FontStyle.Bold
            );

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(35, 25);
            lblTitle.Size = new Size(400, 40);

            this.Controls.Add(lblTitle);

            CreateLabel("Student ID", 35, 85);

            txtStudentId = CreateTextBox(35, 110);

            CreateLabel("First Name", 35, 155);

            txtFirstName = CreateTextBox(35, 180);

            CreateLabel("Last Name", 250, 155);

            txtLastName = CreateTextBox(250, 180);

            CreateLabel("Section", 35, 225);

            txtSection = CreateTextBox(35, 250);

            CreateLabel("Email", 250, 225);

            txtEmail = CreateTextBox(250, 250);

            btnCancel = new Button();

            btnCancel.Text = "Cancel";
            btnCancel.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnCancel.ForeColor = Color.White;
            btnCancel.BackColor =
                Color.FromArgb(42, 51, 70);

            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;

            btnCancel.Location = new Point(35, 320);
            btnCancel.Size = new Size(180, 45);
            btnCancel.Cursor = Cursors.Hand;

            btnCancel.Click += BtnCancel_Click;

            this.Controls.Add(btnCancel);

            btnSave = new Button();

            btnSave.Text = "Save Changes";
            btnSave.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnSave.ForeColor = Color.White;
            btnSave.BackColor =
                Color.FromArgb(55, 115, 220);

            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;

            btnSave.Location = new Point(250, 320);
            btnSave.Size = new Size(180, 45);
            btnSave.Cursor = Cursors.Hand;

            btnSave.Click += BtnSave_Click;

            this.Controls.Add(btnSave);
        }

        private Label CreateLabel(
            string text,
            int x,
            int y)
        {
            Label label = new Label();

            label.Text = text;

            label.Font = new Font(
                "Segoe UI",
                9,
                FontStyle.Bold
            );

            label.ForeColor =
                Color.FromArgb(180, 190, 210);

            label.Location =
                new Point(x, y);

            label.Size =
                new Size(180, 25);

            this.Controls.Add(label);

            return label;
        }

        private TextBox CreateTextBox(
            int x,
            int y)
        {
            TextBox textBox = new TextBox();

            textBox.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            textBox.BackColor =
                Color.FromArgb(30, 38, 55);

            textBox.ForeColor = Color.White;

            textBox.BorderStyle =
                BorderStyle.FixedSingle;

            textBox.Location =
                new Point(x, y);

            textBox.Size =
                new Size(180, 32);

            this.Controls.Add(textBox);

            return textBox;
        }

        private void LoadStudent()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "SELECT student_id, first_name, last_name, " +
                        "section, email " +
                        "FROM students " +
                        "WHERE student_id = @student_id " +
                        "LIMIT 1";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@student_id",
                            originalStudentId
                        );

                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtStudentId.Text =
                                    reader["student_id"].ToString();

                                txtFirstName.Text =
                                    reader["first_name"].ToString();

                                txtLastName.Text =
                                    reader["last_name"].ToString();

                                txtSection.Text =
                                    reader["section"].ToString();

                                if (reader["email"] != DBNull.Value)
                                {
                                    txtEmail.Text =
                                        reader["email"].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load student.\n\n" +
                    ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void BtnCancel_Click(
            object sender,
            EventArgs e)
        {
            this.DialogResult =
                DialogResult.Cancel;

            this.Close();
        }

        private void BtnSave_Click(
            object sender,
            EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(
                txtStudentId.Text))
            {
                MessageBox.Show(
                    "Please enter the Student ID.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtStudentId.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtFirstName.Text))
            {
                MessageBox.Show(
                    "Please enter the first name.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtFirstName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtLastName.Text))
            {
                MessageBox.Show(
                    "Please enter the last name.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtLastName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtSection.Text))
            {
                MessageBox.Show(
                    "Please enter the section.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtSection.Focus();
                return;
            }

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "UPDATE students SET " +
                        "student_id = @new_student_id, " +
                        "first_name = @first_name, " +
                        "last_name = @last_name, " +
                        "section = @section, " +
                        "email = @email " +
                        "WHERE student_id = @old_student_id";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@new_student_id",
                            txtStudentId.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "@first_name",
                            txtFirstName.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "@last_name",
                            txtLastName.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "@section",
                            txtSection.Text.Trim()
                        );

                        if (string.IsNullOrWhiteSpace(
                            txtEmail.Text))
                        {
                            command.Parameters.AddWithValue(
                                "@email",
                                DBNull.Value
                            );
                        }
                        else
                        {
                            command.Parameters.AddWithValue(
                                "@email",
                                txtEmail.Text.Trim()
                            );
                        }

                        command.Parameters.AddWithValue(
                            "@old_student_id",
                            originalStudentId
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Student updated successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult =
                    DialogResult.OK;

                this.Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show(
                        "The new Student ID already exists.",
                        "Duplicate Student ID",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Unable to update student.\n\n" +
                        ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An error occurred.\n\n" +
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}