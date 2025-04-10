using Microsoft.AspNetCore.Mvc.Rendering;

namespace ViewModels.Model
{
    public class DateWithABookViewModel
    {
        public List<BookViewModel> FilteredBooks{  get; set; } 
        public List<SelectListItem> Genres { get; set; } 
        public List<SelectListItem> Ratings { get; set; } 
    }
}
