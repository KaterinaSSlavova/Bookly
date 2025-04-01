using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Microsoft.AspNetCore.Http;
using Models.Enums;

namespace Bookly.Business_logic.Services
{
    public class GoalServices : IGoalServices
    {
        private readonly IGoalRepository _igoalRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        public GoalServices(IGoalRepository igoalRepo, IHttpContextAccessor contextAccessor)
        {
            this._igoalRepo = igoalRepo;
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

        public Goal? GetNewestGoal(bool isIncreasing)
        {
            return _igoalRepo.GetNewestGoal(isIncreasing);
        }

        public void UpdateProgress(int userId, int goalId, int progress)
        {
            _igoalRepo.UpdateProgress(userId, goalId, progress);    
        }
        public void UpdateStatus(Status status, int goalId, int userId)
        {
            _igoalRepo.UpdateStatus(status, goalId, userId);    
        }
    }
}
