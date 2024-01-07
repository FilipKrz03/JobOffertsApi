using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailSedningService.Dto
{
    public record MailToSendDto(IEnumerable<string> EmailsList , string MailContent , string Subject) { }
}
