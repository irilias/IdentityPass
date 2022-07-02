using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CoreServices.Authentication.Descriptions
{
    public interface IJWTAuthenticationService
    {
        Task<string> Authenticate(string webAPILogicalName, string webAPIAuthUri, string userName, string password);
    }
}
