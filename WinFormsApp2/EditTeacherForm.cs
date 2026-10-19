using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class EditTeacherForm : Form
    {
        private TextBox txtTeacherId;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;

        private Button btnSave;
        private Button btnCancel;

        private string originalTeacherId;

        public EditTeacherForm(string teacherId)
        {
            InitializeComponent();

            originalTeacherId = teacherId;

            BuildForm();
            LoadTeacher();
        }

        private void BuildForm()
        {
            this.Text = "Edit Teacher";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 400);
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            Label lblTitle = new Label();

            lblTitle.Text = "Edit Teacher";
            lblTitle.Font = new Font(
                "Segoe UI",
                20,
                FontStyle.Bold
            );

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(35, 25);
            lblTitle.Size = new Size(400, 40);

            this.Controls.Add(lblTitle);

            CreateLabel(
                "Teacher ID",
                35,
                85
            );

            txtTeacherId =
                CreateTextBox(35, 110);

            CreateLabel(
                "First Name",
                35,
                155
            );

            txtFirstName =
                CreateTextBox(35, 180);

            CreateLabel(
                "Last Name",
                250,
                155
            );

            txtLastName =
                CreateTextBox(250, 180);

            CreateLabel(
                "Email",
                35,
                225
            );

            txtEmail =
                CreateTextBox(35, 250);

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

            btnCancel.FlatStyle =
                FlatStyle.Flat;

            btnCancel.FlatAppearance.BorderSize =
                0;

            btnCancel.Location =
                new Point(35, 305);

            btnCancel.Size =
                new Size(180, 45);

            btnCancel.Cursor =
                Cursors.Hand;

            btnCancel.Click +=
                BtnCancel_Click;

            this.Controls.Add(btnCancel);

            btnSave = new Button();

            btnSave.Text = "Save Changes";
            btnSave.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnSave.ForeColor =
                Color.White;

            btnSave.BackColor =
                Color.FromArgb(55, 115, 220);

            btnSave.FlatStyle =
                FlatStyle.Flat;

            btnSave.FlatAppearance.BorderSize =
                0;

            btnSave.Location =
                new Point(250, 305);

            btnSave.Size =
                new Size(180, 45);

            btnSave.Cursor =
                Cursors.Hand;

            btnSave.Click +=
                BtnSave_Click;

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
                Color.FromArgb(
                    180,
                    190,
                    210
                );

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
                Color.FromArgb(
                    30,
                    38,
                    55
                );

            textBox.ForeColor =
                Color.White;

            textBox.BorderStyle =
                BorderStyle.FixedSingle;

            textBox.Location =
                new Point(x, y);

            textBox.Size =
                new Size(180, 32);

            this.Controls.Add(textBox);

            return textBox;
        }

        private void LoadTeacher()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "SELECT teacher_id, first_name, " +
                        "last_name, email " +
                        "FROM teachers " +
                        "WHERE teacher_id = @teacher_id " +
                        "LIMIT 1";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "@teacher_id",
                            originalTeacherId
                        );

                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtTeacherId.Text =
                                    reader["teacher_id"]
                                    .ToString();

                                txtFirstName.Text =
                                    reader["first_name"]
                                    .ToString();

                                txtLastName.Text =
                                    reader["last_name"]
                                    .ToString();

                                if (reader["email"] !=
                                    DBNull.Value)
                                {
                                    txtEmail.Text =
                                        reader["email"]
                                        .ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load teacher.\n\n" +
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
                txtTeacherId.Text))
            {
                MessageBox.Show(
                    "Please enter the Teacher ID.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtTeacherId.Focus();
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

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query =
                        "UPDATE teachers SET " +
                        "teacher_id = @new_teacher_id, " +
                        "first_name = @first_name, " +
                        "last_name = @last_name, " +
                        "email = @email " +
                        "WHERE teacher_id = @old_teacher_id";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "@new_teacher_id",
                            txtTeacherId.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "@first_name",
                            txtFirstName.Text.Trim()
                        );

                        command.Parameters.AddWithValue(
                            "@last_name",
                            txtLastName.Text.Trim()
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
                            "@old_teacher_id",
                            originalTeacherId
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Teacher updated successfully!",
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
                        "The new Teacher ID already exists.",
                        "Duplicate Teacher ID",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                else
                {
                    MessageBox.Show(
                        "Unable to update teacher.\n\n" +
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