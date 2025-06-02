using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookly.ViewModels
{
    public class BookDetailsViewModel
    {
        public BookViewModel Book {  get; set; }
        public List<RegularShelfViewModel> Shelves { get; set; } = new List<RegularShelfViewModel>();
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        public string RatingFromUser { get; set; }
        public List<SelectListItem> Ratings { get; set; }
    }
}
