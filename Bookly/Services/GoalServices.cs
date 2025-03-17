using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Services
{
    public class GoalServices
    {
        private readonly GoalRepository _goalRepo;
        private readonly ShelfServices _shelfService;
        private readonly IHttpContextAccessor _contextAccessor;
        public GoalServices(GoalRepository goalRepo, ShelfServices shelfServices, IHttpContextAccessor contextAccessor)
        {
            this._goalRepo = goalRepo;
            this._shelfService = shelfServices;
            this._contextAccessor = contextAccessor; 
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
            Goal? goal = _goalRepo.GetNewestGoal();
            int progress = _shelfService.GetProgress();
            if (goal!=null)
            {
                if (progress > 0 && progress < goal.ReadingGoal)
                {
                    newStatus = Status.In_progress;
                }
                else if (progress == goal.ReadingGoal)
                {
                    newStatus = Status.Completed;
                    _contextAccessor.HttpContext?.Session.SetInt32("Progress", 0);
                }
                _goalRepo.UpdateProgress(userId, goal.Id, progress, newStatus);
            }
        }
    }
}
