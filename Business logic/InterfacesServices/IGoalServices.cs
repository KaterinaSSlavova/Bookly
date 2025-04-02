using Models.Entities;
using Models.Enums;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IGoalServices
    {
        bool CreateGoal(GoalViewModel goal, int userId);
        List<GoalViewModel> GetPersonalGoals(int id);
        void RemoveGoal(int id);
        void UpdateProgress(int userId, int goalId, int progress);
        void UpdateStatus(Status status, int goalId, int userId);
        GoalViewModel? GetNewestGoal(bool isIncreasing);

    }
}
