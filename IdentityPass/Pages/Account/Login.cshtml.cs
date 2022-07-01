using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreServices.FileServices.Descriptions;
using IdentityPass.Models;
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

            var users = await fileService.ParseJsonToObjects<Credential>(Constants.UsersJsonFilePath);
            var currentUser = users.SingleOrDefault(u => u.Username == Credential.Username
            && u.Password == Credential.Password);
            if (currentUser is null)
            {
                ModelState.AddModelError(nameof(Credential), "User not found!");
                return Page();
            }

            var claimsPrincipal = identityService.GetClaimsPrincipal(currentUser, Constants.CookieAuthScheme);

            var authenticationProperties = new AuthenticationProperties()
            {
                IsPersistent = Credential.RememberMe
            };
            // Serialize the principal, encrypt it and save it as a cookie into the HttpContext.
            await HttpContext.SignInAsync(Constants.CookieAuthScheme, claimsPrincipal, authenticationProperties);
            return RedirectToPage("/Index");
        }
    }


}