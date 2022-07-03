using AspNetIdentity.Data.Account;
using AspNetIdentity.Settings;
using CoreServices.EmailServices.SMTP.Descriptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetIdentity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IEmailSender emailSender;
        private readonly IOptions<SmtpSetting> smtpOptions;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<User> userManager
            , IEmailSender emailSender
            , IOptions<SmtpSetting> smtpOptions)
        {
            this.userManager = userManager;
            this.emailSender = emailSender;
            this.smtpOptions = smtpOptions;
        }

        public void OnGet()
        {
            RegisterViewModel = new RegisterViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = new User()
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email,
                Department = RegisterViewModel.Department,
                Position = RegisterViewModel.Position
            };
            var createUserResult = await userManager.CreateAsync(user, RegisterViewModel.Password);

            if (createUserResult.Succeeded)
            {
                var departmentClaim = new Claim(nameof(RegisterViewModel.Department)
                    , RegisterViewModel.Department);
                var positionClaim = new Claim(nameof(RegisterViewModel.Position)
                    , RegisterViewModel.Position);
                await userManager.AddClaimsAsync(user, new List<Claim>()
                {
                    departmentClaim, positionClaim
                });
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                bool hasEmailBeenSentSuccessfully = await SendConfirmationEmail(user, token);
                if (hasEmailBeenSentSuccessfully)
                {
                    RegisterViewModel.Message = "Please confirm your e-mail to successfully register.";
                }
                else
                {
                    ModelState.AddModelError("SMTPError", "An Error has occured while registering. \n " +
                        "Please try again later.");
                }

                return Page();
            }
            else
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError("RegisteringError", error.Description);
                }
            }
            return Page();
        }

        private async Task<bool> SendConfirmationEmail(IdentityUser user, string token)
        {
            var smtpSetting = smtpOptions.Value;
            var smtpServer = smtpSetting.SmtpServer;
            var confirmationLink = Url.PageLink("EmailConfirmed", values: new { userId = user.Id, token });
            var email = new Email()
            {
                RecipientEmail = user.Email,
                SenderEmail = smtpSetting.SenderEmail,
                Subject = "IdentityPass - Please Confirm your Email",
                Body = $"Please click on this link to confirm your email : \n {confirmationLink}",
                ServerUrl = smtpServer.ServerURL,
                ServerPort = smtpServer.Port,
                ServerUsername = smtpServer.Id,
                ServerPassword = smtpServer.Key
            };
            var emailString = JsonConvert.SerializeObject(email);
            return await emailSender.SendEmail(emailString);
        }
    }

    public class Email
    {
        public string SenderEmail { get; set; }
        public string RecipientEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string ServerUrl { get; set; }
        public int ServerPort { get; set; }
        public string ServerUsername { get; set; }
        public string ServerPassword { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }

        public string Message { get; set; }
    }
}