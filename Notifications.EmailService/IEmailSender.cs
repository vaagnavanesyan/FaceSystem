using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notifications.EmailService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(Message message);
    }

}
