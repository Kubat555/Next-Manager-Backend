using MailKit.Net.Smtp;
using MimeKit;
using ProjectManagement.Services.Interfaces;
using ProjectManagement.Services.Models;

namespace ProjectManagement.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public string GetHtmlConfirmEmail(string userName, string link)
        {
            var htmlBody = @"
                <html>
                <head>
                    <style>
                        /* Стили для кнопки */
                        .button {
                            background-color: #2cadfa;
                            border: none;
                            color: white !important;
                            padding: 15px 32px;
                            text-align: center;
                            text-decoration: none;
                            display: inline-block;
                            font-size: 16px;
                            margin: 4px 2px;
                            cursor: pointer;
                            border-radius: 5px;
                        }
                        a{
                            color: white;
                        }
                    </style>
                </head>
                <body>
                    <h1 style='color: #333;'>ProjectManagementSystem</h1>
                    <h2 style='color: #333;'>Confirm your Email address</h2>
                    <p style='color: #555;'>Dear " + userName + @",</p>
                    <p style='color: #555;'>Please click the following button to confirm your email address:</p>
                    <a href='" + link + @"' class='button'>Confirm Email</a>
                </body>
                </html>";

            return htmlBody;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Kubat", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = message.Content; // Установите HTML-контент
            bodyBuilder.TextBody = "HEllo!";
            emailMessage.Body = bodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);

                client.Send(mailMessage);
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
