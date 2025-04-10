using Business_logic.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Newtonsoft.Json;
using ViewModels.Model;
using Models.Entities;
using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using AutoMapper;

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
            DateWithABookDTO bookDTO = _bookServices.CreateDateDTO(filteredBooksJson);
            DateWithABookViewModel model = _mapper.Map<DateWithABookViewModel>(bookDTO);
            return View(model);
        }

        [HttpPost]
        public IActionResult FilterBooks(Ratings ratings, Genre genre)
        {
            List<Book> filteredBooks = _iRandomServices.FilterBooks(genre, ratings);
            TempData["Filtered"] = JsonConvert.SerializeObject(filteredBooks);  
            return RedirectToAction("DateWithABook", "Random");
        }
    }
}
