using AspNetIdentity.Data.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CustomClaimTypes = AspNetIdentity.Authorization.ClaimTypes;
namespace AspNetIdentity.Pages.Account
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly UserManager<User> userManager;

        [BindProperty]
        public UserProfileViewModel UserProfileViewModel { get; set; }

        [BindProperty]
        public string SuccessMessage { get; set; }

        public UserProfileModel(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task OnGet()
        {
            try
            {
                var (user, departmentClaim, positionClaim) = await GetClaimsAsync();
                UserProfileViewModel = new UserProfileViewModel()
                {
                    Email = user.Email,
                    Department = departmentClaim.Value,
                    Position = positionClaim.Value
                };
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("GetUserProfile", "An error occured while retrieving your profile.");
            }
           
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var (user, departmentClaim, positionClaim) = await GetClaimsAsync();
                await userManager.ReplaceClaimAsync(user, departmentClaim
                    , new Claim(departmentClaim.Type, UserProfileViewModel.Department));
                await userManager.ReplaceClaimAsync(user, positionClaim
                    , new Claim(positionClaim.Type, UserProfileViewModel.Position));
                SuccessMessage = "Your profile has been updated successfully.";
            }
            catch (System.Exception)
            {

                ModelState.AddModelError("GetUserProfile", "An error occured while updating your profile.");

            }
            return Page();
        }

        private async Task<(User, Claim, Claim)> GetClaimsAsync()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var claims = await userManager.GetClaimsAsync(user);

            return (user
                , claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Department)
                , claims.FirstOrDefault(c => c.Type == CustomClaimTypes.Position));
        }
    }

    public class UserProfileViewModel
    {
        public string Email { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }
    }
}