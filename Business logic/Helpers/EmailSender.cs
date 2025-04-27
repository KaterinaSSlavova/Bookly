using System.Net;
using System.Net.Mail;
using Bookly.Business_logic.InterfacesServices;

namespace Business_logic.Helpers
{
    public class EmailSender : IEmailSender
    {
        private const string sender = "booklyapplication@gmail.com";
        private const string password = "cbls voda skeg nfxo";
        public Task SendEmailAsync(string subject, string email, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(sender, password)
            };

            return client.SendMailAsync(
                new MailMessage(from: sender,
                                to: email,
                                subject,
                                message
                    ));
        }
    }
}
