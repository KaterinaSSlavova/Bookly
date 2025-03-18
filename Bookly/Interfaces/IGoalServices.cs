using Bookly.Models;

namespace Bookly.Interfaces
{
    public interface IGoalServices
    {
        bool CreateGoal(Goal goal, int userId);
        List<Goal> GetPersonalGoals(int id);
        void RemoveGoal(int id);
        Goal? GetNewestGoal();
        void UpdateGoalProgress(int userId);
    }
}
