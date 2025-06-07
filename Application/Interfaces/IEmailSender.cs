namespace Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string subject, string email, string message);
    }
}
