using Business_logic.DTOs;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IGoalServices
    {
        bool CreateGoal(GoalDTO goal);
        List<GoalDTO> GetPersonalGoals();
        void UpdateGoal(GoalDTO goal);
        bool RemoveGoal(int id);
        GoalDTO? GetNewestGoal(bool isIncreasing);
        Task SendRemindersAsync();
    }
}
