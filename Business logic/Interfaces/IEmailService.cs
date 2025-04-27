using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IEmailService
    {
        Task SendGoalReminderEmail(GoalDTO goal);
    }
}
