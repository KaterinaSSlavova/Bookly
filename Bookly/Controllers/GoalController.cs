using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.ViewModels;
using Business_logic.DTOs;
using Business_logic.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.WebApp.Controllers
{
    public class GoalController : Controller
    {
        private readonly IGoalServices _goalService;
        private readonly IMapper _mapper;
        public GoalController(IGoalServices goalService, IMapper mapper)
        {
            this._goalService = goalService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GoalOverview()
        {
            List<GoalDTO> goals = _goalService.GetPersonalGoals();
            List<GoalViewModel> personalGoals = _mapper.Map<List<GoalViewModel>>(goals);
            return View(personalGoals);
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
            catch (ServiceValidationException ex)
            {
                TempData["InvalidGoal"] = ex.Message;
            }
            catch (InvalidReadingGoalException ex)
            {
                TempData["InvalidGoal"] = ex.Message;
            }
            catch (InvalidGoalStartDateException ex)
            {
                TempData["InvalidGoal"] = ex.Message;
            }
            catch (InvalidGoalEndDateException ex)
            {
                TempData["InvalidGoal"] = ex.Message;
            }
            return RedirectToAction("CreateGoal", "Goal");
        }

        [HttpPost]
        public IActionResult RemoveGoal(int id)
        {
            _goalService.RemoveGoal(id);
            TempData["GoalSuccess"] = "Goal was removed successfully!";
            return RedirectToAction("GoalOverview", "Goal");
        }
    }
}
