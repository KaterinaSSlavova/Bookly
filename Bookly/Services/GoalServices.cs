using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Services
{
    public class GoalServices
    {
        private readonly GoalRepository _goalRepo;
        public GoalServices(GoalRepository goalRepo)
        {
            _goalRepo = goalRepo;
        }

        public bool CreateGoal(Goal goal, int userId)
        {
            return _goalRepo.CreateGoal(goal, userId);
        }

        public List<Goal> GetPersonalGoals(int id)
        {
            return _goalRepo.GetPersonalGoals(id);
        }

        public void RemoveGoal(int id)
        {
            _goalRepo.RemoveGoal(id);
        }
    }
}
