using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRAttendanceSystem.Database;

namespace QRAttendanceSystem
{
    public partial class AddScheduleForm : Form
    {
        private ComboBox cmbSubject;
        private ComboBox cmbTeacher;
        private TextBox txtSection;
        private ComboBox cmbDay;
        private DateTimePicker dtpStartTime;
        private DateTimePicker dtpEndTime;
        private TextBox txtRoom;

        private Button btnSave;
        private Button btnCancel;

        private TextBox txtSearchStudent;
        private Button btnSearchStudent;

        public AddScheduleForm()
        {
            InitializeComponent();
            BuildUI();
            LoadSubjects();
            LoadTeachers();
        }

        private void BuildUI()
        {
            this.Text = "Add Schedule";
            this.Size = new Size(560, 650);
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.ForeColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // =========================
            // TITLE
            // =========================

            Label lblTitle = new Label();
            lblTitle.Text = "Add New Schedule";
            lblTitle.Font = new Font(
                "Segoe UI",
                20,
                FontStyle.Bold
            );
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(35, 25);
            lblTitle.AutoSize = true;

            this.Controls.Add(lblTitle);

            Label lblSubtitle = new Label();
            lblSubtitle.Text =
                "Enter the class schedule information below.";
            lblSubtitle.Font =
                new Font("Segoe UI", 10);
            lblSubtitle.ForeColor =
                Color.LightGray;
            lblSubtitle.Location =
                new Point(38, 62);
            lblSubtitle.AutoSize = true;

            this.Controls.Add(lblSubtitle);

            // =========================
            // SUBJECT
            // =========================

            Label lblSubject = CreateLabel(
                "Subject",
                new Point(40, 105)
            );

            this.Controls.Add(lblSubject);

            cmbSubject = CreateComboBox(
                new Point(40, 130),
                new Size(450, 35)
            );

            this.Controls.Add(cmbSubject);

            // =========================
            // TEACHER
            // =========================

            Label lblTeacher = CreateLabel(
                "Teacher",
                new Point(40, 175)
            );

            this.Controls.Add(lblTeacher);

            cmbTeacher = CreateComboBox(
                new Point(40, 200),
                new Size(450, 35)
            );

            this.Controls.Add(cmbTeacher);

            // =========================
            // SECTION
            // =========================

            Label lblSection = CreateLabel(
                "Section",
                new Point(40, 245)
            );

            this.Controls.Add(lblSection);

            txtSection = CreateTextBox(
                new Point(40, 270),
                new Size(450, 35)
            );

            txtSection.PlaceholderText =
                "Example: BSCPE 41004";

            this.Controls.Add(txtSection);

            // =========================
            // DAY
            // =========================

            Label lblDay = CreateLabel(
                "Day",
                new Point(40, 315)
            );

            this.Controls.Add(lblDay);

            cmbDay = CreateComboBox(
                new Point(40, 340),
                new Size(450, 35)
            );

            cmbDay.Items.AddRange(new string[]
            {
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
                "Sunday"
            });

            this.Controls.Add(cmbDay);

            // =========================
            // START TIME
            // =========================

            Label lblStartTime = CreateLabel(
                "Start Time",
                new Point(40, 385)
            );

            this.Controls.Add(lblStartTime);

            dtpStartTime = CreateTimePicker(
                new Point(40, 410)
            );

            this.Controls.Add(dtpStartTime);

            // =========================
            // END TIME
            // =========================

            Label lblEndTime = CreateLabel(
                "End Time",
                new Point(270, 385)
            );

            this.Controls.Add(lblEndTime);

            dtpEndTime = CreateTimePicker(
                new Point(270, 410)
            );

            this.Controls.Add(dtpEndTime);

            // =========================
            // ROOM
            // =========================

            Label lblRoom = CreateLabel(
                "Room",
                new Point(40, 460)
            );

            this.Controls.Add(lblRoom);

            txtRoom = CreateTextBox(
                new Point(40, 485),
                new Size(450, 35)
            );

            txtRoom.PlaceholderText =
                "Example: Room 301";

            this.Controls.Add(txtRoom);

            // =========================
            // SAVE
            // =========================

            btnSave = new Button();

            btnSave.Text = "Save Schedule";
            btnSave.Location =
                new Point(250, 540);
            btnSave.Size =
                new Size(115, 40);

            btnSave.BackColor =
                Color.FromArgb(0, 120, 215);

            btnSave.ForeColor =
                Color.White;

            btnSave.FlatStyle =
                FlatStyle.Flat;

            btnSave.FlatAppearance.BorderSize = 0;

            btnSave.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                );

