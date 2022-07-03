using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.Settings
{
    public class SmtpSetting
    {
        public const string SMTP = "SMTP";
        public string SenderEmail { get; set; }
        public SmtpServer SmtpServer { get; set; }
    }
}
