using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
            return RedirectToAction("CreateGoal", "Goal");
        }

        [HttpPost]
        public IActionResult SaveGoal(GoalViewModel goalModel)
        {
            try
            {
                GoalDTO goal = _mapper.Map<GoalDTO>(goalModel);
                _goalService.CreateGoal(goal);
                return RedirectToAction("GoalOverview", "Goal");
            }
            catch (ArgumentException ex)
            {
                TempData["InvalidGoal"] = ex.Message;
                return RedirectToAction("CreateGoal", "Goal");
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An sql error occurred while trying to create a goal: {ErrorMessage}", ex.Message);
                TempData["GoalError"] = "An error occurred while trying to save the goal! Please try again later!";
                return RedirectToAction("GoalOverview", "Goal");
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
                _goalService.RemoveGoal(id);
                TempData["GoalSuccess"] = "Goal was removed successfully!";
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "An sql occurred while trying to remove a goal: {ErrorMessage}", ex.Message);
                TempData["GoalError"] = "An error occurred while trying to remove this goal! Please try again later!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to remove a goal: {ErrorMessage}", ex.Message);
                TempData["GoalError"] = "An unexpected error occurred! Please try again later!";
            }
            return RedirectToAction("GoalOverview", "Goal");
        }
    }
}
