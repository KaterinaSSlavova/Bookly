using Models.Entities;
using Models.Enums;

namespace Bookly.Data.InterfacesRepo
{
    public interface IGoalRepository
    {
        bool CreateGoal(Goal goal);
        List<Goal> GetPersonalGoals(User user);
        Goal? GetNewestGoal(bool isIncreasing, User user);
        Goal? GetLatestCompletedGoal(User user);
        void UpdateProgress(int userId, Goal goal);
        void UpdateStatus(Status status, int goalId, int userId);
        Goal GetGoalById(User user, int goalId);
        bool RemoveGoal(int id);
        Task<List<Goal>> GetAllGoals();

    }
}
