using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Models.Enums;
using Business_logic.DTOs;

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

        public bool CreateGoal(GoalDTO goalDTO)
        {
            if(!ValidateGoal(goalDTO))
            {
                return false;
            }
            goalDTO.User = GetUser();
            Goal goal = _mapper.Map<Goal>(goalDTO);
            return _goalRepo.CreateGoal(goal);
        }

        public List<GoalDTO> GetPersonalGoals()
        {
            User user = GetUser();
            List<Goal> goals = _goalRepo.GetPersonalGoals(user);
            List<GoalDTO> goalDTOs = _mapper.Map<List<GoalDTO>>(goals);
            CheckForExpired(goalDTOs);
            goals = _goalRepo.GetPersonalGoals(user);
            return _mapper.Map<List<GoalDTO>>(goals);
        }

        public bool RemoveGoal(int id)
        {
            return _goalRepo.RemoveGoal(id);
        }

        public GoalDTO? GetNewestGoal(bool isIncreasing)
        {
            User user = GetUser();
            Goal? goal = _goalRepo.GetNewestGoal(isIncreasing, user);
            if (goal == null && !isIncreasing)
            {
                goal = _goalRepo.GetLatestCompletedGoal(user);
            }
            GoalDTO goalDTO = _mapper.Map<GoalDTO>(goal);
            return goalDTO;
        }

        private void UpdateProgress(GoalDTO goalDTO)
        {
            User user = GetUser();
            Goal goal = _mapper.Map<Goal>(goalDTO);
            _goalRepo.UpdateProgress(user.Id, goal);
        }
        private void UpdateStatus(Status status, int goalId)
        {
            User user = GetUser();
            _goalRepo.UpdateStatus(status, goalId, user.Id);
        }

        public void UpdateGoal(GoalDTO goal)
        {
            Status newStatus = Status.Not_started;
            UpdateProgress(goal);

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

        private bool ValidateGoal(GoalDTO goal)
        {
            if (goal == null) return false;
            if (goal.ReadingGoal <= 0) return false;
            if (goal.Start > goal.End) return false;
            if (goal.End < DateTime.Now) return false;

            return true;
        }

        private void CheckForExpired(List<GoalDTO> goals)
        {
            foreach (GoalDTO goal in goals)
            {
                if(goal.End < DateTime.Now)
                {
                    UpdateStatus(Status.Expired, goal.Id);
                }
            }
        }

        private User GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
