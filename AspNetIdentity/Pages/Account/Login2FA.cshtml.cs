using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AspNetIdentity.Data.Account;
using AspNetIdentity.Settings;
using CoreServices.EmailServices.SMTP.Descriptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspNetIdentity.Pages.Account
{
    public class Login2FAModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IEmailSender emailSender;
        private readonly IOptions<SmtpSetting> smtpOptions;

        [BindProperty]
        public Email2FA Email2FA { get; set; }

        public Login2FAModel(UserManager<User> userManager
            , SignInManager<User> signInManager
            , IEmailSender emailSender
            , IOptions<SmtpSetting> smtpOptions)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.emailSender = emailSender;
            this.smtpOptions = smtpOptions;
            //this.Email2FA = new Email2FA();
        }
        public async Task OnGet(string email, bool rememberMe)
        {
            var user = await userManager.FindByEmailAsync(email);
            var token = await userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);
            Email2FA = new Email2FA()
            {
                RememberMe = rememberMe,
                VerificationToken = String.Empty
            };
        // TODO : Refactor this to a seperate service, because you're using the same code during registration
        var smtpSetting = smtpOptions.Value;
            var smtpServer = smtpSetting.SmtpServer;
            var emailMessage = new Email()
            {
                RecipientEmail = user.Email,
                SenderEmail = smtpSetting.SenderEmail,
                Subject = "IdentityPass - OTP Confirmation",
                Body = $"Please confirm your login by using the following code : \n {token}",
                ServerUrl = smtpServer.ServerURL,
                ServerPort = smtpServer.Port,
                ServerUsername = smtpServer.Id,
                ServerPassword = smtpServer.Key
            };
            var emailMessageString = JsonConvert.SerializeObject(emailMessage);
            await emailSender.SendEmail(emailMessageString);

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var result = await signInManager.TwoFactorSignInAsync(TokenOptions.DefaultEmailProvider
                , Email2FA.VerificationToken
                , Email2FA.RememberMe
                , rememberClient: false);
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

    public class Email2FA
    {
        [Required]
        [Display(Name = "Verification Code")]
        public string VerificationToken { get;  set; }
        public bool RememberMe { get; set; }
    }
}