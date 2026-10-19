namespace QRAttendanceSystem
{
    partial class ChangePasswordForm
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
            lblTitle = new Label();
            lblCurrentPassword = new Label();
            txtCurrentPassword = new TextBox();
            lblNewPassword = new Label();
            txtNewPassword = new TextBox();
            lblConfirmPassword = new Label();
            txtConfirmPassword = new TextBox();
            btnChangePassword = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(303, 25);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(124, 20);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Change Password";
            // 
            // lblCurrentPassword
            // 
            lblCurrentPassword.AutoSize = true;
            lblCurrentPassword.Location = new Point(303, 66);
            lblCurrentPassword.Name = "lblCurrentPassword";
            lblCurrentPassword.Size = new Size(122, 20);
            lblCurrentPassword.TabIndex = 1;
            lblCurrentPassword.Text = "Current Password";
            // 
            // txtCurrentPassword
            // 
            txtCurrentPassword.Location = new Point(300, 107);
            txtCurrentPassword.Name = "txtCurrentPassword";
            txtCurrentPassword.Size = new Size(125, 27);
            txtCurrentPassword.TabIndex = 2;
            // 
            // lblNewPassword
            // 
            lblNewPassword.AutoSize = true;
            lblNewPassword.Location = new Point(303, 156);
            lblNewPassword.Name = "lblNewPassword";
            lblNewPassword.Size = new Size(104, 20);
            lblNewPassword.TabIndex = 3;
            lblNewPassword.Text = "New Password";
            // 
            // txtNewPassword
            // 
            txtNewPassword.Location = new Point(300, 191);
            txtNewPassword.Name = "txtNewPassword";
            txtNewPassword.Size = new Size(125, 27);
            txtNewPassword.TabIndex = 4;
            // 
            // lblConfirmPassword
            // 
            lblConfirmPassword.AutoSize = true;
            lblConfirmPassword.Location = new Point(276, 235);
            lblConfirmPassword.Name = "lblConfirmPassword";
            lblConfirmPassword.Size = new Size(161, 20);
            lblConfirmPassword.TabIndex = 5;
            lblConfirmPassword.Text = "Confirm New Password";
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(300, 271);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(125, 27);
            txtConfirmPassword.TabIndex = 6;
            // 
            // btnChangePassword
            // 
            btnChangePassword.Location = new Point(285, 333);
            btnChangePassword.Name = "btnChangePassword";
            btnChangePassword.Size = new Size(142, 29);
            btnChangePassword.TabIndex = 7;
            btnChangePassword.Text = "CHANGE PASSWORD";
            btnChangePassword.UseVisualStyleBackColor = true;
            // 
            // ChangePasswordForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnChangePassword);
            Controls.Add(txtConfirmPassword);
            Controls.Add(lblConfirmPassword);
            Controls.Add(txtNewPassword);
            Controls.Add(lblNewPassword);
            Controls.Add(txtCurrentPassword);
            Controls.Add(lblCurrentPassword);
            Controls.Add(lblTitle);
            Name = "ChangePasswordForm";
            Text = "ChangePasswordForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblCurrentPassword;
        private TextBox txtCurrentPassword;
        private Label lblNewPassword;
        private TextBox txtNewPassword;
        private Label lblConfirmPassword;
        private TextBox txtConfirmPassword;
        private Button btnChangePassword;
    }
}