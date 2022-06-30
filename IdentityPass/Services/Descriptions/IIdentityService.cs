using IdentityPass.Models;
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
        /// <param name="currentUser">Current Logged in user.</param>
        /// <param name="authScheme">The authentication Scheme / Type</param>
        /// <returns></returns>
        ClaimsPrincipal GetClaimsPrincipal(Credential currentUser, string authScheme);
    }
}