            btnSave.Cursor =
                Cursors.Hand;

            btnSave.Click += BtnSave_Click;

            this.Controls.Add(btnSave);

            // =========================
            // CANCEL
            // =========================

            btnCancel = new Button();

            btnCancel.Text = "Cancel";
            btnCancel.Location =
                new Point(375, 540);
            btnCancel.Size =
                new Size(115, 40);

            btnCancel.BackColor =
                Color.FromArgb(45, 55, 75);

            btnCancel.ForeColor =
                Color.White;

            btnCancel.FlatStyle =
                FlatStyle.Flat;

            btnCancel.FlatAppearance.BorderSize = 0;

            btnCancel.Font =
                new Font(
                    "Segoe UI",
                    9,
                    FontStyle.Bold
                );

            btnCancel.Cursor =
                Cursors.Hand;

            btnCancel.Click += (sender, e) =>
            {
                this.DialogResult =
                    DialogResult.Cancel;

                this.Close();
            };

            this.Controls.Add(btnCancel);
        }

        // =========================
        // LABEL
        // =========================

        private Label CreateLabel(
            string text,
            Point location)
        {
            Label label = new Label();

            label.Text = text;
            label.Location = location;
            label.AutoSize = true;

            label.Font =
                new Font(
                    "Segoe UI",
                    10,
                    FontStyle.Bold
                );

            label.ForeColor =
                Color.White;

            return label;
        }

        // =========================
        // TEXTBOX
        // =========================

        private TextBox CreateTextBox(
            Point location,
            Size size)
        {
            TextBox textBox = new TextBox();

            textBox.Location = location;
            textBox.Size = size;

            textBox.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            textBox.BackColor =
                Color.FromArgb(30, 39, 58);

            textBox.ForeColor =
                Color.White;

            textBox.BorderStyle =
                BorderStyle.FixedSingle;

            return textBox;
        }

        // =========================
        // COMBOBOX
        // =========================

        private ComboBox CreateComboBox(
            Point location,
            Size size)
        {
            ComboBox comboBox =
                new ComboBox();

            comboBox.Location =
                location;

            comboBox.Size =
                size;

            comboBox.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            comboBox.BackColor =
                Color.FromArgb(30, 39, 58);

            comboBox.ForeColor =
                Color.White;

            comboBox.DropDownStyle =
                ComboBoxStyle.DropDownList;

            return comboBox;
        }

        // =========================
        // TIME PICKER
        // =========================

        private DateTimePicker CreateTimePicker(
            Point location)
        {
            DateTimePicker picker =
                new DateTimePicker();

            picker.Location =
                location;

            picker.Size =
                new Size(210, 35);

            picker.Font =
                new Font(
                    "Segoe UI",
                    10
                );

            picker.Format =
                DateTimePickerFormat.Custom;

            picker.CustomFormat =
                "hh:mm tt";

            picker.ShowUpDown = true;

            picker.Value =
                DateTime.Today.AddHours(8);

            return picker;
        }

        // =========================
        // LOAD SUBJECTS
        // =========================

        private void LoadSubjects()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            id,
                            subject_code,
                            subject_name
                        FROM subjects
                        WHERE status = 'Active'
                        ORDER BY subject_code";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbSubject.Items.Add(
                                    new ComboBoxItem(
                                        Convert.ToInt32(
                                            reader["id"]
                                        ),
                                        reader["subject_code"]
                                            .ToString()
                                        + " - "
                                        + reader["subject_name"]
                                            .ToString()
                                    )
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading subjects:\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // LOAD TEACHERS
        // =========================

