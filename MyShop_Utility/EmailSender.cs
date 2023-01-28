using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace MyShop_Utility
{
    public class EmailSender : IEmailSender
    {
        private IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //return Ececute(email,subject,htmlMessage);
            return GmailExecute(email, subject, htmlMessage);
        }
        /*public async Task Ececute(string email, string subject, string htmlmessage)
        {
            MailJetSettings mailJetSettings =_configuration.GetSection("MailJet").Get<MailJetSettings>();
            MailjetClient client = new MailjetClient(mailJetSettings.ApiKey,
               mailJetSettings.SecretKey);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            };


            var emailMessage = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("viosagmir@gmail.com"))
                .WithSubject(subject)
                .WithHtmlPart(htmlmessage)
                .WithTo(new SendContact(email))
                .Build();

            var response = await client.SendTransactionalEmailAsync(emailMessage);
        }*/
        public async Task GmailExecute(string email, string subject, string htmlmessage)
        {
            MailAddress from = new MailAddress(PathManager.Email, "Тимофей Локтионов");
            MailAddress to = new MailAddress(email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = subject;
            m.Body = htmlmessage;
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(PathManager.Email, "tnwojglzznkyyouy");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
            
        }
    }
}
