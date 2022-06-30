using IdentityPass.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace IdentityPass.Pages.Admin
{
    [Authorize(Policy = Policies.OnlyAdmin)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}