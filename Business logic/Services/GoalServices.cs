using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Models.Enums;
using AutoMapper;
using ViewModels.Model;

namespace Bookly.Business_logic.Services
{
    public class GoalServices : IGoalServices
    {
        private readonly IGoalRepository _igoalRepo;
        private readonly IUserServices _userServices;   
        private readonly IMapper _mapper;
        public GoalServices(IGoalRepository igoalRepo, IMapper mapper, IUserServices userServices)
        {
            this._igoalRepo = igoalRepo;
            this._mapper = mapper;
            this._userServices = userServices;
        }

        public bool CreateGoal(GoalViewModel goalModel)
        {
            Goal goal = _mapper.Map<Goal>(goalModel);
            goal.SetUser(GetUser());
            return _igoalRepo.CreateGoal(goal);
        }

        public List<GoalViewModel> GetPersonalGoals()
        {
            User user = GetUser();
            List<Goal> goals = _igoalRepo.GetPersonalGoals(user);
            List<GoalViewModel> goalsModel = _mapper.Map<List<GoalViewModel>>(goals);
            return goalsModel;
        }

        public void RemoveGoal(int id)
        {
            _igoalRepo.RemoveGoal(id);
        }

        public Goal? GetNewestGoal(bool isIncreasing)
        {
            User user = GetUser();
            Goal goal = _igoalRepo.GetNewestGoal(isIncreasing, user);
            return goal;
        }

        public void UpdateProgress(int goalId, int progress)
        {
            User user = GetUser();
            _igoalRepo.UpdateProgress(user.Id, goalId, progress);    
        }
        public void UpdateStatus(Status status, int goalId)
        {
            User user = GetUser();
            _igoalRepo.UpdateStatus(status, goalId, user.Id);    
        }

        private User GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
