using Business_logic.DTOs;

namespace Interfaces
{
    public interface IEmailService
    {
        Task SendGoalReminderEmail(GoalDTO goal);
    }
}
