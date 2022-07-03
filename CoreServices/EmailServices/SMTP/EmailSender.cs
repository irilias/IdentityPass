using CoreServices.EmailServices.SMTP.Descriptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CoreServices.EmailServices.SMTP
{
    public class EmailSender : IEmailSender
    {
        public async Task<bool> SendEmail(string emailJson)
        {
            var email = JsonConvert.DeserializeObject<Email>(emailJson);

            // TODO : Validation the email entity using the Chain of responsibility pattern 
            // https://levelup.gitconnected.com/validation-using-the-chain-of-responsibility-pattern-236a6ded7078
            bool isSuccess;
            var mail = new MailMessage(email.SenderEmail
                , email.RecipientEmail
                , email.Subject
                , email.Body);
            using (var emailClient = new SmtpClient(email.ServerUrl
                , email.ServerPort))
            {
                emailClient.Credentials = new NetworkCredential()
                {
                    UserName = email.SenderEmail,
                    Password = email.ServerPassword
                };
                await emailClient.SendMailAsync(mail);
                isSuccess = true;
            }
            return isSuccess;
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
}
