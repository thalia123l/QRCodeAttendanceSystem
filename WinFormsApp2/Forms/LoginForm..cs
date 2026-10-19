using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class LoginForm : Form
    {
        private Panel leftPanel;
        private Panel loginPanel;

        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblMessage;

        private TextBox txtUsername;
        private TextBox txtPassword;

        private Button btnLogin;
        private Button btnShowPassword;

        public LoginForm()
        {
            InitializeComponent();

            BuildLoginInterface();
        }


        private void BuildLoginInterface()
        {
            // FORM
            Text = "QR Attendance System";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1050, 650);
            MinimumSize = new Size(1050, 650);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            BackColor = Color.FromArgb(5, 18, 42);
            Font = new Font("Segoe UI", 10);

            // LEFT BRAND PANEL
            leftPanel = new Panel
            {
                BackColor = Color.FromArgb(10, 35, 75),
                Location = new Point(0, 0),
                Size = new Size(465, 650)
            };
            Controls.Add(leftPanel);

            // BCP LOGO
            PictureBox logoPicture = new PictureBox
            {
                Size = new Size(120, 130),
                Location = new Point(172, 45),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            string logoPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Images",
                "bestlink.png"
            );

            if (System.IO.File.Exists(logoPath))
            {
                using (Image img = Image.FromFile(logoPath))
                {
                    logoPicture.Image = new Bitmap(img);
                }
            }

            leftPanel.Controls.Add(logoPicture);

            // COLLEGE NAME
            Label lblCollege = new Label
            {
                Text = "BESTLINK COLLEGE\nOF THE PHILIPPINES",
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(30, 180),
                Size = new Size(405, 75)
            };
            leftPanel.Controls.Add(lblCollege);

            // BLUE ACCENT LINE
            Panel accentLine = new Panel
            {
                BackColor = Color.FromArgb(40, 150, 255),
                Location = new Point(175, 270),
                Size = new Size(115, 4)
            };
            leftPanel.Controls.Add(accentLine);

            // SYSTEM NAME
            Label lblSystemTitle = new Label
            {
                Text = "QR ATTENDANCE",
                Font = new Font("Segoe UI", 25, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 239, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 295),
                Size = new Size(425, 50)
            };
            leftPanel.Controls.Add(lblSystemTitle);

            // SYSTEM SUBTITLE
            Label lblSystemSubtitle = new Label
            {
                Text = "CLASSROOM ATTENDANCE MANAGEMENT SYSTEM",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(135, 190, 245),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(20, 345),
                Size = new Size(425, 30)
            };
            leftPanel.Controls.Add(lblSystemSubtitle);

            // DESCRIPTION
            Label lblDescription = new Label
            {
                Text = "A smarter way to manage classroom attendance.\n\n" +
                       "Fast. Simple. Organized.",
                Font = new Font("Segoe UI", 11),
                ForeColor = Color.FromArgb(195, 215, 240),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(40, 405),
                Size = new Size(385, 85)
            };
            leftPanel.Controls.Add(lblDescription);

            // VERSION
            Label lblVersion = new Label
            {
                Text = "QR Attendance System  •  v1.0",
                Font = new Font("Segoe UI", 8),
                ForeColor = Color.FromArgb(125, 160, 200),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(30, 590),
                Size = new Size(405, 25)
            };
            leftPanel.Controls.Add(lblVersion);

            // RIGHT LOGIN PANEL
            loginPanel = new Panel
            {
                BackColor = Color.FromArgb(8, 24, 50),
                Location = new Point(465, 0),
                Size = new Size(585, 650)
            };
            Controls.Add(loginPanel);

            // LOGIN CARD
            Panel card = new Panel
            {
                BackColor = Color.FromArgb(13, 34, 67),
                Location = new Point(35, 35),
                Size = new Size(515, 575)
            };
            loginPanel.Controls.Add(card);

            // WELCOME TITLE
            lblTitle = new Label
            {
                Text = "Welcome Back!",
                Font = new Font("Segoe UI", 27, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(30, 48),
                Size = new Size(455, 55)
            };
            card.Controls.Add(lblTitle);

            // SUBTITLE
            lblSubtitle = new Label
            {
                Text = "Sign in to continue to your dashboard",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(155, 190, 230),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(25, 103),
                Size = new Size(465, 30)
            };
            card.Controls.Add(lblSubtitle);

            // ACCENT
            Panel titleLine = new Panel
            {
                BackColor = Color.FromArgb(40, 145, 255),
                Location = new Point(205, 145),
                Size = new Size(105, 4)
            };
            card.Controls.Add(titleLine);

            // USERNAME LABEL
            lblUsername = CreateLabel("USERNAME", new Point(48, 180));
            lblUsername.ForeColor = Color.FromArgb(220, 235, 255);
            lblUsername.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            card.Controls.Add(lblUsername);

            // USERNAME FIELD
            Panel usernameBox = new Panel
            {
                BackColor = Color.FromArgb(23, 49, 86),
                Location = new Point(45, 211),
                Size = new Size(425, 48)
            };
            card.Controls.Add(usernameBox);

            txtUsername = new TextBox
            {
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(23, 49, 86),
                BorderStyle = BorderStyle.None,
                Location = new Point(14, 13),
                Size = new Size(395, 25),
                PlaceholderText = "Enter your username"
            };
            usernameBox.Controls.Add(txtUsername);

            // PASSWORD LABEL
            lblPassword = CreateLabel("PASSWORD", new Point(48, 280));
            lblPassword.ForeColor = Color.FromArgb(220, 235, 255);
            lblPassword.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            card.Controls.Add(lblPassword);

            // PASSWORD FIELD
            Panel passwordBox = new Panel
            {
                BackColor = Color.FromArgb(23, 49, 86),
                Location = new Point(45, 311),
                Size = new Size(425, 48)
            };
            card.Controls.Add(passwordBox);

            txtPassword = new TextBox
            {
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(23, 49, 86),
                BorderStyle = BorderStyle.None,
                Location = new Point(14, 13),
                Size = new Size(325, 25),
                UseSystemPasswordChar = true,
                PlaceholderText = "Enter your password"
            };
            passwordBox.Controls.Add(txtPassword);

            // SHOW / HIDE PASSWORD
            btnShowPassword = new Button
            {
                Text = "SHOW",
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 180, 255),
                BackColor = Color.FromArgb(23, 49, 86),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(345, 8),
                Size = new Size(70, 32),
                Cursor = Cursors.Hand
            };
            btnShowPassword.FlatAppearance.BorderSize = 0;
            btnShowPassword.Click += BtnShowPassword_Click;
            passwordBox.Controls.Add(btnShowPassword);

            // LOGIN BUTTON
            btnLogin = new Button
            {
                Text = "SIGN IN   →",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(30, 115, 235),
                FlatStyle = FlatStyle.Flat,
                Location = new Point(45, 390),
                Size = new Size(425, 55),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(45, 140, 255);
            btnLogin.Click += BtnLogin_Click;
            card.Controls.Add(btnLogin);

            // LOGIN MESSAGE
            lblMessage = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(255, 115, 125),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(35, 455),
                Size = new Size(445, 30)
            };
            card.Controls.Add(lblMessage);

            // SECURITY FOOTER
            Label lblFooter = new Label
            {
                Text = "Secure Classroom Attendance Management",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(130, 170, 215),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(30, 515),
                Size = new Size(455, 25)
            };
            card.Controls.Add(lblFooter);

            // ENTER KEY LOGS IN
            AcceptButton = btnLogin;
        }


        private Label CreateLabel(
            string text,
            Point location)
        {
            Label label = new Label();

            label.Text = text;

            label.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold
            );

            label.ForeColor =
                Color.FromArgb(220, 225, 235);

            label.Location =
                location;

            label.Size =
                new Size(200, 25);

            return label;
        }

        private void BtnShowPassword_Click(
            object sender,
            EventArgs e)
        {
            txtPassword.UseSystemPasswordChar =
                !txtPassword.UseSystemPasswordChar;

            btnShowPassword.Text =
                txtPassword.UseSystemPasswordChar
                ? "Show"
                : "Hide";
        }

        private void BtnLogin_Click(
            object sender,
            EventArgs e)
        {
            string username =
                txtUsername.Text.Trim();

            string password =
                txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username))
            {
                lblMessage.Text =
                    "Please enter your username.";

                txtUsername.Focus();

                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text =
                    "Please enter your password.";

                txtPassword.Focus();

                return;
            }

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();


                    string query =
                        "SELECT id, username, role FROM users " +
                        "WHERE username = @username " +
                        "AND password = @password " +
                        "LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = Convert.ToInt32(reader["id"]);
                                string role = reader["role"].ToString().Trim();

                                Form dashboard = null;

                                if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                                    dashboard = new AdminDashboard();
                                else if (role.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
                                    dashboard = new TeacherDashboard(userId);
                                else if (role.Equals("Student", StringComparison.OrdinalIgnoreCase))
                                    dashboard = new StudentDashboard(userId);

                                reader.Close();

                                if (dashboard != null)
                                {
                                    this.Hide();
                                    dashboard.FormClosed += (s, args) => this.Close();
                                    dashboard.Show();
                                }
                                else
                                {
                                    lblMessage.Text = "Unknown user role.";
                                }
                            }
                            else
                            {
                                lblMessage.Text = "Invalid username or password.";
                                txtPassword.Clear();
                                txtPassword.Focus();
                            }
                        }
                    }
                }
            }



            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to connect to the database.\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}