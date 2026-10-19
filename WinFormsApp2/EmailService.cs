
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace QRAttendanceSystem
{
    public class EmailService
    {
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;

        public async Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string body)
        {
            string emailAddress =
                ConfigurationManager.AppSettings["EmailAddress"];

            string appPassword =
                ConfigurationManager.AppSettings["EmailAppPassword"];

            if (string.IsNullOrWhiteSpace(emailAddress) ||
                string.IsNullOrWhiteSpace(appPassword))
            {
                throw new InvalidOperationException(
                    "Email settings are missing from App.config.");
            }

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(
                "QR Attendance System", emailAddress));

            message.To.Add(MailboxAddress.Parse(recipientEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(
                    SmtpServer, SmtpPort,
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    emailAddress, appPassword);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
