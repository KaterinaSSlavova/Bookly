using Bookly.Models;

namespace Bookly.Interfaces
{
    public interface IGoalServices
    {
        bool CreateGoal(Goal goal, int userId);
        List<Goal> GetPersonalGoals(int id);
        void RemoveGoal(int id);
        void UpdateGoalProgress(int userId);
    }
}
