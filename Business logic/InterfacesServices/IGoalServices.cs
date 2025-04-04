using Models.Enums;
using Models.Entities;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IGoalServices
    {
        bool CreateGoal(GoalViewModel goal);
        List<GoalViewModel> GetPersonalGoals();
        void RemoveGoal(int id);
        void UpdateProgress(int goalId, int progress);
        void UpdateStatus(Status status, int goalId);
        Goal? GetNewestGoal(bool isIncreasing);

    }
}
