using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Bookly.ViewModels;
using Business_logic.DTOs;
using AutoMapper;

namespace Bookly.WebApp.Controllers
{
    public class GoalController : Controller
    {
        private readonly ILogger<GoalController> _logger;   
        private readonly IGoalServices _goalService;
        private readonly IMapper _mapper;
        public GoalController(ILogger<GoalController> logger, IGoalServices goalService, IMapper mapper)
        {
            this._logger = logger;
            this._goalService = goalService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GoalOverview()
        {
            try
            {
                List<GoalDTO> goals = _goalService.GetPersonalGoals();
                List<GoalViewModel> personalGoals = _mapper.Map<List<GoalViewModel>>(goals);
                return View(personalGoals);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to load goal overview page: {ErrorMessage}", ex.Message);
                TempData["GoalError"] = "An unexpected error occurred! Please try again later!";
                return View();
            }
        }

        [HttpGet]
        public IActionResult CreateGoal()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GoToCreateGoal()
        {  
            return RedirectToAction("CreateGoal","Goal");
        }

        [HttpPost]
        public IActionResult SaveGoal(GoalViewModel goalModel)
        {
            try
            {
                GoalDTO goal = _mapper.Map<GoalDTO>(goalModel);
                if (_goalService.CreateGoal(goal))
                {
                    return RedirectToAction("GoalOverview", "Goal");
                }
                TempData["InvalidGoal"] = "Please enter valid data!";
                return RedirectToAction("CreateGoal", "Goal");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to create a goal: {ErrorMessage}", ex.Message);
                TempData["GoalError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("GoalOverview", "Goal");
            }
        }

        [HttpPost]
        public IActionResult RemoveGoal(int id)
        {
            try
            {
                if (!_goalService.RemoveGoal(id))
                {
                    TempData["GoalError"] = "Cannot remove this goal!";
                }
                else
                {
                    TempData["GoalSuccess"] = "Goal was removed successfully!";
                }
                return RedirectToAction("GoalOverview", "Goal");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while trying to remove a goal: {ErrorMessage}", ex.Message);
                TempData["GoalError"] = "An unexpected error occurred! Please try again later!";
                return RedirectToAction("GoalOverview", "Goal");
            }
        }
    }
}
