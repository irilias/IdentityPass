using CoreServices.EmailServices.SMTP.Descriptions;
using IdentityPass.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AspNetIdentity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IEmailSender emailSender;

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }

        public RegisterModel(UserManager<IdentityUser> userManager
            , IConfiguration configuration
            , IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.emailSender = emailSender;
        }
        public void OnGet()
        {
            RegisterViewModel = new RegisterViewModel();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = new IdentityUser()
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email
            };
            var createUserResult = await userManager.CreateAsync(user, RegisterViewModel.Password);

            if (createUserResult.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                bool hasEmailBeenSentSuccessfully = await SendConfirmationEmail(user, token);
                if(hasEmailBeenSentSuccessfully)
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
            var smtpConfig = configuration.GetSection(Constants.SMTP);
            var confirmationLink = Url.PageLink("EmailConfirmed", values: new { userId = user.Id, token });
            var sendInBlueSmtpServer = smtpConfig.GetSection(Constants.SMTPServerName);
            var email = new Email()
            {
                RecipientEmail = user.Email,
                SenderEmail = smtpConfig.GetValue<string>(Constants.SenderEmail),
                Subject = "IdentityPass - Please Confirm your Email",
                Body = $"Please click on this link to confirm your email : \n {confirmationLink}",
                ServerUrl = sendInBlueSmtpServer.GetValue<string>(Constants.SMTPServerUrl),
                ServerPort = sendInBlueSmtpServer.GetValue<int>(Constants.SMTPServerPort),
                ServerUsername = smtpConfig.GetValue<string>(Constants.SenderEmail),
                ServerPassword = sendInBlueSmtpServer.GetValue<string>(Constants.SMTPKey)
            };
            var emailString = JsonConvert.SerializeObject(email);
            return await emailSender.SendEmail(emailString);
            //bool isSuccess;
            //var mail = new MailMessage(smtpConfig.GetValue<string>(Constants.SenderEmail)
            //    , user.Email
            //    , "IdentityPass - Please Confirm your Email"
            //    , $"Please click on this link to confirm your email : \n {confirmationLink}");
            //using (var emailClient = new SmtpClient(sendInBlueSmtpServer.GetValue<string>(Constants.SMTPServerUrl)
            //    , sendInBlueSmtpServer.GetValue<int>(Constants.SMTPServerPort)))
            //{
            //    emailClient.Credentials = new NetworkCredential()
            //    {
            //        UserName = smtpConfig.GetValue<string>(Constants.SenderEmail),
            //        Password = sendInBlueSmtpServer.GetValue<string>(Constants.SMTPKey)
            //    };
            //    await emailClient.SendMailAsync(mail);
            //    isSuccess = true;
            //}
            //return isSuccess;
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

        public string Message { get; set; }
    }
}