
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class StudentQRCodeForm : Form
    {
        private readonly int userId;
        private PictureBox picQR;
        private Label lblStudentInfo;

        public StudentQRCodeForm(int userId)
        {
            this.userId = userId;
            BuildForm();
            LoadStudentQRCode();
        }

        private void BuildForm()
        {
            Text = "My QR Code";
            Size = new Size(650, 700);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(5, 17, 45);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10);

            Label title = new Label
            {
                Text = "MY STUDENT QR CODE",
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(40, 30),
                Size = new Size(550, 45),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(title);

            Label subtitle = new Label
            {
                Text = "Present this QR code when your attendance is scanned.",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(174, 199, 239),
                Location = new Point(35, 85),
                Size = new Size(565, 30),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(subtitle);

            lblStudentInfo = new Label
            {
                Text = "Loading student information...",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(35, 135),
                Size = new Size(565, 55),
                TextAlign = ContentAlignment.MiddleCenter
            };
            Controls.Add(lblStudentInfo);

            picQR = new PictureBox
            {
                Location = new Point(175, 210),
                Size = new Size(280, 280),
                BackColor = Color.White,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            Controls.Add(picQR);

            Button btnSave = new Button
            {
                Text = "Save QR Code",
                Location = new Point(175, 520),
                Size = new Size(135, 42),
                BackColor = Color.FromArgb(39, 99, 244),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += SaveQRCode;
            Controls.Add(btnSave);

            Button btnClose = new Button
            {
                Text = "Close",
                Location = new Point(325, 520),
                Size = new Size(130, 42),
                BackColor = Color.FromArgb(40, 55, 85),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();
            Controls.Add(btnClose);
        }

        private void LoadStudentQRCode()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT s.student_id,
                               s.first_name,
                               s.last_name,
                               s.section
                        FROM students s
                        INNER JOIN users u ON s.user_id = u.id
                        WHERE u.id = @userId
                        LIMIT 1";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                lblStudentInfo.Text =
                                    "No student profile is linked to this account.";
                                return;
                            }

                            string studentId = reader["student_id"].ToString();
                            string fullName = reader["first_name"].ToString()
                                + " " + reader["last_name"].ToString();
                            string section = reader["section"].ToString();

                            lblStudentInfo.Text =
                                fullName + "\n" + studentId + " | " + section;

                            using (QRCodeGenerator generator =
                                new QRCodeGenerator())
                            using (QRCodeData data = generator.CreateQrCode(
                                studentId, QRCodeGenerator.ECCLevel.Q))
                            using (QRCode qrCode = new QRCode(data))
                            {
                                picQR.Image = qrCode.GetGraphic(10);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load your QR code:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SaveQRCode(object sender, EventArgs e)
        {
            if (picQR.Image == null)
            {
                MessageBox.Show("No QR code is available to save.");
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "PNG Image|*.png";
                dialog.FileName = "My_QR_Code.png";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    picQR.Image.Save(
                        dialog.FileName,
                        System.Drawing.Imaging.ImageFormat.Png);

                    MessageBox.Show("QR code saved successfully.");
                }
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            picQR?.Image?.Dispose();
            picQR.Image = null;
            base.OnFormClosed(e);
        }
    }
}
