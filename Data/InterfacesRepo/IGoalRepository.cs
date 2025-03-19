using Bookly.Data.Models;

namespace Bookly.Data.InterfacesRepo
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
