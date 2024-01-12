using FluentAssertions;
using MailSedningService.Dto;
using MailSedningService.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MailSendingServiceTests.Services
{
    public class MailSendingServiceTests
    {

        private readonly MailService _mailService;

        public MailSendingServiceTests()
        {
            _mailService = new MailService(
                new Mock<ILogger<MailService>>().Object
                );
        }

        [Fact]
        public void MailSendingService_ShouldNot_ThrowAnyError_WhenEnvironmentVariablesSetted()
        {
            Environment.SetEnvironmentVariable("sendingEmail", "fakemail@gmial.com");
            Environment.SetEnvironmentVariable("smtpServer", "serverfake.com");
            Environment.SetEnvironmentVariable("smtpPort", "111");
            Environment.SetEnvironmentVariable("smtpLogin", "fakeLogin");
            Environment.SetEnvironmentVariable("smtpPassword", "fakePwd");

            List<string> emails = new();

            MailToSendDto mailToSend = new(emails, "simpleContent", "simpleSubject");

            _mailService.Invoking(x => x.SendMail(mailToSend))
                  .Should()
                  .NotThrow();
        }

        [Fact]
        public void MailSendingService_Should_ThrowError_WhenEnvironmentVariablesNotSetted()
        {
            List<string> emails = new();

            MailToSendDto mailToSend = new(emails, "simpleContent", "simpleSubject");

            _mailService.Invoking(x => x.SendMail(mailToSend))
                  .Should()
                  .Throw<Exception>();
        }
    }
}
