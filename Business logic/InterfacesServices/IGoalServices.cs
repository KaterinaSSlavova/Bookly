using Models.Entities;
using Models.Enums;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IGoalServices
    {
        bool CreateGoal(Goal goal, int userId);
        List<Goal> GetPersonalGoals(int id);
        void RemoveGoal(int id);
        void UpdateProgress(int userId, int goalId, int progress);
        void UpdateStatus(Status status, int goalId, int userId);
        Goal? GetNewestGoal(bool isIncreasing);

    }
}
