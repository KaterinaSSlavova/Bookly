using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Newtonsoft.Json;
using Bookly.ViewModels;
using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Business_logic.Exceptions;

namespace Bookly.Controllers
{
    public class RandomController : Controller
    {
        private readonly IRandomServices _randomServices;
        private readonly IBookServices _bookServices;
        private readonly IMapper _mapper;
        public RandomController(IRandomServices randomServices, IBookServices bookServices, IMapper mapper)
        {
            _randomServices = randomServices;
            _bookServices = bookServices;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult SpinTheWheel()
        {
            int? bookId = HttpContext.Session.GetInt32("Id");
            if (bookId.HasValue)
            {
                BookDTO bookDTO = _bookServices.GetBookById(bookId.Value);
                BookViewModel bookModel = _mapper.Map<BookViewModel>(bookDTO);
                return View(bookModel);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Spin()
        {
            HttpContext.Session.SetInt32("Id", _randomServices.RandomResult().Id);
            return RedirectToAction("SpinTheWheel", "Random");
        }

        [HttpGet]
        public IActionResult DateWithABook()
        {
            var filteredBooksJson = TempData["Filtered"] as string;
            DateWithABookDTO bookDTO = _randomServices.CreateDateDTO(filteredBooksJson);
            List<BookViewModel> bookModel = _mapper.Map<List<BookViewModel>>(bookDTO.FilteredBooks);
            DateWithABookViewModel model = new DateWithABookViewModel(bookModel, MapGenres(bookDTO.Genres), MapRatings(bookDTO.Ratings));
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooks(Ratings ratings, Genre genre)
        {
            List<BookDTO> filteredBooks = _randomServices.FilterBooks(genre, ratings);
            TempData["Filtered"] = JsonConvert.SerializeObject(filteredBooks);
            if (filteredBooks.Count == 0 || filteredBooks == null)
            {
                TempData["Warning"] = "No matches found!";
            }
            return RedirectToAction("DateWithABook", "Random");
        }

        [HttpPost]
        public IActionResult AddBookFromSpinTheWheel(BookViewModel bookModel)
        {
            AddToWishList(bookModel);
            return RedirectToAction("SpinTheWheel", "Random");
        }

        [HttpPost]
        public IActionResult AddBookFromRandomDate(BookViewModel bookModel)
        {
            AddToWishList(bookModel);
            return RedirectToAction("DateWithABook", "Random");
        }

        private void AddToWishList(BookViewModel bookModel)
        {
            try
            {
                BookDTO book = _mapper.Map<BookDTO>(bookModel);
                _randomServices.AddToWishList(book);
                TempData["Success"] = "Book added to shelf!";
            }
            catch(BookIsAlreadyOnShelfException ex)
            {
                TempData["Warning"] = ex.Message;
            }
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
