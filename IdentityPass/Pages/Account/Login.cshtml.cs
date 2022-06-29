using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityPass.Services.Descriptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityPass.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IIdentityService identityService;
        private readonly IFileService fileService;
        private const string authenticationScheme = "CookieAuth";
        [BindProperty]
        public Credential Credential { get; set; }

        public LoginModel(IIdentityService identityService, IFileService fileService)
        {
            this.identityService = identityService;
            this.fileService = fileService;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            var users = await fileService.ParseJsonToObjects<Credential>("users.json");
            var currentUser = users.SingleOrDefault(u => u.Username == Credential.Username
            && u.Password == Credential.Password);
            if (currentUser is null)
            {
                ModelState.AddModelError($"{Credential.Username}", "User not found!");
                return Page();
            }

            var claimsPrincipal = identityService.GetClaimsPrincipal(currentUser.Username, authenticationScheme);
            
            // Serialize the principal, encrypt it and save it as a cookie into the HttpContext.
            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
            return RedirectToPage("/Index");
        }
    }

    public class Credential
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}