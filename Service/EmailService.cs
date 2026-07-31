using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace TaskTrackerProject.Service
{
    public class EmailService : IEmailService
    {
        public async Task<bool> EmailSend(string email)
        {
            try
            {
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress("Ayusha", "ayushakarki689@gmail.com"));
                message.To.Add(MailboxAddress.Parse(email));

                message.Subject = "Test Email from MailKit";

                message.Body = new BodyBuilder
                {
                    HtmlBody = "<h2>Hello!</h2><p>This is a test email sent using <b>MailKit</b>.</p>",
                    TextBody = "Hello! This is a test email sent using MailKit."
                }.ToMessageBody();

                using var client = new SmtpClient();

                await client.ConnectAsync(
                    "smtp.gmail.com",
                    587,
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(
                    "ayushakarki689@gmail.com",
                    "your_app_password_here"); // Replace with your app password

                await client.SendAsync(message);

                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email Error: {ex.Message}");
                return false;
            }
        }
    }
}