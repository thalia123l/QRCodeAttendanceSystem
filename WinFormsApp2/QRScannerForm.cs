using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Drawing;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.Windows.Compatibility;   // <-- NEW: provides BarcodeReader for Bitmap
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

// Resolve the ambiguity between System.Drawing and OpenCvSharp
using Point = System.Drawing.Point;  // <-- NEW
using Size = System.Drawing.Size;    // <-- NEW
namespace QRAttendanceSystem
{
    public partial class QRScannerForm : Form
    {
        private VideoCapture camera;
        private System.Windows.Forms.Timer cameraTimer;
        private PictureBox cameraBox;

        private Label lblStatus;
        private Label lblStudent;
        private ComboBox cmbSubjects;
        private int selectedSubjectId = 0;

        private BarcodeReader barcodeReader;

        private string lastScannedCode = "";
        private DateTime lastScanTime = DateTime.MinValue;

        public QRScannerForm()
        {
            InitializeComponent();
            BuildUI();
            LoadSubjects();
            InitializeScanner();

            cmbSubjects.SelectedIndexChanged += (sender, e) =>
            {
                if (cmbSubjects.SelectedItem != null)
                {
                    selectedSubjectId =
                        ((SubjectItem)cmbSubjects.SelectedItem).Id;
                }
            };
        }
        private void BuildUI()
        {
            this.Text = "QR Attendance Scanner";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.AutoScroll = true;

            Label lblTitle = new Label();
            lblTitle.Text = "QR Attendance Scanner";
            lblTitle.Font = new Font(
                "Segoe UI",
                22,
                FontStyle.Bold);

            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 25);
            lblTitle.AutoSize = true;

            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text =
                "Scan a student's QR code to record attendance";

            lblSubtitle.Font = new Font(
                "Segoe UI",
                10);

            lblSubtitle.ForeColor =
                Color.FromArgb(160, 170, 190);

            lblSubtitle.Location =
                new Point(32, 65);

            lblSubtitle.AutoSize = true;

            this.Controls.Add(lblSubtitle);

            cameraBox = new PictureBox();

            cameraBox.Location =
                new Point(30, 110);

            cameraBox.Size =
                new Size(620, 460);

            cameraBox.BackColor =
                Color.Black;

            cameraBox.SizeMode =
                PictureBoxSizeMode.Zoom;

            this.Controls.Add(cameraBox);

            Panel infoPanel = new Panel();

            infoPanel.Location =
                new Point(675, 110);

            infoPanel.Size =
                new Size(190, 460);

            infoPanel.BackColor =
                Color.FromArgb(30, 39, 58);

            this.Controls.Add(infoPanel);

            Label lblInfoTitle = new Label();

            lblInfoTitle.Text =
                "SCAN STATUS";

            lblInfoTitle.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold);

            lblInfoTitle.ForeColor =
                Color.FromArgb(160, 170, 190);

            lblInfoTitle.Location =
                new Point(20, 25);

            lblInfoTitle.AutoSize = true;

            infoPanel.Controls.Add(lblInfoTitle);

            lblStatus = new Label();

            lblStatus.Text =
                "Camera starting...";

            lblStatus.Font =
                new Font(
                    "Segoe UI",
                    12,
                    FontStyle.Bold);

            lblStatus.ForeColor =
                Color.White;

            lblStatus.Location =
                new Point(20, 65);

            lblStatus.MaximumSize =
                new Size(150, 100);

            lblStatus.AutoSize = true;

            infoPanel.Controls.Add(lblStatus);

            Label lblStudentTitle = new Label();

            lblStudentTitle.Text =
                "STUDENT";

