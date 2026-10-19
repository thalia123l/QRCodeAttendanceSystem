
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using QRCoder;

namespace QRAttendanceSystem
{
    public partial class QRGeneratorForm : Form
    {
        private ComboBox cmbStudents;
        private PictureBox picQR;
        private Label lblStudentInfo;
        private Button btnGenerate;
        private Button btnSave;

        private TextBox txtSearchStudent;
        private Button btnSearchStudent;

        public QRGeneratorForm()
        {
            InitializeComponent();
            BuildInterface();
            LoadStudents();
        }


private void BtnSearchStudent_Click(object sender, EventArgs e)
        {
            string search = txtSearchStudent.Text.Trim();

            cmbStudents.Items.Clear();

            if (string.IsNullOrWhiteSpace(search))
            {
                LoadStudents();
                return;
            }

            try
            {
                using (MySqlConnection conn =
                    Database.DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                SELECT student_id, first_name, last_name, section
                FROM students
                WHERE status = 'Active'
                  AND (
                    student_id LIKE @search
                    OR first_name LIKE @search
                    OR last_name LIKE @search
                    OR CONCAT(first_name, ' ', last_name) LIKE @search
                  )
                ORDER BY last_name, first_name";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue(
                            "@search", "%" + search + "%");

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cmbStudents.Items.Add(new StudentItem
                                {
                                    StudentId = reader["student_id"].ToString(),
                                    FullName = reader["first_name"].ToString()
                                        + " " + reader["last_name"].ToString(),
                                    Section = reader["section"].ToString()
                                });
                            }
                        }
                    }
                }

                if (cmbStudents.Items.Count > 0)
                {
                    cmbStudents.SelectedIndex = 0;
                    lblStudentInfo.Text =
                        cmbStudents.Items.Count + " matching student(s) found.";
                }
                else
                {
                    lblStudentInfo.Text = "No matching student found.";

                    picQR.Image?.Dispose();
                    picQR.Image = null;

                    MessageBox.Show(
                        "No matching active student found.",
                        "Search Results");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Student search failed: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void BuildInterface()
        {
            this.Text = "QR Code Generator";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 24, 38);
            this.ForeColor = Color.White;

            Label title = new Label();
            title.Text = "STUDENT QR CODE GENERATOR";
            title.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.Location = new Point(30, 25);
            title.Size = new Size(600, 35);
            this.Controls.Add(title);


            Label lblSearch = new Label();
            lblSearch.Text = "Search Student Name or ID";
            lblSearch.Location = new Point(35, 80);
            lblSearch.Size = new Size(250, 25);
            lblSearch.ForeColor = Color.White;
            this.Controls.Add(lblSearch);

            txtSearchStudent = new TextBox();
            txtSearchStudent.Location = new Point(35, 110);
            txtSearchStudent.Size = new Size(430, 35);
            txtSearchStudent.Font = new Font("Segoe UI", 11);
            txtSearchStudent.PlaceholderText = "Enter student name or ID...";
            this.Controls.Add(txtSearchStudent);

            btnSearchStudent = new Button();
            btnSearchStudent.Text = "SEARCH";
            btnSearchStudent.Location = new Point(475, 108);
            btnSearchStudent.Size = new Size(160, 35);
            btnSearchStudent.BackColor = Color.FromArgb(45, 105, 180);
            btnSearchStudent.ForeColor = Color.White;
            btnSearchStudent.FlatStyle = FlatStyle.Flat;
            btnSearchStudent.Cursor = Cursors.Hand;
            btnSearchStudent.Click += BtnSearchStudent_Click;
            this.Controls.Add(btnSearchStudent);

            Label lblSelect = new Label();
            lblSelect.Text = "Select Matching Student";
            lblSelect.Location = new Point(35, 155);
            lblSelect.Size = new Size(250, 25);
            lblSelect.ForeColor = Color.White;
            this.Controls.Add(lblSelect);

            cmbStudents = new ComboBox();
            cmbStudents.Location = new Point(35, 185);
            cmbStudents.Size = new Size(600, 35);
            cmbStudents.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbStudents.Font = new Font("Segoe UI", 11);
            cmbStudents.BackColor = Color.White;
            cmbStudents.ForeColor = Color.FromArgb(25, 35, 55);
            this.Controls.Add(cmbStudents);


            lblStudentInfo = new Label();
            lblStudentInfo.Location = new Point(35, 230);
            lblStudentInfo.Size = new Size(600, 30);
            lblStudentInfo.Font = new Font("Segoe UI", 10);
            this.Controls.Add(lblStudentInfo);

            picQR = new PictureBox();
            picQR.Location = new Point(235, 285);
            picQR.Size = new Size(200, 200);
            picQR.BackColor = Color.White;
            picQR.SizeMode = PictureBoxSizeMode.Zoom;
            this.Controls.Add(picQR);

            btnGenerate = new Button();
            btnGenerate.Text = "Generate QR Code";
            btnGenerate.Location = new Point(160, 510);
            btnGenerate.Size = new Size(180, 40);
            btnGenerate.BackColor = Color.FromArgb(45, 105, 180);
            btnGenerate.ForeColor = Color.White;
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.Click += BtnGenerate_Click;
            this.Controls.Add(btnGenerate);

            btnSave = new Button();
            btnSave.Text = "Save QR as PNG";
            btnSave.Location = new Point(360, 510);
            btnSave.Size = new Size(180, 40);
            btnSave.BackColor = Color.FromArgb(35, 145, 100);
            btnSave.ForeColor = Color.White;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);
        }

        private void LoadStudents()
        {
            try
            {
                using (MySqlConnection conn =
                    Database.DatabaseConnection.GetConnection())
                {
                    conn.Open();

                    string query = @"
                        SELECT student_id, first_name, last_name, section
                        FROM students
                        WHERE status = 'Active'
                        ORDER BY last_name, first_name";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string studentId = reader["student_id"].ToString();
                            string firstName = reader["first_name"].ToString();
                            string lastName = reader["last_name"].ToString();
                            string section = reader["section"].ToString();

                            cmbStudents.Items.Add(new StudentItem
                            {
                                StudentId = studentId,
                                FullName = firstName + " " + lastName,
                                Section = section
                            });
                        }
                    }
                }

                if (cmbStudents.Items.Count > 0)
                    cmbStudents.SelectedIndex = 0;
                else
                    lblStudentInfo.Text = "No active students found.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load students: " + ex.Message,
                    "Database Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            if (cmbStudents.SelectedItem == null)
            {
                MessageBox.Show("Please select a student first.");
                return;
            }

            StudentItem student = (StudentItem)cmbStudents.SelectedItem;

            try
            {
                using (QRCodeGenerator generator = new QRCodeGenerator())
                using (QRCodeData data = generator.CreateQrCode(
                    student.StudentId,
                    QRCodeGenerator.ECCLevel.Q))
                using (QRCode qrCode = new QRCode(data))
                {
                    Bitmap image = qrCode.GetGraphic(10);

                    Image oldImage = picQR.Image;
                    picQR.Image = image;
                    if (oldImage != null)
                        oldImage.Dispose();
                }

                lblStudentInfo.Text =
                    student.FullName + " | " +
                    student.StudentId + " | " + student.Section;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "QR generation failed: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (picQR.Image == null || cmbStudents.SelectedItem == null)
            {
                MessageBox.Show("Generate a QR code before saving.");
                return;
            }

            StudentItem student = (StudentItem)cmbStudents.SelectedItem;

            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Filter = "PNG Image|*.png";
                saveDialog.FileName = student.StudentId + "_QR.png";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    picQR.Image.Save(
                        saveDialog.FileName,
                        ImageFormat.Png);

                    MessageBox.Show("QR code saved successfully.");
                }
            }
        }

        private class StudentItem
        {
            public string StudentId { get; set; }
            public string FullName { get; set; }
            public string Section { get; set; }

            public override string ToString()
            {
                return StudentId + " - " + FullName;
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (picQR != null && picQR.Image != null)
            {
                picQR.Image.Dispose();
                picQR.Image = null;
            }

            base.OnFormClosed(e);
        }
    }
}
