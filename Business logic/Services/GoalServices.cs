using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.Models;
using Bookly.Data.InterfacesRepo;
using Microsoft.AspNetCore.Http;

namespace Bookly.Business_logic.Services
{
    public class GoalServices: IGoalServices
    {
        private readonly IGoalRepository _igoalRepo;
        private readonly IShelfServices _ishelfService;
        private readonly IHttpContextAccessor _contextAccessor;
        public GoalServices(IGoalRepository igoalRepo, IShelfServices ishelfServices, IHttpContextAccessor contextAccessor)
        {
            this._igoalRepo = igoalRepo;
            this._ishelfService = ishelfServices;
            this._contextAccessor = contextAccessor; 
        }

        public bool CreateGoal(Goal goal, int userId)
        {
            return _igoalRepo.CreateGoal(goal, userId);
        }

        public List<Goal> GetPersonalGoals(int id)
        {
            return _igoalRepo.GetPersonalGoals(id);
        }

        public void RemoveGoal(int id)
        {
            _igoalRepo.RemoveGoal(id);
        }

        public Goal? GetNewestGoal()
        {
            return _igoalRepo.GetNewestGoal();
        }

        public void UpdateGoalProgress(int userId)
        {
            Status newStatus = Status.Not_started;
            Goal? goal = _igoalRepo.GetNewestGoal();
            int progress = _ishelfService.GetProgress();
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
                _igoalRepo.UpdateProgress(userId, goal.Id, progress, newStatus);
            }
        }
    }
}
