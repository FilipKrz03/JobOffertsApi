using MailKit.Net.Smtp;
using MailSedningService.Interfaces;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSedningService.Services
{
    public class SendMailToUsersGroupWithRecommendedOfferService : ISendMailToUsersGroupWithRecommendedOfferService
    {
        public void SendMail()
        {
            var sendingEmail = Environment.GetEnvironmentVariable("sendingEmail");
            var smtpServer = Environment.GetEnvironmentVariable("smtpServer");
            var smtpPort = int.Parse(Environment.GetEnvironmentVariable("smtpPort")!);
            var smtpLogin = Environment.GetEnvironmentVariable("smtpLogin");
            var smtpPassword = Environment.GetEnvironmentVariable("smtpPassword");

            var email = new MimeMessage();

            string body = "Test";

            email.From.Add(MailboxAddress.Parse(sendingEmail));
            email.To.Add(MailboxAddress.Parse("filipos2003@gmail.com"));
            email.Subject = "Test";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();

            smtp.Connect(smtpServer, smtpPort, true);
            smtp.Authenticate(smtpLogin, smtpPassword);
            smtp.Send(email);

            smtp.Disconnect(true);
        }
    }
}
