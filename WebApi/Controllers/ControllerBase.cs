using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    public class ControllerBase : Microsoft.AspNetCore.Mvc.Controller
    {
        // If Public, i get  :
        // Microsoft.AspNetCore.Routing.Matching.AmbiguousMatchException: The request matched multiple endpoints.Matches: 
        // WebApi.Controllers.AuthenticationController.OnPost(WebApi)
        // WebApi.Controllers.AuthenticationController.CreateToken(WebApi)
        // The routing system seem to detect CreateToken as an endpoint, so to prevent that i changed it to : protected
        protected string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt, string secretKey)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);
            var jwt = new JwtSecurityToken(
               claims: claims,
               notBefore: DateTime.UtcNow,
               expires: expiresAt,
               signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key)
               , SecurityAlgorithms.HmacSha256Signature)
               );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}
