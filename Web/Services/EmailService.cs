using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Interactive.HateBin.Services
{
    public class EmailService
    {
        public Task SendMail(string recipent, string message, Attachment attachment = null)
        {
            var mail = new MailMessage
            {
                From = new MailAddress(Configuration.EmailAddress),
                Subject = Configuration.EmailSubject,
                Body = message,
                IsBodyHtml = true
            };
            mail.To.Add(recipent);
            if (attachment != null)
            {
                mail.Attachments.Add(attachment);
            }

            var smtp = new SmtpClient(Configuration.EmailServer)
            {
                Credentials = new NetworkCredential(Configuration.EmailAddress, Configuration.EmailPassword),
#if(DEBUG)
                Port = 587
#endif
            };

            return smtp.SendMailAsync(mail);
        }
    }
}