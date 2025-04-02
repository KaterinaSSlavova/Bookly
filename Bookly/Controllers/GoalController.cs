using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using ViewModels.Model;

namespace Bookly.WebApp.Controllers
{
    public class GoalController : Controller
    {

        private readonly IGoalServices _igoalService;
        private readonly IUserServices _iuserService;
        public GoalController(IGoalServices igoalService, IUserServices iuserService)
        {
            this._igoalService = igoalService;
            this._iuserService = iuserService;
        }

        [HttpGet]
        public IActionResult GoalOverview()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _iuserService.LoadUser(ViewBag.Username);
            List<GoalViewModel> personalGoals = _igoalService.GetPersonalGoals(user.Id);
            return View(personalGoals);
        }

        [HttpGet]
        public IActionResult CreateGoal()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
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
            ViewBag.Username = HttpContext.Session.GetString("Username");
            if (_igoalService.CreateGoal(goal, _iuserService.LoadUser(ViewBag.Username).Id))
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
