using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Newtonsoft.Json;
using Bookly.ViewModels;
using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace Bookly.Controllers
{
    public class RandomController : Controller
    {
        private readonly IRandomServices _iRandomServices;
        private readonly IBookServices _bookServices;
        private readonly IMapper _mapper;
        public RandomController(IRandomServices iRandomServices, IBookServices bookServices, IMapper mapper)
        {
            _iRandomServices = iRandomServices;
            _bookServices = bookServices;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult SpinTheWheel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Spin()
        {
            TempData["Book"] = _iRandomServices.RandomResult().Title;
            return RedirectToAction("SpinTheWheel", "Random");
        }

        [HttpGet]
        public IActionResult DateWithABook()
        {
            var filteredBooksJson = TempData["Filtered"] as string;
            DateWithABookDTO bookDTO = _iRandomServices.CreateDateDTO(filteredBooksJson);
            List<BookViewModel> bookModel = _mapper.Map<List<BookViewModel>>(bookDTO.FilteredBooks);
            DateWithABookViewModel model = new DateWithABookViewModel(bookModel, MapGenres(bookDTO.Genres), MapRatings(bookDTO.Ratings));
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooks(Ratings ratings, Genre genre)
        {
            List<BookDTO> filteredBooks = _iRandomServices.FilterBooks(genre, ratings);
            TempData["Filtered"] = JsonConvert.SerializeObject(filteredBooks);
            if (filteredBooks.Count == 0 || filteredBooks == null)
            {
                TempData["DateIndication"] = "No matches found!";
            }
            return RedirectToAction("DateWithABook", "Random");
        }

        [HttpPost]
        public IActionResult SelectDescription(int id)
        {
            BookDTO book = _bookServices.GetBookById(id);
            BookViewModel bookModel = _mapper.Map<BookViewModel>(book);
            return PartialView("_BookModalPartial", bookModel);
        }

        private List<SelectListItem> MapGenres(List<Genre> genres)
        {
            return genres.Select(g => new SelectListItem
            {
                Value = g.ToString(),
                Text = g.ToString()
            }).ToList();
        }

        private List<SelectListItem> MapRatings(List<Ratings> ratings)
        {
            return ratings.Select(r => new SelectListItem
            {
                Value = r.ToString(),
                Text = r.ToString()
            }).ToList();
        }
    }
}
