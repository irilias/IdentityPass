using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityPass.Services.Descriptions
{
    public interface IIdentityService
    {
        /// <summary>
        /// Create and return the Claims Principal.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <param name="authScheme">The authentication Scheme / Type</param>
        /// <returns></returns>
        ClaimsPrincipal GetClaimsPrincipal(string userName, string authScheme);
    }
}
