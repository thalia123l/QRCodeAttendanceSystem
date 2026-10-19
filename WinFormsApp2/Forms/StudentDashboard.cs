
using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class StudentDashboard : Form
    {
        private readonly int userId;

        private string studentName = "Student";
        private string studentId = "";
        private string studentEmail = "";

        private readonly Color background = Color.FromArgb(5, 17, 45);
        private readonly Color sidebarColor = Color.FromArgb(4, 14, 38);
        private readonly Color textMuted = Color.FromArgb(174, 199, 239);

        public StudentDashboard(int userId)
        {
            this.userId = userId;
            LoadStudentInfo();
            BuildDashboard();
        }

        private void LoadStudentInfo()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT CONCAT(s.first_name, ' ', s.last_name),
                               s.student_id,
                               s.email
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
                            if (reader.Read())
                            {
                                studentName = reader.IsDBNull(0)
                                    ? "Student" : reader.GetString(0);

                                studentId = reader.IsDBNull(1)
                                    ? "" : reader.GetString(1);

                                studentEmail = reader.IsDBNull(2)
                                    ? "" : reader.GetString(2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load student information:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void BuildDashboard()
        {
            Text = "Student Dashboard - " + studentName;
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(1450, 900);
            MinimumSize = new Size(1150, 750);
            BackColor = background;
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 10);
            DoubleBuffered = true;

            // SIDEBAR
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 280,
                BackColor = sidebarColor
            };
            Controls.Add(sidebar);

            Label logoMark = new Label
            {
                Text = "◆",
                Font = new Font("Segoe UI", 34, FontStyle.Bold),
                ForeColor = Color.FromArgb(61, 119, 255),
                Location = new Point(25, 27),
                Size = new Size(65, 65),
                TextAlign = ContentAlignment.MiddleCenter
            };
            sidebar.Controls.Add(logoMark);

            Label logoTitle = new Label
            {
                Text = "QR Attendance",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(95, 35),
                Size = new Size(175, 30)
            };
            sidebar.Controls.Add(logoTitle);

            Label logoSubtitle = new Label
            {
                Text = "SYSTEM",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(70, 139, 255),
                Location = new Point(98, 66),
                Size = new Size(120, 22)
            };
            sidebar.Controls.Add(logoSubtitle);

            AddNavigation(sidebar, "⌂    Dashboard", 150,
                true, () => { });

            AddNavigation(sidebar, "◉    My Profile", 225,
                false, ShowProfile);

            AddNavigation(sidebar, "▥    View Attendance", 300,
                false, ViewAttendance);

            AddNavigation(sidebar, "▦    My QR Code", 375,
                false, ShowQRCode);

            AddNavigation(sidebar, "⇥    Logout", 450,
                false, Logout);

            Label quoteMark = new Label
            {
                Text = "“",
                Font = new Font("Georgia", 38, FontStyle.Bold),
                ForeColor = Color.FromArgb(104, 158, 255),
                Location = new Point(25, 610),
                Size = new Size(45, 45),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };
            sidebar.Controls.Add(quoteMark);

            Label quote = new Label
            {
                Text = "Every day is a\nnew opportunity\nto learn.",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = textMuted,
                Location = new Point(30, 660),
                Size = new Size(220, 85),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };
            sidebar.Controls.Add(quote);

            // MAIN CONTENT
            Panel main = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = background,
                AutoScroll = true,
                AutoScrollMinSize = new Size(1250, 850)
            };
            Controls.Add(main);
            main.BringToFront();

            Label welcome = new Label
            {
                Text = "Welcome, " + studentName,
                Font = new Font("Segoe UI", 29, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(35, 35),
                Size = new Size(660, 55),
                AutoEllipsis = true
            };
            main.Controls.Add(welcome);

            Label subtitle = new Label
            {
                Text = "View your profile and keep track of your attendance.",
                Font = new Font("Segoe UI", 12),
                ForeColor = textMuted,
                Location = new Point(40, 100),
                Size = new Size(650, 32)
            };
            main.Controls.Add(subtitle);

            // PROFILE CARD
            Panel profile = new Panel
            {
                Location = new Point(750, 30),
                Size = new Size(350, 155),
                BackColor = Color.FromArgb(14, 36, 82),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            main.Controls.Add(profile);

            Label avatar = new Label
            {
                Text = "●",
                Font = new Font("Segoe UI", 37, FontStyle.Bold),
                ForeColor = Color.FromArgb(91, 136, 255),
                Location = new Point(15, 17),
                Size = new Size(65, 65),
                TextAlign = ContentAlignment.MiddleCenter
            };
            profile.Controls.Add(avatar);

            Label name = new Label
            {
                Text = studentName,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(90, 20),
                Size = new Size(240, 30),
                AutoEllipsis = true
            };
            profile.Controls.Add(name);

            Label role = new Label
            {
                Text = "Student",
                Font = new Font("Segoe UI", 10),
                ForeColor = textMuted,
                Location = new Point(92, 50),
                Size = new Size(200, 25)
            };
            profile.Controls.Add(role);

            Label id = new Label
            {
                Text = "◆   " + (string.IsNullOrWhiteSpace(studentId)
                    ? "Student ID unavailable" : studentId),
                ForeColor = textMuted,
                Location = new Point(20, 92),
                Size = new Size(310, 23),
                AutoEllipsis = true
            };
            profile.Controls.Add(id);

            Label email = new Label
            {
                Text = "✉   " + (string.IsNullOrWhiteSpace(studentEmail)
                    ? "Email not provided" : studentEmail),
                ForeColor = textMuted,
                Location = new Point(20, 121),
                Size = new Size(310, 23),
                AutoEllipsis = true
            };
            profile.Controls.Add(email);

            // DASHBOARD CARDS
            AddActionCard(main, "My Profile",
                "View your student information\nand account details.",
                "●", Color.FromArgb(11, 54, 123),
                Color.FromArgb(29, 126, 255),
                35, 225, ShowProfile);

            AddActionCard(main, "View Attendance",
                "Check your recorded attendance\nand attendance history.",
                "✓", Color.FromArgb(5, 75, 83),
                Color.FromArgb(0, 199, 169),
                440, 225, ViewAttendance);

            AddActionCard(main, "My QR Code",
                "Access your QR code for\nclassroom identification.",
                "▦", Color.FromArgb(48, 26, 112),
                Color.FromArgb(132, 58, 255),
                35, 455, ShowQRCode);

            AddActionCard(main, "Logout",
                "Safely end your current\nstudent session.",
                "⇥", Color.FromArgb(94, 24, 64),
                Color.FromArgb(255, 51, 105),
                440, 455, Logout);



            AddActionCard(main, "Change Password",
                "Update your password to\nkeep your account secure.",
                "🔒", Color.FromArgb(75, 45, 20),
                Color.FromArgb(255, 166, 64),
                845, 455, () => btnChangePassword_Click(this, EventArgs.Empty));



            Label footer = new Label
            {
                Text = "QR ATTENDANCE SYSTEM     •     STUDENT PORTAL",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(104, 145, 210),
                Location = new Point(35, 720),
                Size = new Size(500, 30)
            };
            main.Controls.Add(footer);
        }

        private void AddNavigation(
            Panel parent, string text, int y,
            bool selected, Action action)
        {
            Button button = new Button
            {
                Text = text,
                Location = new Point(14, y),
                Size = new Size(252, 58),
                BackColor = selected
                    ? Color.FromArgb(39, 99, 244)
                    : sidebarColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(17, 0, 0, 0),
                Cursor = Cursors.Hand
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(29, 69, 133);

            button.Click += (s, e) => action();
            parent.Controls.Add(button);
        }

        private void AddActionCard(
            Panel parent,
            string title,
            string description,
            string symbol,
            Color cardColor,
            Color accentColor,
            int x, int y,
            Action action)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(385, 205),
                BackColor = cardColor,
                Cursor = Cursors.Hand
            };

            Label icon = new Label
            {
                Text = symbol,
                Font = new Font("Segoe UI", 31, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = accentColor,
                Location = new Point(25, 20),
                Size = new Size(78, 78),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label heading = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(25, 110),
                Size = new Size(335, 32),
                AutoEllipsis = true
            };

            Label detail = new Label
            {
                Text = description,
                Font = new Font("Segoe UI", 11),
                ForeColor = textMuted,
                Location = new Point(25, 148),
                Size = new Size(330, 48)
            };

            Label arrow = new Label
            {
                Text = "➜",
                Font = new Font("Segoe UI", 19, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = accentColor,
                Location = new Point(320, 24),
                Size = new Size(48, 48),
                TextAlign = ContentAlignment.MiddleCenter,
                Cursor = Cursors.Hand
            };

            card.Controls.Add(icon);
            card.Controls.Add(heading);
            card.Controls.Add(detail);
            card.Controls.Add(arrow);

            void OpenCard(object sender, EventArgs e) => action();

            card.Click += OpenCard;
            icon.Click += OpenCard;
            heading.Click += OpenCard;
            detail.Click += OpenCard;
            arrow.Click += OpenCard;

            parent.Controls.Add(card);
        }

        private void ShowProfile()
        {
            MessageBox.Show(
                "Student Name: " + studentName +
                "\nStudent ID: " + studentId +
                "\nEmail: " + studentEmail,
                "My Profile",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }


        private void ViewAttendance()
        {
            using (StudentAttendanceForm form =
                new StudentAttendanceForm(userId))
            {
                form.ShowDialog(this);
            }
        }


        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            using (ChangePasswordForm form = new ChangePasswordForm(userId))
            {
                form.ShowDialog(this);
            }
        }




        private void ShowQRCode()
        {
            using (StudentQRCodeForm form = new StudentQRCodeForm(userId))
            {
                form.ShowDialog(this);
            }
        }


        private void Logout()
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Hide();

                using (LoginForm login = new LoginForm())
                {
                    login.ShowDialog();
                }

                Close();
            }
        }
    }
}
