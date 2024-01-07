using MailSedningService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSedningService.Interfaces
{
    public interface IMailService
    {
        public void SendMail(MailToSendDto mailToSend);
    }
}
