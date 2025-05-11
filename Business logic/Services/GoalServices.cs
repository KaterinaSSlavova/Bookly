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
        private readonly IEmailService _emailService;   

        public GoalServices(IGoalRepository goalRepo, IMapper mapper, IUserServices userServices, IEmailService emailService)
        {
            _goalRepo = goalRepo;
            _mapper = mapper;
            _userServices = userServices;
            _emailService = emailService;   
        }

        public void CreateGoal(GoalDTO goalDTO)
        {
            ValidateGoal(goalDTO);
            goalDTO.User = GetUser();
            Goal goal = ConvertToEntity(goalDTO, goalDTO.User);
           _goalRepo.CreateGoal(goal);
        }

        public List<GoalDTO> GetPersonalGoals()
        {
            User user = _userServices.ConvertToEntity(GetUser());
            List<Goal> goals = _goalRepo.GetPersonalGoals(user);
            CheckForExpired(goals);
            goals = _goalRepo.GetPersonalGoals(user);
            return goals.Select(g => ConvertToDTO(g,g.User)).ToList();
        }

        public void RemoveGoal(int id)
        {
             _goalRepo.RemoveGoal(id);
        }

        public GoalDTO? GetNewestGoal(bool isIncreasing)
        {
            Goal? goal = _goalRepo.GetNewestGoal(isIncreasing, _userServices.ConvertToEntity(GetUser()));
            if (goal == null && !isIncreasing)
            {
                goal = _goalRepo.GetLatestCompletedGoal(_userServices.ConvertToEntity(GetUser()));
            }
            GoalDTO goalDTO = ConvertToDTO(goal, goal.User);
            return goalDTO;
        }

        private void UpdateProgress(GoalDTO goalDTO)
        {
            UserDTO user = GetUser();
            Goal goal = ConvertToEntity(goalDTO, user);
            _goalRepo.UpdateProgress(user.Id, goal);
        }
        private void UpdateStatus(Status status, int goalId)
        {
            UserDTO user = GetUser();
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

        private void ValidateGoal(GoalDTO goal)
        {
            if (goal == null) throw new ArgumentNullException("Enter valid data!");
            if (goal.ReadingGoal <= 0) throw new ArgumentException("Reading goal cannot be negative!");
            if (goal.Start > goal.End) throw new ArgumentException("Invalid timeframe!");
            if (goal.End < DateTime.Now) throw new ArgumentException("Invalid end of goal!");
        }

        private void CheckForExpired(List<Goal> goals)
        {
            foreach (Goal goal in goals)
            {
                if(goal.End < DateTime.Now)
                {
                    UpdateStatus(Status.Expired, goal.Id);
                }
            }
        }

        private UserDTO GetUser()
        {
            return _userServices.LoadUser();
        }

        private async Task<List<GoalDTO>> GetAllGoals()
        {
            List<Goal> goals  = await _goalRepo.GetAllGoals();
            return goals.Select(g => ConvertToDTO(g,g.User)).ToList();
        }

        public async Task SendRemindersAsync()
        { 
            foreach(GoalDTO goal in await GetAllGoals())
            {
                await _emailService.SendGoalReminderEmail(goal);
            }
        }

        private Goal ConvertToEntity(GoalDTO goal, UserDTO user)
        {
            return new Goal(goal.Id, goal.Start, goal.End, goal.ReadingGoal, goal.CurrentProgress, goal.Status, _userServices.ConvertToEntity(user));
        }

        private GoalDTO ConvertToDTO(Goal goal, User user)
        { 
            return new GoalDTO(goal.Id, goal.Start, goal.End, goal.ReadingGoal, goal.CurrentProgress, goal.Status, _userServices.ConvertToDTO(user));
        }
    }
}
