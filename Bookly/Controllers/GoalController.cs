using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Model;
using Models.Entities;
using AutoMapper;

namespace Bookly.WebApp.Controllers
{
    public class GoalController : Controller
    {
        private readonly IGoalServices _igoalService;
        private readonly IMapper _mapper;
        public GoalController(IGoalServices igoalService, IMapper mapper)
        {
            this._igoalService = igoalService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GoalOverview()
        {
            List<Goal> goals = _igoalService.GetPersonalGoals();
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
            return RedirectToAction("CreateGoal","Goal");
        }

        [HttpPost]
        public IActionResult SaveGoal(GoalViewModel goalModel)
        {
            Goal goal = _mapper.Map<Goal>(goalModel);
            if (_igoalService.CreateGoal(goal))
            {
                return RedirectToAction("GoalOverview", "Goal");
            }
            TempData["InvalidGoal"] = "Please enter valid data!";
            return RedirectToAction("CreateGoal","Goal");
        }

        [HttpPost]
        public IActionResult RemoveGoal(int id)
        {
            if(!_igoalService.RemoveGoal(id))
            {
                TempData["GoalError"] = "Cannot remove this goal!";
            }
            else
            {
                TempData["GoalSuccess"] = "Goal was removed successfully!";
            }
            return RedirectToAction("GoalOverview", "Goal");
        }
    }
}
