using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Services
{
    public class GoalServices
    {
        private readonly GoalRepository _goalRepo;
        private readonly ShelfServices _shelfService;
        public GoalServices(GoalRepository goalRepo, ShelfServices shelfServices)
        {
            this._goalRepo = goalRepo;
            this._shelfService = shelfServices;
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

        public void UpdateGoalProgress(int userId)
        {
            Status newStatus = Status.Not_started;
            int progress = _shelfService.GetProgress();
            Goal? goal = _goalRepo.GetNewestGoal();
           // int progress = amount + goal.CurrentProgress;
            if(goal!=null)
            {
                if (progress > 0 && progress < goal.ReadingGoal)
                {
                    newStatus = Status.In_progress;
                }
                else if (progress == goal.ReadingGoal)
                {
                    newStatus = Status.Completed;
                }
                else
                {
                    newStatus = Status.Not_started;
                }
                _goalRepo.UpdateProgress(userId, goal.Id, progress, newStatus);
            }
        }
    }
}
