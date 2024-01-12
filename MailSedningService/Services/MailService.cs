using MailKit.Net.Smtp;
using MailSedningService.Dto;
using MailSedningService.Interfaces;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSedningService.Services
{
    public class MailService : IMailService
    {

        private readonly ILogger<MailService> _logger;
        
        public MailService(ILogger<MailService> logger)
        {
            _logger = logger;
        }

        public void SendMail(MailToSendDto mailToSend)
        {
            var sendingEmail = Environment.GetEnvironmentVariable("sendingEmail");
            var smtpServer = Environment.GetEnvironmentVariable("smtpServer");
            var smtpPort = int.Parse(Environment.GetEnvironmentVariable("smtpPort")!);
            var smtpLogin = Environment.GetEnvironmentVariable("smtpLogin");
            var smtpPassword = Environment.GetEnvironmentVariable("smtpPassword");

            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(sendingEmail));

            foreach(var mail in mailToSend.EmailsList)
            {
                email.To.Add(MailboxAddress.Parse(mail));
            }

            email.Subject = mailToSend.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = mailToSend.MailContent};

            try
            {
                using var smtp = new SmtpClient();

                smtp.Connect(smtpServer, smtpPort, true);
                smtp.Authenticate(smtpLogin, smtpPassword);
                smtp.Send(email);

                smtp.Disconnect(true);
            }
            catch(Exception ex)
            {
                _logger.LogError("Mail sending service exception {ex}", ex);
            }
        }
    }
}
