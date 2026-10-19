using System;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class AdminDashboard : Form
    {

        private int presentCount = 0;
        private int lateCount = 0;
        private int absentCount = 0;
        private Panel chartArea;
        private Panel sidebar;
        private Panel contentPanel;
        private Panel headerPanel;

        private Label lblWelcome;
        private Label lblDate;

        private Label lblStudentsValue;
        private Label lblTeachersValue;
        private Label lblPresentValue;
        private Label lblLateValue;
        private Label lblAbsentValue;

        private DataGridView attendanceGrid;


        public AdminDashboard()
        {
            InitializeComponent();

            BuildDashboard();
            LoadDashboardData();

            this.Activated += (sender, e) =>
            {
                LoadDashboardData();
            };
        }


        private void BuildDashboard()
        {
            // ==============================
            // FORM
            // ==============================

            this.Text = "QR Attendance System - Admin Dashboard";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(1200, 720);
            this.MinimumSize = new Size(1100, 650);
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;


            // ==============================
            // MODERN SIDEBAR
            // ==============================

            sidebar = new Panel();
            sidebar.AutoScroll = true;
            sidebar.BackColor = Color.FromArgb(7, 22, 46);
            sidebar.Location = new Point(0, 0);
            sidebar.Size = new Size(230, 720);
            sidebar.AutoScrollMinSize = new Size(0, 850);
            this.Controls.Add(sidebar);

            // BCP LOGO
            PictureBox logoPicture = new PictureBox();
            logoPicture.Size = new Size(85, 85);
            logoPicture.Location = new Point(72, 22);
            logoPicture.SizeMode = PictureBoxSizeMode.Zoom;
            logoPicture.BackColor = Color.Transparent;

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

            sidebar.Controls.Add(logoPicture);

            // SYSTEM BRANDING
            Label lblSystem = new Label();
            lblSystem.Text = "QR ATTENDANCE";
            lblSystem.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblSystem.ForeColor = Color.White;
            lblSystem.TextAlign = ContentAlignment.MiddleCenter;
            lblSystem.Location = new Point(10, 115);
            lblSystem.Size = new Size(210, 30);
            sidebar.Controls.Add(lblSystem);

            Label lblSystemSub = new Label();
            lblSystemSub.Text = "M A N A G E M E N T   S Y S T E M";
            lblSystemSub.Font = new Font("Segoe UI", 6.5F, FontStyle.Bold);
            lblSystemSub.ForeColor = Color.FromArgb(115, 170, 235);
            lblSystemSub.TextAlign = ContentAlignment.MiddleCenter;
            lblSystemSub.Location = new Point(8, 145);
            lblSystemSub.Size = new Size(214, 25);
            sidebar.Controls.Add(lblSystemSub);

            // BLUE ACCENT
            Panel accent = new Panel();
            accent.BackColor = Color.FromArgb(30, 135, 255);
            accent.Location = new Point(75, 181);
            accent.Size = new Size(80, 4);
            sidebar.Controls.Add(accent);

            // NAVIGATION MENU
            int menuY = 205;

            AddMenuButton("⌂   Dashboard", menuY, true);
            AddMenuButton("♙   Students", menuY + 52, false);
            AddMenuButton("♟   Teachers", menuY + 104, false);
            AddMenuButton("▤   Subjects", menuY + 156, false);
            AddMenuButton("▦   Schedules", menuY + 208, false);
            AddMenuButton("✓   Attendance", menuY + 260, false);
            AddMenuButton("▥   Reports", menuY + 312, false);
            AddMenuButton("▣   QR Scanner", menuY + 364, false);
            AddMenuButton("▦   QR Generator", menuY + 416, false);

            // DIVIDER
            Panel divider = new Panel();
            divider.BackColor = Color.FromArgb(35, 62, 100);
            divider.Location = new Point(20, 685);
            divider.Size = new Size(190, 1);
            sidebar.Controls.Add(divider);

            // ADMIN PROFILE
            Label lblAdmin = new Label();
            lblAdmin.Text = "●";
            lblAdmin.Font = new Font("Segoe UI", 25, FontStyle.Bold);
            lblAdmin.ForeColor = Color.FromArgb(75, 160, 255);
            lblAdmin.TextAlign = ContentAlignment.MiddleCenter;
            lblAdmin.Location = new Point(18, 700);
            lblAdmin.Size = new Size(45, 45);
            sidebar.Controls.Add(lblAdmin);

            Label lblAdminName = new Label();
            lblAdminName.Text = "Administrator";
            lblAdminName.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblAdminName.ForeColor = Color.White;
            lblAdminName.Location = new Point(65, 705);
            lblAdminName.Size = new Size(140, 20);
            sidebar.Controls.Add(lblAdminName);

            Label lblAdminRole = new Label();
            lblAdminRole.Text = "System Access";
            lblAdminRole.Font = new Font("Segoe UI", 8);
            lblAdminRole.ForeColor = Color.FromArgb(130, 165, 210);
            lblAdminRole.Location = new Point(65, 726);
            lblAdminRole.Size = new Size(130, 20);
            sidebar.Controls.Add(lblAdminRole);

            // LOGOUT
            Button btnLogout = new Button();
            btnLogout.Text = "↪   Logout";
            btnLogout.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnLogout.ForeColor = Color.FromArgb(220, 230, 245);
            btnLogout.BackColor = Color.FromArgb(12, 31, 59);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(150, 45, 55);
            btnLogout.TextAlign = ContentAlignment.MiddleLeft;
            btnLogout.Location = new Point(15, 780);
            btnLogout.Size = new Size(195, 42);
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.Click += BtnLogout_Click;
            sidebar.Controls.Add(btnLogout);


            // ==============================
            // CONTENT PANEL
            // ==============================

            contentPanel = new Panel();
            contentPanel.AutoScroll = true;

            contentPanel.BackColor =
                Color.FromArgb(
                    18,
                    24,
                    38
                );

            contentPanel.Location =
                new Point(230, 0);

            contentPanel.Size =
                new Size(970, 720);

            this.Controls.Add(contentPanel);

            // ==============================
            // HEADER
            // ==============================

            headerPanel = new Panel();

            headerPanel.BackColor =
                Color.FromArgb(
                    30,
                    38,
                    55
                );

            headerPanel.Location =
                new Point(0, 0);

            headerPanel.Size =
                new Size(970, 90);

            contentPanel.Controls.Add(
                headerPanel
            );

            // ==============================
            // WELCOME
            // ==============================

            lblWelcome = new Label();

            lblWelcome.Text =
                "Good afternoon, Admin 👋";

            lblWelcome.Font =
                new Font(
                    "Segoe UI",
                    20,
                    FontStyle.Bold
                );

            lblWelcome.ForeColor =
                Color.White;

            lblWelcome.Location =
                new Point(35, 20);

            lblWelcome.Size =
                new Size(500, 35);

            headerPanel.Controls.Add(
                lblWelcome
            );

            // ==============================
            // DATE
            // ==============================

            lblDate = new Label();

            lblDate.Text =
                DateTime.Now.ToString(
                    "dddd, MMMM dd, yyyy"
                );

            lblDate.Font =
                new Font(
                    "Segoe UI",
                    9
                );

            lblDate.ForeColor =
                Color.FromArgb(
                    150,
                    165,
                    190
                );

            lblDate.Location =
                new Point(38, 55);

            lblDate.Size =
                new Size(400, 25);

            headerPanel.Controls.Add(
                lblDate
            );

            // ==============================
            // ADMIN PROFILE
            // ==============================

            Label lblProfile = new Label();

            lblProfile.Text =
                "A";

            lblProfile.Font =
                new Font(
                    "Segoe UI",
                    13,
                    FontStyle.Bold
                );

            lblProfile.ForeColor =
                Color.White;

            lblProfile.BackColor =
                Color.FromArgb(
                    55,
                    115,
                    220
                );

            lblProfile.TextAlign =
                ContentAlignment.MiddleCenter;

            lblProfile.Location =
                new Point(850, 25);

            lblProfile.Size =
                new Size(45, 45);

            headerPanel.Controls.Add(
                lblProfile
            );

            // ==============================
            // PAGE TITLE
            // ==============================

            Label lblDashboard =
                new Label();

            lblDashboard.Text =
                "Dashboard";

            lblDashboard.Font =
                new Font(
                    "Segoe UI",
                    22,
                    FontStyle.Bold
                );

            lblDashboard.ForeColor =
                Color.White;

            lblDashboard.Location =
                new Point(35, 120);

            lblDashboard.Size =
                new Size(400, 40);

            contentPanel.Controls.Add(
                lblDashboard
            );

            // ==============================
            // STATISTICS CARDS
            // ==============================

            CreateStatCard("STUDENTS", "6", "Registered students", 25, 180);
            CreateStatCard("TEACHERS", "3", "Active teachers", 205, 180);
            CreateStatCard("PRESENT TODAY", "0", "Attendance recorded", 385, 180);
            CreateStatCard("LATE TODAY", "0", "Late attendance", 565, 180);
            CreateStatCard("ABSENT TODAY", "0", "Absent attendance", 745, 180);



            // Attendance Chart Panel
            Panel chartPanel = new Panel();
            chartPanel.Location = new Point(35, 630);
            chartPanel.Size = new Size(270, 250);
            chartPanel.BackColor = Color.FromArgb(28, 36, 52);
            chartPanel.BorderStyle = BorderStyle.FixedSingle;

            contentPanel.Controls.Add(chartPanel);
            chartPanel.BringToFront();

            Label chartTitle = new Label();
            chartTitle.Text = "Attendance Overview";
            chartTitle.ForeColor = Color.White;
            chartTitle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            chartTitle.Location = new Point(10, 10);
            chartTitle.AutoSize = true;

            chartPanel.Controls.Add(chartTitle);

            chartArea = new Panel();
            chartArea.Location = new Point(10, 45);
            chartArea.Size = new Size(205, 185);
            chartArea.BackColor = Color.FromArgb(28, 36, 52);

            chartPanel.Controls.Add(chartArea);


            chartArea.Paint += (sender, e) =>
            {
                Graphics g = e.Graphics;
                g.SmoothingMode =
                    System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                int maxValue = Math.Max(
                    1, Math.Max(presentCount,
                    Math.Max(lateCount, absentCount)));

                int barMaxWidth = 100;
                int barHeight = 15;
                int barX = 75;
                int barTrackWidth = 100;

                using Font labelFont = new Font("Segoe UI", 8);
                using Brush textBrush = new SolidBrush(Color.White);
                using Brush presentBrush = new SolidBrush(
                    Color.FromArgb(46, 204, 113));
                using Brush lateBrush = new SolidBrush(
                    Color.FromArgb(241, 196, 15));
                using Brush absentBrush = new SolidBrush(
                    Color.FromArgb(231, 76, 60));

                int[] counts = {
        presentCount, lateCount, absentCount
    };

                string[] labels = {
        "Present", "Late", "Absent"
    };

                Brush[] brushes = {
        presentBrush, lateBrush, absentBrush
    };

                for (int i = 0; i < counts.Length; i++)
                {
                    int y = 15 + (i * 50);

                    g.DrawString(
                        labels[i], labelFont, textBrush, 5, y);

                    int barWidth = (int)(
                        (double)counts[i] / maxValue * barMaxWidth);

                    g.FillRectangle(
    Brushes.DimGray,
    barX, y + 2,
    barTrackWidth, barHeight);

                    if (barWidth > 0)
                    {
                        g.FillRectangle(
                            brushes[i], barX, y + 2,
                            barWidth, barHeight);
                    }

                    g.DrawString(
                        counts[i].ToString(),
                        labelFont, textBrush,
                        78 + barWidth, y + 1);
                }
            };


            // ==============================
            // RECENT ATTENDANCE
            // ==============================

            Label lblRecent =
                new Label();

            lblRecent.Text =
                "Today's Attendance";

            lblRecent.Font =
                new Font(
                    "Segoe UI",
                    16,
                    FontStyle.Bold
                );

            lblRecent.ForeColor =
                Color.White;

            lblRecent.Location =
                new Point(35, 310);

            lblRecent.Size =
                new Size(400, 35);

            contentPanel.Controls.Add(
                lblRecent
            );

            // ==============================
            // TABLE
            // ==============================

            attendanceGrid =
    new DataGridView();

            attendanceGrid.Location =
                new Point(35, 355);

            attendanceGrid.Size =
                new Size(870, 250);

            attendanceGrid.BackgroundColor =
                Color.FromArgb(
                    30,
                    38,
                    55
                );

            attendanceGrid.BorderStyle =
                BorderStyle.None;

            attendanceGrid.GridColor =
                Color.FromArgb(
                    50,
                    60,
                    80
                );

            attendanceGrid.RowHeadersVisible =
                false;

            attendanceGrid.AllowUserToAddRows =
                false;

            attendanceGrid.AllowUserToResizeRows =
                false;

            attendanceGrid.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            attendanceGrid.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(
                            42,
                            51,
                            70
                        ),

                    ForeColor = Color.White,

                    Font =
                        new Font(
                            "Segoe UI",
                            9,
                            FontStyle.Bold
                        ),

                    SelectionBackColor =
                        Color.FromArgb(
                            42,
                            51,
                            70
                        )
                };

            attendanceGrid.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(
                            30,
                            38,
                            55
                        ),

                    ForeColor =
                        Color.FromArgb(
                            220,
                            225,
                            235
                        ),

                    Font =
                        new Font(
                            "Segoe UI",
                            9
                        ),

                    SelectionBackColor =
                        Color.FromArgb(
                            55,
                            75,
                            110
                        ),

                    SelectionForeColor =
                        Color.White
                };

            attendanceGrid.Columns.Add(
                "student",
                "Student"
            );

            attendanceGrid.Columns.Add(
                "section",
                "Section"
            );

            attendanceGrid.Columns.Add(
                "subject",
                "Subject"
            );

            attendanceGrid.Columns.Add(
                "time",
                "Time In"
            );

            attendanceGrid.Columns.Add(
                "status",
                "Status"
            );

            contentPanel.Controls.Add(
                attendanceGrid
            );

            contentPanel.AutoScrollMinSize = new Size(0, 920);
        }

        private void AddMenuButton(
    string text,
    int y,
    bool active)
        {
            Button button = new Button();

            button.Text = text;

            button.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Regular
                );

            button.ForeColor =
                active
                ? Color.White
                : Color.FromArgb(
                    175,
                    185,
                    205
                );

            button.BackColor =
                active
                ? Color.FromArgb(
                    55,
                    115,
                    220
                )
                : Color.Transparent;

            button.FlatStyle =
                FlatStyle.Flat;

            button.FlatAppearance.BorderSize =
                0;

            button.TextAlign =
                ContentAlignment.MiddleLeft;

            button.Location =
                new Point(15, y);

            button.Size =
                new Size(200, 45);

            button.Cursor =
                Cursors.Hand;

            // STUDENTS BUTTON
            if (text.Contains("Students"))
            {
                button.Click += (sender, e) =>
                {
                    StudentsForm studentsForm =
                        new StudentsForm();

                    studentsForm.ShowDialog();
                };
            }

            if (text.Contains("Teachers"))
            {
                button.Click += (sender, e) =>
                {
                    TeachersForm teachersForm =
                        new TeachersForm();

                    teachersForm.ShowDialog();
                };
            }

            if (text.Contains("Subjects"))
            {
                button.Click += (sender, e) =>
                {
                    SubjectsForm subjectsForm = new SubjectsForm();
                    subjectsForm.ShowDialog();
                };
            }

            if (text.Contains("Schedules"))
            {
                button.Click += (sender, e) =>
                {
                    SchedulesForm schedulesForm = new SchedulesForm();
                    schedulesForm.ShowDialog();
                };
            }

            if (text.Contains("Attendance"))
            {
                button.Click += (sender, e) =>
                {
                    AttendanceForm attendanceForm = new AttendanceForm();
                    attendanceForm.ShowDialog();
                };
            }

            if (text.Contains("QR Scanner"))
            {
                button.Click += (sender, e) =>
                {
                    QRScannerForm scannerForm = new QRScannerForm();
                    scannerForm.ShowDialog();
                };
            }


            if (text.Contains("QR Generator"))
            {
                button.Click += (sender, e) =>
                {
                    QRGeneratorForm generatorForm = new QRGeneratorForm();
                    generatorForm.ShowDialog();
                };
            }


            if (text.Contains("Reports"))
            {
                button.Click += (sender, e) =>
                {
                    ReportsForm reportsForm = new ReportsForm();
                    reportsForm.ShowDialog();
                };
            }

            sidebar.Controls.Add(button);
        }

        private void CreateStatCard(
            string title,
            string value,
            string subtitle,
            int x,
            int y)
        {
            Panel card = new Panel();

            card.BackColor =
                Color.FromArgb(
                    30,
                    38,
                    55
                );

            card.Location =
                new Point(x, y);

            card.Size =
                new Size(165, 105);

            contentPanel.Controls.Add(card);

            Label lblTitle =
                new Label();

            lblTitle.Text =
                title;

            lblTitle.Font =
                new Font(
                    "Segoe UI",
                    8,
                    FontStyle.Bold
                );

            lblTitle.ForeColor =
                Color.FromArgb(
                    140,
                    155,
                    180
                );

            lblTitle.Location =
                new Point(15, 12);

            lblTitle.Size =
                new Size(170, 20);

            card.Controls.Add(
                lblTitle
            );


            Label lblValue = new Label();

            lblValue.Text = value;

            lblValue.Font = new Font(
                "Segoe UI",
                25,
                FontStyle.Bold
            );

            lblValue.ForeColor = Color.White;

            lblValue.Location = new Point(15, 32);

            lblValue.Size = new Size(170, 40);

            card.Controls.Add(lblValue);

            if (title == "STUDENTS")
            {
                lblStudentsValue = lblValue;
            }
            else if (title == "TEACHERS")
            {
                lblTeachersValue = lblValue;
            }
            else if (title == "PRESENT TODAY")
            {
                lblPresentValue = lblValue;
            }
            else if (title == "LATE TODAY")
            {
                lblLateValue = lblValue;
            }
            else if (title == "ABSENT TODAY")
            {
                lblAbsentValue = lblValue;
            }

            Label lblSubtitle =
                new Label();

            lblSubtitle.Text =
                subtitle;

            lblSubtitle.Font =
                new Font(
                    "Segoe UI",
                    8
                );

            lblSubtitle.ForeColor =
                Color.FromArgb(
                    120,
                    135,
                    160
                );

            lblSubtitle.Location =
                new Point(15, 75);

            lblSubtitle.Size =
                new Size(170, 20);

            card.Controls.Add(
                lblSubtitle
            );
        }

        private void BtnLogout_Click(
    object sender,
    EventArgs e)
        {
            DialogResult result =
                MessageBox.Show(
                    "Are you sure you want to logout?",
                    "Logout",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void LoadDashboardData()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    // ==============================
                    // STUDENTS
                    // ==============================

                    string studentQuery =
                        "SELECT COUNT(*) FROM students " +
                        "WHERE status = 'Active'";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            studentQuery,
                            connection))
                    {
                        int count =
                            Convert.ToInt32(
                                command.ExecuteScalar()
                            );

                        lblStudentsValue.Text =
                            count.ToString();
                    }

                    // ==============================
                    // TEACHERS
                    // ==============================

                    string teacherQuery =
                        "SELECT COUNT(*) FROM teachers " +
                        "WHERE status = 'Active'";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            teacherQuery,
                            connection))
                    {
                        int count =
                            Convert.ToInt32(
                                command.ExecuteScalar()
                            );

                        lblTeachersValue.Text =
                            count.ToString();
                    }

                    // ==============================
                    // PRESENT TODAY
                    // ==============================

                    string presentQuery =
                        "SELECT COUNT(*) FROM attendance " +
                        "WHERE attendance_date = CURDATE() " +
                        "AND status = 'Present'";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            presentQuery,
                            connection))
                    {
                        int count =
                            Convert.ToInt32(
                                command.ExecuteScalar()
                            );

                        lblPresentValue.Text = count.ToString();
                        presentCount = count;
                    }

                    // ==============================
                    // LATE TODAY
                    // ==============================

                    string lateQuery =
                        "SELECT COUNT(*) FROM attendance " +
                        "WHERE attendance_date = CURDATE() " +
                        "AND status = 'Late'";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            lateQuery,
                            connection))
                    {
                        int count =
                            Convert.ToInt32(
                                command.ExecuteScalar()
                            );

                        lblLateValue.Text = count.ToString();
                        lateCount = count;
                    }

                    // ==============================
                    // ABSENT TODAY
                    // ==============================

                    string absentQuery =
                        "SELECT COUNT(*) FROM attendance " +
                        "WHERE attendance_date = CURDATE() " +
                        "AND status = 'Absent'";

                    using (MySqlCommand command =
                        new MySqlCommand(absentQuery, connection))
                    {
                        absentCount = Convert.ToInt32(
                            command.ExecuteScalar());
                        lblAbsentValue.Text = absentCount.ToString();
                    }

                    // ==============================
                    // TODAY'S ATTENDANCE TABLE
                    // ==============================

                    string attendanceQuery =
                        "SELECT " +
                        "CONCAT(s.first_name, ' ', s.last_name) AS Student, " +
                        "s.section AS Section, " +
                        "sub.subject_code AS Subject, " +
                        "TIME_FORMAT(a.time_in, '%h:%i %p') AS TimeIn, " +
                        "a.status AS Status " +
                        "FROM attendance a " +
                        "INNER JOIN students s " +
                        "ON a.student_id = s.id " +
                        "INNER JOIN subjects sub " +
                        "ON a.subject_id = sub.id " +
                        "WHERE a.attendance_date = CURDATE() " +
                        "ORDER BY a.time_in ASC";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            attendanceQuery,
                            connection))
                    {
                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            attendanceGrid.Rows.Clear();

                            while (reader.Read())
                            {
                                attendanceGrid.Rows.Add(
                                    reader["Student"].ToString(),
                                    reader["Section"].ToString(),
                                    reader["Subject"].ToString(),
                                    reader["TimeIn"].ToString(),
                                    reader["Status"].ToString()
                                );
                            }
                        }
                    }
                }
                chartArea.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load dashboard data.\n\n" +
                    ex.Message,
                    "Dashboard Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}