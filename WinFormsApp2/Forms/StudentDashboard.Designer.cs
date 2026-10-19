namespace QRAttendanceSystem
{
    partial class StudentDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnChangePassword = new Button();
            SuspendLayout();
            // 
            // btnChangePassword
            // 
            btnChangePassword.Location = new Point(280, 120);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(128, 29);
            btnChangePassword.TabIndex = 0;
            btnChangePassword.Text = "ChangePassword";
            btnChangePassword.UseVisualStyleBackColor = true;
            // 
            // StudentDashboard
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnChangePassword);
            Name = "StudentDashboard";
            Text = "StudentDashboard";
            ResumeLayout(false);
        }

        #endregion

        private Button btnChangePassword;
    }
}