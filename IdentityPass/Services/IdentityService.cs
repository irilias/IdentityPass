using IdentityPass.Services.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityPass.Services
{
    public class IdentityService : IIdentityService
    {
        public ClaimsPrincipal GetClaimsPrincipal(string userName, string authScheme)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,userName),
                new Claim(ClaimTypes.Email,$"{userName}@identitypass.io")
            };
            var identity = new ClaimsIdentity(claims,$"{authScheme}");
            return new ClaimsPrincipal(identity);
        }
    }
}
