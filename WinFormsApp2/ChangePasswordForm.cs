
using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class ChangePasswordForm : Form
    {
        private readonly int userId;

        public ChangePasswordForm(int userId)
        {
            InitializeComponent();
            this.userId = userId;

            txtCurrentPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar = true;

            btnChangePassword.Click += btnChangePassword_Click;
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(currentPassword) ||
                string.IsNullOrWhiteSpace(newPassword) ||
                string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please fill in all password fields.");
                return;
            }

            if (newPassword.Length < 8)
            {
                MessageBox.Show("New password must be at least 8 characters.");
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New passwords do not match.");
                return;
            }

            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string checkQuery =
                        "SELECT COUNT(*) FROM users WHERE id = @id AND password = @password";

                    using (var checkCommand = new MySqlCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@id", userId);
                        checkCommand.Parameters.AddWithValue("@password", currentPassword);

                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count == 0)
                        {
                            MessageBox.Show("Current password is incorrect.");
                            return;
                        }
                    }

                    string updateQuery =
                        "UPDATE users SET password = @newPassword WHERE id = @id";

                    using (var updateCommand = new MySqlCommand(updateQuery, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@newPassword", newPassword);
                        updateCommand.Parameters.AddWithValue("@id", userId);

                        updateCommand.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Password changed successfully!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error changing password: " + ex.Message);
            }
        }
    }
}
