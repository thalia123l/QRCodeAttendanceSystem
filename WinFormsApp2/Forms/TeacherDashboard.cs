
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class TeacherDashboard : Form
    {
        private readonly int userId;

        private string teacherName = "Teacher";
        private string teacherId = "";
        private string teacherEmail = "";
        private int teacherDatabaseId = 0;
        private Label lblTotalStudents;
        private Label lblPresentToday;
        private Label lblLateToday;

        private readonly Color background = Color.FromArgb(5, 17, 45);
        private readonly Color sidebarColor = Color.FromArgb(4, 14, 38);
        private readonly Color textMuted = Color.FromArgb(174, 199, 239);

        public TeacherDashboard(int userId)
        {
            this.userId = userId;

            LoadTeacherInfo();
            BuildDashboard();
        }

        private void LoadTeacherInfo()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT t.id,
                                CONCAT(t.first_name, ' ', t.last_name),
                               t.teacher_id,
                               t.email
                        FROM teachers t
                        INNER JOIN users u ON t.user_id = u.id
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
                                teacherDatabaseId = reader.GetInt32(0);

                                teacherName = reader.IsDBNull(1)
                                    ? "Teacher" : reader.GetString(1);

                                teacherId = reader.IsDBNull(2)
                                    ? "" : reader.GetString(2);

                                teacherEmail = reader.IsDBNull(3)
                                    ? "" : reader.GetString(3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load teacher information:\n" +
                    ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BuildDashboard()
        {
            Text = "Teacher Dashboard - " + teacherName;
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

            AddNavigation(sidebar, "▣    Record Attendance", 225,
                false, OpenAttendance);

            AddNavigation(sidebar, "▦    Scan Student QR", 300,
                false, OpenQRScanner);

            AddNavigation(sidebar, "▥    View Reports", 375,
                false, OpenReports);

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
                Text = "Better attendance\nbuilds a brighter\nfuture.",
                Font = new Font("Segoe UI", 12, FontStyle.Italic),
                ForeColor = textMuted,
                Location = new Point(30, 660),
                Size = new Size(220, 85),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };
            sidebar.Controls.Add(quote);

            Panel quoteLine = new Panel
            {
                BackColor = Color.FromArgb(48, 126, 255),
                Location = new Point(30, 755),
                Size = new Size(60, 4),
                Anchor = AnchorStyles.Left | AnchorStyles.Bottom
            };
            sidebar.Controls.Add(quoteLine);

            // MAIN AREA
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
                Text = "Welcome, " + teacherName,
                Font = new Font("Segoe UI", 29, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(35, 35),
                Size = new Size(660, 55),
                AutoEllipsis = true
            };
            main.Controls.Add(welcome);

            Label subtitle = new Label
            {
                Text = "Manage classroom attendance and student records.",
                Font = new Font("Segoe UI", 12),
                ForeColor = textMuted,
                Location = new Point(40, 100),
                Size = new Size(630, 32)
            };
            main.Controls.Add(subtitle);

            // PROFILE CARD
            Panel profile = new Panel
            {
                Location = new Point(850, 30),
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
                Text = teacherName,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(90, 20),
                Size = new Size(240, 30),
                AutoEllipsis = true
            };
            profile.Controls.Add(name);

            Label teacherRole = new Label
            {
                Text = "Teacher",
                Font = new Font("Segoe UI", 10),
                ForeColor = textMuted,
                Location = new Point(92, 50),
                Size = new Size(200, 25)
            };
            profile.Controls.Add(teacherRole);

            Label id = new Label
            {
                Text = "◆   " + (string.IsNullOrWhiteSpace(teacherId)
                    ? "Teacher ID unavailable" : teacherId),
                ForeColor = textMuted,
                Location = new Point(20, 92),
                Size = new Size(310, 23),
                AutoEllipsis = true
            };
            profile.Controls.Add(id);

            Label email = new Label
            {
                Text = "✉   " + (string.IsNullOrWhiteSpace(teacherEmail)
                    ? "Email not provided" : teacherEmail),
                ForeColor = textMuted,
                Location = new Point(20, 121),
                Size = new Size(310, 23),
                AutoEllipsis = true
            };
            profile.Controls.Add(email);

            // ATTENDANCE SUMMARY CARDS
            lblTotalStudents = CreateSummaryCard(
                main, "TOTAL STUDENTS", "0",
                35, 205, Color.FromArgb(29, 126, 255));

            lblPresentToday = CreateSummaryCard(
                main, "PRESENT TODAY", "0",
                440, 205, Color.FromArgb(0, 199, 169));

            lblLateToday = CreateSummaryCard(
                main, "LATE TODAY", "0",
                845, 205, Color.FromArgb(255, 166, 64));

            // FOUR LARGE ACTION CARDS
            AddActionCard(main, "Record Attendance",
                "Mark student attendance\nfor your class.",
                "✓", Color.FromArgb(11, 54, 123),
                Color.FromArgb(29, 126, 255),
                35, 355, OpenAttendance);

            AddActionCard(main, "Scan Student QR",
                "Scan student QR codes\nfor quick attendance.",
                "▦", Color.FromArgb(5, 75, 83),
                Color.FromArgb(0, 199, 169),
                440, 355, OpenQRScanner);

            AddActionCard(main, "View Reports",
                "Review attendance records\nand generate reports.",
                "▥", Color.FromArgb(48, 26, 112),
                Color.FromArgb(132, 58, 255),
                35, 585, OpenReports);

            AddActionCard(main, "Logout",
                "End your current session\nsafely.",
                "⇥", Color.FromArgb(94, 24, 64),
                Color.FromArgb(255, 51, 105),
                440, 585, Logout);


            AddActionCard(main, "Change Password",
                "Update your password to\nkeep your account secure.",
                "🔒", Color.FromArgb(75, 45, 20),
                Color.FromArgb(255, 166, 64),
                845, 585, OpenChangePassword);

            LoadTeacherSummary();
        


        Label footer = new Label
            {
                Text = "QR ATTENDANCE SYSTEM     •     TEACHER PORTAL",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(104, 145, 210),
                Location = new Point(35, 770),
                Size = new Size(500, 30)
            };
            main.Controls.Add(footer);
        }


        private Label CreateSummaryCard(
    Panel parent,
    string title,
    string value,
    int x,
    int y,
    Color accentColor)
        {
            Panel card = new Panel
            {
                Location = new Point(x, y),
                Size = new Size(385, 125),
                BackColor = Color.FromArgb(14, 36, 82)
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = textMuted,
                Location = new Point(20, 18),
                Size = new Size(340, 25)
            };

            Label lblValue = new Label
            {
                Text = value,
                Font = new Font("Segoe UI", 28, FontStyle.Bold),
                ForeColor = accentColor,
                Location = new Point(20, 48),
                Size = new Size(340, 55)
            };

            Panel accent = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(5, 125),
                BackColor = accentColor
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            card.Controls.Add(accent);

            parent.Controls.Add(card);

            return lblValue;
        }

        private void LoadTeacherSummary()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                SELECT COUNT(DISTINCT st.id)
                FROM students st
                INNER JOIN schedules sc
                    ON sc.section = st.section
                WHERE sc.teacher_id = @teacherId
                  AND st.status = 'Active'";

                    using (MySqlCommand command =
                        new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue(
                            "@teacherId", teacherDatabaseId);

                        int totalStudents = Convert.ToInt32(
                            command.ExecuteScalar());

                        lblTotalStudents.Text = totalStudents.ToString();
                        string presentQuery = @"
    SELECT COUNT(DISTINCT a.student_id)
    FROM attendance a
    INNER JOIN schedules sc
        ON sc.id = a.schedule_id
    WHERE sc.teacher_id = @teacherId
      AND a.attendance_date = CURDATE()
      AND a.status = 'Present'";

                        using (MySqlCommand presentCommand =
                            new MySqlCommand(presentQuery, connection))
                        {
                            presentCommand.Parameters.AddWithValue(
                                "@teacherId", teacherDatabaseId);

                            int presentToday = Convert.ToInt32(
                                presentCommand.ExecuteScalar());

                            lblPresentToday.Text = presentToday.ToString();
                            string lateQuery = @"
    SELECT COUNT(DISTINCT a.student_id)
    FROM attendance a
    INNER JOIN schedules sc
        ON sc.id = a.schedule_id
    WHERE sc.teacher_id = @teacherId
      AND a.attendance_date = CURDATE()
      AND a.status = 'Late'";

                            using (MySqlCommand lateCommand =
                                new MySqlCommand(lateQuery, connection))
                            {
                                lateCommand.Parameters.AddWithValue(
                                    "@teacherId", teacherDatabaseId);

                                int lateToday = Convert.ToInt32(
                                    lateCommand.ExecuteScalar());

                                lblLateToday.Text = lateToday.ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load teacher summary:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
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

            card.MouseEnter += (s, e) =>
                card.BackColor = ControlPaint.Light(cardColor, 0.08f);

            card.MouseLeave += (s, e) =>
                card.BackColor = cardColor;

            parent.Controls.Add(card);
        }

        private void OpenAttendance()
        {
            using (AttendanceForm form = new AttendanceForm())
                form.ShowDialog(this);
        }

        private void OpenQRScanner()
        {
            using (QRScannerForm form = new QRScannerForm())
                form.ShowDialog(this);
        }

        private void OpenReports()
        {
            using (ReportsForm form = new ReportsForm())
                form.ShowDialog(this);
        }


        private void OpenChangePassword()
        {
            using (ChangePasswordForm form = new ChangePasswordForm(userId))
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
                this.Hide();

                using (LoginForm login = new LoginForm())
                {
                    login.ShowDialog();
                }

                this.Close();
            }
        }

    }
}
