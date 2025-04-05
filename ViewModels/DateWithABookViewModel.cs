using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViewModels.Model
{
    public class DateWithABookViewModel
    {
        public List<BookViewModel> FilteredBooks{  get; set; } = new List<BookViewModel>();
        public List<SelectListItem> Genres { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Ratings { get; set; } = new List<SelectListItem>();
    }
}
