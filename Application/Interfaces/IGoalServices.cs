using Business_logic.DTOs;

namespace Interfaces
{
    public interface IGoalServices
    {
        void CreateGoal(GoalDTO goal);
        List<GoalDTO> GetPersonalGoals();
        void UpdateGoal(GoalDTO goal);
        void RemoveGoal(int id);
        GoalDTO? GetNewestGoal(bool isIncreasing, UserDTO user = null);
        Task SendRemindersAsync();
        void IncreaseProgress(UserDTO user = null);
        void DecreaseProgress();
    }
}
