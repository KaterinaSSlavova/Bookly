using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Model;

namespace Bookly.WebApp.Controllers
{
    public class GoalController : Controller
    {
        private readonly IGoalServices _igoalService;
        public GoalController(IGoalServices igoalService)
        {
            this._igoalService = igoalService;
        }

        [HttpGet]
        public IActionResult GoalOverview()
        {
            List<GoalViewModel> personalGoals = _igoalService.GetPersonalGoals();
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
        public IActionResult SaveGoal(GoalViewModel goal)
        {
            if (_igoalService.CreateGoal(goal))
            {
                return RedirectToAction("GoalOverview", "Goal");
            }
            return RedirectToAction("CreateGoal","Goal");
        }

        [HttpPost]
        public IActionResult RemoveGoal(int id)
        {
            _igoalService.RemoveGoal(id);
            return RedirectToAction("GoalOverview", "Goal");
        }
    }
}
