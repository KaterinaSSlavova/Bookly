using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Models.Enums;
using ViewModels.Model;

namespace Bookly.Business_logic.Services
{
    public class GoalServices : IGoalServices
    {
        private readonly IGoalRepository _goalRepo;
        private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        public GoalServices(IGoalRepository goalRepo, IMapper mapper, IUserServices userServices)
        {
            this._goalRepo = goalRepo;
            this._mapper = mapper;
            this._userServices = userServices;
        }

        public bool CreateGoal(Goal goal)
        {
            if(!ValidateGoal(goal))
            {
                return false;
            }
            goal.SetUser(GetUser());
            return _goalRepo.CreateGoal(goal);
        }

        public List<Goal> GetPersonalGoals()
        {
            User user = GetUser();
            List<Goal> goals = _goalRepo.GetPersonalGoals(user);
            CheckForExpired(goals);
            goals = _goalRepo.GetPersonalGoals(user);
            return goals;
        }

        public bool RemoveGoal(int id)
        {
            return _goalRepo.RemoveGoal(id);
        }

        public Goal? GetNewestGoal(bool isIncreasing)
        {
            User user = GetUser();
            Goal? goal = _goalRepo.GetNewestGoal(isIncreasing, user);
            if (goal == null && !isIncreasing)
            {
                goal = _goalRepo.GetLatestCompletedGoal(user);
            }
            return goal;
        }

        private void UpdateProgress(Goal goal, int progress)
        {
            User user = GetUser();
            _goalRepo.UpdateProgress(user.Id, goal, progress);
        }
        private void UpdateStatus(Status status, int goalId)
        {
            User user = GetUser();
            _goalRepo.UpdateStatus(status, goalId, user.Id);
        }

        public void SetStatus(Goal goal, int progress)
        {
            Status newStatus = Status.Not_started;
            if(goal.End < DateTime.Now)
            {
                newStatus = Status.Expired;
            }

            UpdateProgress(goal, progress);
            if (goal.CurrentProgress > 0 && goal.CurrentProgress < goal.ReadingGoal)
            {
                newStatus = Status.In_progress;
            }
            else if (goal.CurrentProgress == goal.ReadingGoal)
            {
                newStatus = Status.Completed;
            }
            UpdateStatus(newStatus, goal.Id);
        }

        private bool ValidateGoal(Goal goal)
        {
            if (goal == null) return false;
            if (goal.ReadingGoal <= 0) return false;
            if (goal.Start > goal.End) return false;
            if (goal.End < DateTime.Now) return false;

            return true;
        }

        private void CheckForExpired(List<Goal> goals)
        {
            foreach (Goal goal in goals)
            {
                SetStatus(goal, goal.CurrentProgress);
            }
        }

        private User GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
