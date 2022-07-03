using AspNetIdentity.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AspNetIdentity.Pages.Account
{
    public class EmailConfirmedModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public string Message { get; set; }

        public EmailConfirmedModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> OnGet(string userId, string token)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var identityResult = await userManager.ConfirmEmailAsync(user, token);
                if (identityResult.Succeeded)
                {
                    Message = "Confirmation has been done successfully! You can go ahead and login.";
                }
                else
                {
                    Message = "Confirmation has failed.";
                }
                return Page();
            }
            Message = "User not found.";
            return Page();
        }
    }
}