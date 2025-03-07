using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Controllers
{
    public class BookController : Controller
    {
        [HttpGet]
        public IActionResult BookDetails(int id)   
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            Book? book = DbHelper.GetBook(id);
            User user = DbHelper.LoadUser(ViewBag.Username);
            ViewBag.Shelves = DbHelper.GetUserShelves(user.Id);
            return View(book);
        }

        [HttpPost]
        public IActionResult GoBack()
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult MoveToShelf(int bookId, int shelfId)
        {
            string username = HttpContext.Session.GetString("Username");
            User? user = DbHelper.LoadUser(username);
            if(DbHelper.AddBookToShelf(bookId,shelfId, user.Id))
            {
                TempData["Message"] = "Book added to shelf!";
            }
            return RedirectToAction("BookDetails", "Book",new { id=bookId });
        }
    }
}