        private void LoadTeachers()
        {
            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT
                            id,
                            teacher_id,
                            first_name,
                            last_name
                        FROM teachers
                        WHERE status = 'Active'
                        ORDER BY last_name, first_name";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        using (MySqlDataReader reader =
                            command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbTeacher.Items.Add(
                                    new ComboBoxItem(
                                        Convert.ToInt32(
                                            reader["id"]
                                        ),
                                        reader["teacher_id"]
                                            .ToString()
                                        + " - "
                                        + reader["first_name"]
                                            .ToString()
                                        + " "
                                        + reader["last_name"]
                                            .ToString()
                                    )
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error loading teachers:\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // SAVE SCHEDULE
        // =========================

        private void BtnSave_Click(
            object sender,
            EventArgs e)
        {
            // =========================
            // VALIDATION
            // =========================

            if (cmbSubject.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a subject.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cmbSubject.Focus();
                return;
            }

            if (cmbTeacher.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a teacher.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cmbTeacher.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(
                txtSection.Text))
            {
                MessageBox.Show(
                    "Please enter the section.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                txtSection.Focus();
                return;
            }

            if (cmbDay.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a day.",
                    "Missing Information",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                cmbDay.Focus();
                return;
            }

            if (dtpEndTime.Value.TimeOfDay <=
                dtpStartTime.Value.TimeOfDay)
            {
                MessageBox.Show(
                    "End time must be later than start time.",
                    "Invalid Time",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );

                dtpEndTime.Focus();
                return;
            }

            // =========================
            // GET VALUES
            // =========================

            ComboBoxItem subject =
                (ComboBoxItem)cmbSubject.SelectedItem;

            ComboBoxItem teacher =
                (ComboBoxItem)cmbTeacher.SelectedItem;

            string section =
                txtSection.Text.Trim();

            string day =
                cmbDay.SelectedItem.ToString();

            TimeSpan startTime =
                dtpStartTime.Value.TimeOfDay;

            TimeSpan endTime =
                dtpEndTime.Value.TimeOfDay;

            string room =
                txtRoom.Text.Trim();

            // =========================
            // INSERT
            // =========================

            try
            {
                using (MySqlConnection connection =
                    DatabaseConnection.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO schedules
                        (
                            subject_id,
                            teacher_id,
                            section,
                            day,
                            start_time,
                            end_time,
                            room
                        )
                        VALUES
                        (
                            @subject_id,
                            @teacher_id,
                            @section,
                            @day,
                            @start_time,
                            @end_time,
                            @room
                        )";

                    using (MySqlCommand command =
                        new MySqlCommand(
                            query,
                            connection))
                    {
                        command.Parameters.AddWithValue(
                            "@subject_id",
                            subject.Id
                        );

                        command.Parameters.AddWithValue(
                            "@teacher_id",
                            teacher.Id
                        );

                        command.Parameters.AddWithValue(
                            "@section",
                            section
                        );

                        command.Parameters.AddWithValue(
                            "@day",
                            day
                        );

                        command.Parameters.AddWithValue(
                            "@start_time",
                            startTime
                        );

                        command.Parameters.AddWithValue(
                            "@end_time",
                            endTime
                        );

                        command.Parameters.AddWithValue(
                            "@room",
                            string.IsNullOrWhiteSpace(room)
                                ? (object)DBNull.Value
                                : room
                        );

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show(
                    "Schedule added successfully.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult =
                    DialogResult.OK;

                this.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Database error:\n\n"
                    + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error adding schedule:\n\n"
                    + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        // =========================
        // COMBOBOX ITEM
        // =========================

        private class ComboBoxItem
        {
            public int Id { get; set; }
            public string Text { get; set; }

            public ComboBoxItem(
                int id,
                string text)
            {
                Id = id;
                Text = text;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}