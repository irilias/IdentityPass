using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreServices.EmailServices.SMTP.Descriptions
{
    public interface IEmailSender
    {
        Task<bool> SendEmail(string emailJson);
    }
}
