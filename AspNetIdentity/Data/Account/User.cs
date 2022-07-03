using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetIdentity.Data.Account
{
    public class User : IdentityUser
    {
        public string Department { get; internal set; }
        public string Position { get; internal set; }
    }
}
