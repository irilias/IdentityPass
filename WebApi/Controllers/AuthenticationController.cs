using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CoreServices.FileServices.Descriptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BuiltInClaims = System.Security.Claims.ClaimTypes;
using CustomClaims = WebApi.Authorization.ClaimTypes;
namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IFileService fileService;
        private readonly IConfiguration configuration;

        public AuthenticationController(IFileService fileService
            , IConfiguration configuration)
        {
            this.fileService = fileService;
            this.configuration = configuration;
        }
        public async Task<IActionResult> OnPost([FromBody] Credential credential)
        {

            var users = await fileService.ParseJsonToObjects<Credential>("users.json");
            var currentUser = users.SingleOrDefault(u => u.Username == credential.Username
            && u.Password == credential.Password);
            if (currentUser is null)
            {
                ModelState.AddModelError("Unauthorized","You are not authorized to access this endpoint.");
                return Unauthorized(ModelState);
            }

            var claims = new List<Claim>
            {
                new Claim(BuiltInClaims.Name,currentUser.Username),
                new Claim(BuiltInClaims.Email,$"{currentUser.Username}@identitypass.io"),
                new Claim(CustomClaims.IsAdmin, currentUser.IsAdmin.ToString()),
                new Claim(CustomClaims.IsHR, currentUser.IsHR.ToString()),
                new Claim(CustomClaims.EmployementDate, currentUser.EmploymentDate.ToString()),
            };
            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            var secretKey = Encoding.ASCII.GetBytes(configuration.GetValue<string>("SecretKey"));
            return Ok(new
            {
                access_token = CreateToken(claims
                ,expiresAt
                , configuration.GetValue<string>("SecretKey")),
                expires_at = expiresAt
            });
        }
       
    }

  
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsHR { get; set; }
        public DateTime EmploymentDate { get; set; }
    }
}