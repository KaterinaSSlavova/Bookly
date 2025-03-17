using Bookly.Repository;
using Bookly.Services;
using Bookly.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookly.Controllers
{
    public class GoalController : Controller
    {

        private readonly GoalServices _goalService;
        private readonly UserServices _userService;
        public GoalController(GoalServices goalService, UserServices userService)
        {
            this._goalService = goalService;
            this._userService = userService;
        }

        [HttpGet]
        public IActionResult GoalOverview()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            User user = _userService.LoadUser(ViewBag.Username);
            _goalService.UpdateGoalProgress(user.Id);
            _goalService.GetPersonalGoals(user.Id);
            List<Goal> personalGoals = _goalService.GetPersonalGoals(_userService.LoadUser(ViewBag.Username).Id);
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
        public IActionResult SaveGoal(Goal goal)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            if (_goalService.CreateGoal(goal, _userService.LoadUser(ViewBag.Username).Id))
            {
                return RedirectToAction("GoalOverview", "Goal");
            }
            return RedirectToAction("CreateGoal","Goal");
        }

        [HttpPost]
        public IActionResult RemoveGoal(int id)
        {
            _goalService.RemoveGoal(id);
            return RedirectToAction("GoalOverview", "Goal");
        }
    }
}
