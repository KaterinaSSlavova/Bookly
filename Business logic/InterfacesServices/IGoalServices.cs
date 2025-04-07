using Models.Entities;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IGoalServices
    {
        bool CreateGoal(GoalViewModel goal);
        List<GoalViewModel> GetPersonalGoals();
        void SetStatus(Goal goal, int progress);
        bool RemoveGoal(int id);
        Goal? GetNewestGoal(bool isIncreasing);

    }
}
