
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QRAttendanceSystem
{
    public partial class EmailComposeForm : Form
    {
        private TextBox txtRecipient;
        private TextBox txtSubject;
        private TextBox txtMessage;
        private Button btnSend;
        private Label lblStatus;

        public EmailComposeForm(string studentName, string studentEmail)
        {
            InitializeComponent();
            BuildEmailForm(studentName, studentEmail);
        }

        private void BuildEmailForm(string studentName, string studentEmail)
        {
            this.Text = "Send Email to Student";
            this.Size = new Size(600, 550);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.ForeColor = Color.White;
            this.Font = new Font("Segoe UI", 10);

            Label lblTitle = new Label();
            lblTitle.Text = "Compose Email";
            lblTitle.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(25, 20);
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            AddLabel("Student:", 30, 80);
            Label lblStudent = new Label();
            lblStudent.Text = studentName;
            lblStudent.ForeColor = Color.LightGray;
            lblStudent.Location = new Point(130, 80);
            lblStudent.AutoSize = true;
            this.Controls.Add(lblStudent);

            AddLabel("To:", 30, 120);
            txtRecipient = new TextBox();
            txtRecipient.Text = studentEmail;
            txtRecipient.Location = new Point(130, 115);
            txtRecipient.Size = new Size(420, 30);
            txtRecipient.ReadOnly = true;
            this.Controls.Add(txtRecipient);

            AddLabel("Subject:", 30, 165);
            txtSubject = new TextBox();
            txtSubject.Location = new Point(130, 160);
            txtSubject.Size = new Size(420, 30);
            this.Controls.Add(txtSubject);

            AddLabel("Message:", 30, 210);
            txtMessage = new TextBox();
            txtMessage.Location = new Point(130, 205);
            txtMessage.Size = new Size(420, 200);
            txtMessage.Multiline = true;
            txtMessage.ScrollBars = ScrollBars.Vertical;
            txtMessage.AcceptsReturn = true;
            txtMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left
                | AnchorStyles.Right;
            this.Controls.Add(txtMessage);

            btnSend = new Button();
            btnSend.Text = "Send Email";
            btnSend.Location = new Point(410, 430);
            btnSend.Size = new Size(140, 40);
            btnSend.BackColor = Color.FromArgb(20, 150, 120);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Click += BtnSend_Click;
            this.Controls.Add(btnSend);

            lblStatus = new Label();
            lblStatus.Location = new Point(30, 435);
            lblStatus.Size = new Size(360, 35);
            lblStatus.ForeColor = Color.LightGray;
            this.Controls.Add(lblStatus);
        }

        private void AddLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Location = new Point(x, y);
            label.AutoSize = true;
            label.ForeColor = Color.White;
            this.Controls.Add(label);
        }

        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSubject.Text) ||
                string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show(
                    "Please enter both the subject and message.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            btnSend.Enabled = false;
            btnSend.Text = "Sending...";
            lblStatus.Text = "Sending email...";

            try
            {
                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(
                    txtRecipient.Text,
                    txtSubject.Text.Trim(),
                    txtMessage.Text.Trim());

                lblStatus.Text = "Email sent successfully!";
                MessageBox.Show(
                    "Email sent successfully to " + txtRecipient.Text,
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Failed to send email.";
                MessageBox.Show(
                    "Unable to send email.\n\n" + ex.Message,
                    "Email Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                if (!this.IsDisposed)
                {
                    btnSend.Enabled = true;
                    btnSend.Text = "Send Email";
                }
            }
        }
    }
}
