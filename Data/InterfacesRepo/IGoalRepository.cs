using Models.Entities;
using Models.Enums;

namespace Bookly.Data.InterfacesRepo
{
    public interface IGoalRepository
    {
        bool CreateGoal(Goal goal);
        List<Goal> GetPersonalGoals(User user);
        Goal? GetNewestGoal(bool isIncreasing, User user);
        void UpdateProgress(int userId, int goalId, int progress);
        void UpdateStatus(Status status, int goalId, int userId);
        void RemoveGoal(int id);

    }
}
