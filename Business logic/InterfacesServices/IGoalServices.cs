using Bookly.Data.Models;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IGoalServices
    {
        bool CreateGoal(Goal goal, int userId);
        List<Goal> GetPersonalGoals(int id);
        void RemoveGoal(int id);
    }
}
