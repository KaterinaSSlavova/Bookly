namespace Bookly.Business_logic.InterfacesServices
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string email, string message);
    }
}