            lblStudentTitle.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold);

            lblStudentTitle.ForeColor =
                Color.FromArgb(160, 170, 190);

            lblStudentTitle.Location =
                new Point(20, 180);

            lblStudentTitle.AutoSize = true;

            infoPanel.Controls.Add(lblStudentTitle);

            lblStudent = new Label();

            lblStudent.Text =
                "Waiting for scan...";

            lblStudent.Font =
                new Font(
                    "Segoe UI",
                    11,
                    FontStyle.Bold);

            lblStudent.ForeColor =
                Color.White;

            lblStudent.Location =
                new Point(20, 215);

            lblStudent.MaximumSize =
                new Size(150, 120);

            lblStudent.AutoSize = true;

            infoPanel.Controls.Add(lblStudent);

            Label lblInstruction = new Label();

            lblInstruction.Text =
                "Position the QR code inside the camera view.";

            lblInstruction.Font =
                new Font(
                    "Segoe UI",
                    9);

            lblInstruction.ForeColor =
                Color.FromArgb(160, 170, 190);

            lblInstruction.Location =
                new Point(20, 350);

            lblInstruction.MaximumSize =
                new Size(150, 80);

            lblInstruction.AutoSize = true;

            infoPanel.Controls.Add(lblInstruction);

            Label lblSubject = new Label();
            lblSubject.Text = "SELECT SUBJECT";
            lblSubject.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            lblSubject.ForeColor = Color.FromArgb(160, 170, 190);
            lblSubject.Location = new Point(20, 300);
            lblSubject.AutoSize = true;
            infoPanel.Controls.Add(lblSubject);

            cmbSubjects = new ComboBox();
            cmbSubjects.Location = new Point(20, 325);
            cmbSubjects.Size = new Size(150, 30);
            cmbSubjects.DropDownStyle = ComboBoxStyle.DropDownList;
            infoPanel.Controls.Add(cmbSubjects);

        }


        private void LoadSubjects()
        {
            try
            {
                using (MySqlConnection conn =
                    DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT id, subject_code, subject_name
                FROM subjects
                WHERE status = 'Active'
                ORDER BY subject_code";

                    using (MySqlCommand cmd =
                        new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmbSubjects.Items.Clear();

                        while (reader.Read())
                        {
                            cmbSubjects.Items.Add(new SubjectItem
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Display = reader["subject_code"].ToString()
                                    + " - "
                                    + reader["subject_name"].ToString()
                            });
                        }
                    }
                }

                if (cmbSubjects.Items.Count > 0)
                {
                    cmbSubjects.SelectedIndex = 0;
                    selectedSubjectId =
                        ((SubjectItem)cmbSubjects.SelectedItem).Id;
                }
                else
                {
                    selectedSubjectId = 0;
                    lblStatus.Text = "No active subjects found.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load subjects: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private class SubjectItem
        {
            public int Id { get; set; }
            public string Display { get; set; }

            public override string ToString()
            {
                return Display;
            }
        }


        private void InitializeScanner()
        {
            try
            {
                barcodeReader = new BarcodeReader();

                camera = new VideoCapture(0);

                if (!camera.IsOpened())
                {
                    MessageBox.Show(
                        "Unable to open the camera.",
                        "Camera Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    lblStatus.Text =
                        "Camera unavailable.";

                    return;
                }

                cameraTimer = new System.Windows.Forms.Timer();

                cameraTimer.Interval = 100;

                cameraTimer.Tick +=
                    CameraTimer_Tick;

                cameraTimer.Start();

                lblStatus.Text =
                    "Ready to scan";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error starting camera:\n\n" +
                    ex.Message,
                    "Scanner Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                lblStatus.Text =
                    "Scanner error.";
            }
        }

        private void CameraTimer_Tick(
            object sender,
            EventArgs e)
        {
            try
            {
                if (camera == null ||
                    !camera.IsOpened())
                    return;

                using (Mat frame = new Mat())
                {
                    camera.Read(frame);

                    if (frame.Empty())
                        return;

                    Bitmap bitmap =
                        BitmapConverter.ToBitmap(frame);

                    if (cameraBox.Image != null)
                    {
                        cameraBox.Image.Dispose();
                    }

                    cameraBox.Image = bitmap;

                    DecodeQRCode(bitmap);
                }
            }
            catch
            {
                // Ignore individual camera frame errors.
            }
        }

        private void DecodeQRCode(Bitmap bitmap)
        {
            try
            {
                if (barcodeReader == null)
                    return;

                var result =
                    barcodeReader.Decode(bitmap);

                if (result == null)
                    return;

                string qrCode =
                    result.Text.Trim();

                System.Diagnostics.Debug.WriteLine("QR CONTENT: [" + qrCode + "]");

                if (string.IsNullOrEmpty(qrCode))
                    return;

                // Prevent the same QR code
                // from being processed repeatedly.
                if (qrCode == lastScannedCode &&
                    (DateTime.Now - lastScanTime)
                    .TotalSeconds < 3)
                {
                    return;
                }

                lastScannedCode = qrCode;
                lastScanTime = DateTime.Now;

                ProcessQRCode(qrCode);
            }
            catch
            {
                // Ignore decoding errors.
            }
        }


        private void ProcessQRCode(string qrCode)
        {
            if (selectedSubjectId <= 0)
            {
                MessageBox.Show("Please select a subject first.");
                return;
            }

            try
            {
                using (MySqlConnection conn =
                    DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    // 1. Find the registered student.
                    string studentQuery = @"
                SELECT id, student_id, first_name, last_name, section
                FROM students
                WHERE student_id = @studentCode
                  AND status = 'Active'
                LIMIT 1";

                    int studentDbId;
                    string studentCode;
                    string fullName;
                    string section;

                    using (MySqlCommand cmd =
                        new MySqlCommand(studentQuery, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@studentCode", qrCode);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                lblStatus.Text = "Student not found";
                                lblStudent.Text = qrCode;

                                MessageBox.Show(
                                    "This student ID is not registered or is inactive.",
                                    "Invalid Student",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                return;
                            }

                            studentDbId = Convert.ToInt32(reader["id"]);
                            studentCode = reader["student_id"].ToString();
                            fullName = reader["first_name"].ToString()
                                + " " + reader["last_name"].ToString();
                            section = reader["section"].ToString();
                        }
                    }

                    // 2. Find the selected subject's schedule
                    // for this student's section today.
                    string today = DateTime.Now.DayOfWeek.ToString();
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    TimeSpan startTime;
                    TimeSpan endTime;
                    int scheduleId;

                    string scheduleQuery = @"
                SELECT id, start_time, end_time
                FROM schedules
                WHERE subject_id = @subjectId
                  AND section = @section
                  AND day = @day
                LIMIT 1";

                    using (MySqlCommand cmd =
                        new MySqlCommand(scheduleQuery, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@subjectId", selectedSubjectId);
                        cmd.Parameters.AddWithValue(
                            "@section", section);
                        cmd.Parameters.AddWithValue("@day", today);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                lblStatus.Text = "No matching class";
                                lblStudent.Text = fullName;

                                MessageBox.Show(
                                    "No schedule was found today for "
                                    + section + " under the selected subject.",
                                    "Schedule Not Found",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                                return;
                            }

                            scheduleId = Convert.ToInt32(reader["id"]);
                            startTime = (TimeSpan)reader["start_time"];
                            endTime = (TimeSpan)reader["end_time"];
                        }
                    }

                    // 3. Do not record attendance outside class hours.
                    if (currentTime > endTime)
                    {
                        lblStatus.Text = "Class has ended";
                        lblStudent.Text = fullName;

                        MessageBox.Show(
                            "The scheduled class has already ended.",
                            "Attendance Not Recorded",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return;
                    }

                    // 4. Prevent duplicate attendance for this subject today.
                    string duplicateQuery = @"
                SELECT COUNT(*)
                FROM attendance
                WHERE student_id = @studentId
                  AND subject_id = @subjectId
                  AND attendance_date = @attendanceDate";

                    using (MySqlCommand cmd =
                        new MySqlCommand(duplicateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentId", studentDbId);
                        cmd.Parameters.AddWithValue(
                            "@subjectId", selectedSubjectId);
                        cmd.Parameters.AddWithValue(
                            "@attendanceDate", DateTime.Today);

                        int existing = Convert.ToInt32(cmd.ExecuteScalar());

                        if (existing > 0)
                        {
                            lblStatus.Text = "Already recorded";
                            lblStudent.Text = fullName;

                            MessageBox.Show(
                                fullName + " already has attendance recorded "
                                + "for this subject today.",
                                "Duplicate Attendance",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            return;
                        }
                    }

                    // 5. Apply the 15-minute grace period.
                    TimeSpan lateDeadline = startTime.Add(
                        TimeSpan.FromMinutes(15));

                    string attendanceStatus =
                        currentTime > lateDeadline ? "Late" : "Present";

                    // 6. Save attendance to MySQL.
                    string insertQuery = @"
                INSERT INTO attendance
                (student_id, subject_id, schedule_id,
                 attendance_date, time_in, status)
                VALUES
                (@studentId, @subjectId, @scheduleId,
                 @attendanceDate, @timeIn, @status)";

                    using (MySqlCommand cmd =
                        new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@studentId", studentDbId);
                        cmd.Parameters.AddWithValue(
                            "@subjectId", selectedSubjectId);
                        cmd.Parameters.AddWithValue("@scheduleId", scheduleId);
                        cmd.Parameters.AddWithValue(
                            "@attendanceDate", DateTime.Today);
                        cmd.Parameters.AddWithValue(
                            "@timeIn", currentTime);
                        cmd.Parameters.AddWithValue(
                            "@status", attendanceStatus);

                        cmd.ExecuteNonQuery();
                    }

                    // 7. Display the result.
                    lblStatus.Text = "Attendance " + attendanceStatus;
                    lblStudent.Text = fullName + "\n"
                        + studentCode + "\n"
                        + section;

                    MessageBox.Show(
                        "Student: " + fullName
                        + "\nStudent ID: " + studentCode
                        + "\nSubject ID: " + selectedSubjectId
                        + "\nTime In: " + DateTime.Now.ToString("hh:mm:ss tt")
                        + "\nStatus: " + attendanceStatus,
                        "Attendance Recorded",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Attendance error";

                MessageBox.Show(
                    "Unable to record attendance:\n" + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        protected override void OnFormClosing(
            FormClosingEventArgs e)
        {
            if (cameraTimer != null)
            {
                cameraTimer.Stop();
                cameraTimer.Dispose();
            }

            if (camera != null)
            {
                camera.Release();
                camera.Dispose();
            }

            if (cameraBox != null &&
                cameraBox.Image != null)
            {
                cameraBox.Image.Dispose();
            }

            base.OnFormClosing(e);
        }
    }
}