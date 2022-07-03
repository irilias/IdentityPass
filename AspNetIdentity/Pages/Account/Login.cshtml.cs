using AspNetIdentity.Data.Account;
using AspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using PageModel = Microsoft.AspNetCore.Mvc.RazorPages.PageModel;

namespace AspNetIdentity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> signInManager;

        [BindProperty]
        public Credential Credential { get; set; }

        public LoginModel(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await signInManager.PasswordSignInAsync(Credential.Email
                , Credential.Password
                , Credential.RememberMe
                , lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return RedirectToPage("/index");
            }
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("LoginLockedOut", "You have been locked out. " +
                    "Please wait 30 seconds before trying again.");
            }
            else
            {
                ModelState.AddModelError("LoginFailed", "Login Failed!");
            }
            return Page();
        }
    }
}