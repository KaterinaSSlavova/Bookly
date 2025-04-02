using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Microsoft.AspNetCore.Http;
using Models.Enums;
using AutoMapper;
using ViewModels.Model;

namespace Bookly.Business_logic.Services
{
    public class GoalServices : IGoalServices
    {
        private readonly IGoalRepository _igoalRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        public GoalServices(IGoalRepository igoalRepo, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            this._igoalRepo = igoalRepo;
            this._contextAccessor = contextAccessor;
            this._mapper = mapper;
        }

        public bool CreateGoal(GoalViewModel goalModel, int userId)
        {
            Goal goal = _mapper.Map<Goal>(goalModel);
            return _igoalRepo.CreateGoal(goal, userId);
        }

        public List<GoalViewModel> GetPersonalGoals(int id)
        {
            List<GoalViewModel> goals = new List<GoalViewModel>();
            foreach (Goal goal in _igoalRepo.GetPersonalGoals(id))
            {
                goals.Add(_mapper.Map<GoalViewModel>(goal));
            }
            return goals;
        }

        public void RemoveGoal(int id)
        {
            _igoalRepo.RemoveGoal(id);
        }

        public GoalViewModel? GetNewestGoal(bool isIncreasing)
        {
            GoalViewModel goal = _mapper.Map<GoalViewModel>(_igoalRepo.GetNewestGoal(isIncreasing));
            return goal;
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
