
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QRAttendanceSystem
{
    public partial class EmailTestForm : Form
    {
        private TextBox txtRecipient;
        private TextBox txtSubject;
        private TextBox txtBody;
        private Button btnSend;
        private Label lblStatus;

        public EmailTestForm()
        {
            InitializeComponent();
            BuildForm();
        }

        private void BuildForm()
        {
            Text = "Test Email - QR Attendance System";
            Size = new Size(600, 500);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(18, 24, 38);

            Label lblTitle = new Label
            {
                Text = "Send Test Email",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(30, 20),
                AutoSize = true
            };
            Controls.Add(lblTitle);

            AddLabel("Recipient Email:", 30, 85);
            txtRecipient = AddTextBox(30, 110, 500);

            AddLabel("Subject:", 30, 155);
            txtSubject = AddTextBox(30, 180, 500);
            txtSubject.Text = "QR Attendance System Test";

            AddLabel("Message:", 30, 225);
            txtBody = AddTextBox(30, 250, 500);
            txtBody.Multiline = true;
            txtBody.Height = 80;
            txtBody.Text = "This is a test email from the QR Attendance System.";

            btnSend = new Button
            {
                Text = "Send Test Email",
                Location = new Point(30, 355),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSend.Click += BtnSend_Click;
            Controls.Add(btnSend);

            lblStatus = new Label
            {
                Text = "Ready to send.",
                Location = new Point(30, 410),
                AutoSize = true,
                ForeColor = Color.LightGray
            };
            Controls.Add(lblStatus);
        }

        private void AddLabel(string text, int x, int y)
        {
            Controls.Add(new Label
            {
                Text = text,
                Location = new Point(x, y),
                AutoSize = true,
                ForeColor = Color.White
            });
        }

        private TextBox AddTextBox(int x, int y, int width)
        {
            TextBox box = new TextBox
            {
                Location = new Point(x, y),
                Width = width
            };
            Controls.Add(box);
            return box;
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRecipient.Text) ||
                string.IsNullOrWhiteSpace(txtSubject.Text) ||
                string.IsNullOrWhiteSpace(txtBody.Text))
            {
                MessageBox.Show(
                    "Please complete all fields.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnSend.Enabled = false;
                lblStatus.Text = "Sending email...";

                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(
                    txtRecipient.Text.Trim(),
                    txtSubject.Text.Trim(),
                    txtBody.Text.Trim());

                lblStatus.Text = "Email sent successfully!";
                MessageBox.Show(
                    "Test email sent successfully!",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Email sending failed.";
                MessageBox.Show(
                    "Unable to send email:\n" + ex.Message,
                    "Email Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }
    }
}
