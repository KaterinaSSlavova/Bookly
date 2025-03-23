using Bookly.Data.Models;

namespace Bookly.Data.InterfacesRepo
{
    public interface IGoalRepository
    {
        bool CreateGoal(Goal goal, int userId);
        List<Goal> GetPersonalGoals(int id);
        Goal? GetNewestGoal(bool isIncreasing);
        void UpdateProgress(int userId, int goalId, int progress);
        void UpdateStatus(Status status, int goalId, int userId);
        void RemoveGoal(int id);

    }
}
