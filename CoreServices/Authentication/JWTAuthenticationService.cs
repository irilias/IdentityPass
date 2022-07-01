using CoreServices.Authentication.Descriptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CoreServices.Authentication
{
    public class JWTAuthenticationService : IJWTAuthenticationService
    {
        public string CreateToken(IEnumerable<Claim> claims, DateTime expiresAt, string secretKey)
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
