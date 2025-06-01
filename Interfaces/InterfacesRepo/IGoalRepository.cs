using Models.Entities;
using Models.Enums;

namespace Interfaces
{
    public interface IGoalRepository
    {
        void CreateGoal(Goal goal);
        List<Goal> GetPersonalGoals(User user);
        Goal? GetNewestGoal(bool isIncreasing, User user);
        Goal? GetLatestCompletedGoal(User user);
        void UpdateGoal(Goal goal);
        void RemoveGoal(int id);
        Task<List<Goal>> GetAllGoals();
    }
}
