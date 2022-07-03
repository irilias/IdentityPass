using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.Settings
{
    public class SmtpServer
    {
        public string ServerURL { get; set; }
        public string Id { get; set; }
        public string Key { get; set; }
        public int Port { get; set; }
    }
}
