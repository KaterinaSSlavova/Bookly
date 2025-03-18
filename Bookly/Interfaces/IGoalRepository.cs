using Bookly.Models;

namespace Bookly.Interfaces
{
    public interface IGoalRepository
    {
        bool CreateGoal(Goal goal, int userId);
        List<Goal> GetPersonalGoals(int id);
        Goal? GetNewestGoal();
        void UpdateProgress(int userId, int goalId, int progress, Status status);
        void UpdateStatus(Status status, int goalId, int userId);
        void RemoveGoal(int id);

    }
}
