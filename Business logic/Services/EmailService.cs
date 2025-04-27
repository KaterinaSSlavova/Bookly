using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Business_logic.Services
{
    public class EmailService: BackgroundService, IEmailService
    {
        private readonly IServiceProvider _serviceProvider; 
        private readonly IEmailSender _emailSender;

        public EmailService(IServiceProvider serviceProvider, IEmailSender emailSender)
        {
            _serviceProvider = serviceProvider;
            _emailSender = emailSender;
        }

        protected override async Task ExecuteAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var goalService = scope.ServiceProvider.GetRequiredService<IGoalServices>();
                    await goalService.SendRemindersAsync();
                }

                var nextRun = DateTime.Today.AddDays(1).AddHours(8);
                var delay = nextRun - DateTime.Now;

                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, token);
                }
            }
        }

        public async Task SendGoalReminderEmail(GoalDTO goal)
        {
            if (IsDayBeforeEndDate(goal))
            {
                string subject = "Reminder: Your goal ends tomorrow!";
                string message = $"Hi! Your goal is ending on {goal.End:MMMM dd, yyyy}. Make sure you're ready!";
                await _emailSender.SendEmailAsync(subject, goal.User.Email, message);
            }

            if (IsInMiddleOfGoal(goal))
            {
                string subject = "Reminder: You're halfway through your goal!";
                string message = $"Hey! You're halfway through your goal. You have read {goal.ReadingGoal - goal.CurrentProgress} out of {goal.ReadingGoal}. Keep going strong!";
                await _emailSender.SendEmailAsync(subject, goal.User.Email, message);
            }
        }

        private bool IsDayBeforeEndDate(GoalDTO goal)
        {
            DateTime dayBeforeEnd = goal.End.Date.AddDays(-1);
            if (DateTime.Today != dayBeforeEnd) return false;
            return true;
        }

        private bool IsInMiddleOfGoal(GoalDTO goal)
        {
            DateTime startDate = goal.Start.Date;
            DateTime endDate = goal.End.Date;
            int totalDays = (endDate - startDate).Days;
            DateTime middleDate = startDate.AddDays(totalDays / 2);

            if (DateTime.Today != middleDate) return false;
            return true;
        }
    }
}
