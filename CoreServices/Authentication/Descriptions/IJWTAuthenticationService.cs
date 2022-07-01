using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CoreServices.Authentication.Descriptions
{
    public interface IJWTAuthenticationService
    {
        string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt, string secretKey);
    }
}
