using IdentityPass.Models;
using IdentityPass.Services.Descriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using BuiltInClaims = System.Security.Claims.ClaimTypes;
using CustomClaims = IdentityPass.Authorization.ClaimTypes;
using System.Threading.Tasks;

namespace IdentityPass.Services
{
    public class IdentityService : IIdentityService
    {
        public ClaimsPrincipal GetClaimsPrincipal(Credential currentUser, string authScheme)
        {
            var claims = new List<Claim>
            {
                new Claim(BuiltInClaims.Name,currentUser.Username),
                new Claim(BuiltInClaims.Email,$"{currentUser.Username}@identitypass.io"),
                new Claim(CustomClaims.IsAdmin, currentUser.IsAdmin.ToString()),
                new Claim(CustomClaims.IsHR, currentUser.IsHR.ToString()),
                new Claim(CustomClaims.EmployementDate, currentUser.EmploymentDate.ToString()),
            };
            var identity = new ClaimsIdentity(claims,$"{authScheme}");
            return new ClaimsPrincipal(identity);
        }
    }
}
