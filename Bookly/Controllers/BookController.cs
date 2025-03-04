using Microsoft.AspNetCore.Mvc;
using Bookly.Models;
using Bookly.Repository;

namespace Bookly.Controllers
{
    public class BookController : Controller
    {
        public IActionResult BookDetails(int id)
        {
            ViewBag.Username = HttpContext.Session.GetString("Username");
            Book? book = DbHelper.GetBook(id);
            return View(book);
        }

    }
}
