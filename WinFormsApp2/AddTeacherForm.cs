using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class AddTeacherForm : Form
    {
        private TextBox txtTeacherId;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private TextBox txtPassword;

        private Button btnSave;
        private Button btnCancel;

        public AddTeacherForm()
        {
            InitializeComponent();
            BuildForm();
        }

        private void BuildForm()
        {
            this.Text = "Add Teacher";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(500, 400);
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;

            Label lblTitle = new Label();

            lblTitle.Text = "Add New Teacher";
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

            CreateLabel(
                "Password",
                250,
                225
            );

            txtPassword = CreateTextBox(250, 250);
            txtPassword.UseSystemPasswordChar = true;
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

            btnCancel.FlatAppearance.BorderSize = 0;

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

            btnSave.Text = "Save Teacher";
            btnSave.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            btnSave.ForeColor = Color.White;
            btnSave.BackColor =
                Color.FromArgb(55, 115, 220);

            btnSave.FlatStyle =
                FlatStyle.Flat;

            btnSave.FlatAppearance.BorderSize = 0;

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

        private void BtnCancel_Click(
            object sender,
            EventArgs e)
        {
            this.DialogResult =
                DialogResult.Cancel;

            this.Close();
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTeacherId.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(
                    "Please complete the Teacher ID, First Name, Last Name, and Temporary Password.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    using (MySqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            long userId;

                            // Create the teacher's login account.
                            string userQuery =
                                "INSERT INTO users (username, password, role) " +
                                "VALUES (@username, @password, 'Teacher')";

                            using (MySqlCommand userCommand =
                                   new MySqlCommand(userQuery, connection, transaction))
                            {
                                userCommand.Parameters.AddWithValue(
                                    "@username", txtTeacherId.Text.Trim());

                                userCommand.Parameters.AddWithValue(
                                    "@password", txtPassword.Text);

                                userCommand.ExecuteNonQuery();
                                userId = userCommand.LastInsertedId;
                            }

                            // Save the teacher and link it to the login account.
                            string teacherQuery =
                                "INSERT INTO teachers " +
                                "(teacher_id, first_name, last_name, email, status, user_id) " +
                                "VALUES " +
                                "(@teacher_id, @first_name, @last_name, @email, 'Active', @user_id)";

                            using (MySqlCommand teacherCommand =
                                   new MySqlCommand(teacherQuery, connection, transaction))
                            {
                                teacherCommand.Parameters.AddWithValue(
                                    "@teacher_id", txtTeacherId.Text.Trim());

                                teacherCommand.Parameters.AddWithValue(
                                    "@first_name", txtFirstName.Text.Trim());

                                teacherCommand.Parameters.AddWithValue(
                                    "@last_name", txtLastName.Text.Trim());

                                teacherCommand.Parameters.AddWithValue(
                                    "@email",
                                    string.IsNullOrWhiteSpace(txtEmail.Text)
                                        ? DBNull.Value
                                        : (object)txtEmail.Text.Trim());

                                teacherCommand.Parameters.AddWithValue("@user_id", userId);

                                teacherCommand.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

                MessageBox.Show(
                    "Teacher added successfully!\n\n" +
                    "Username: " + txtTeacherId.Text.Trim() +
                    "\nTemporary Password: " + txtPassword.Text,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    MessageBox.Show(
                        "The Teacher ID or username already exists. Please use a different one.",
                        "Duplicate Entry",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show(
                        "Unable to save teacher.\n\n" + ex.Message,
                        "Database Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "An unexpected error occurred.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

    }
}