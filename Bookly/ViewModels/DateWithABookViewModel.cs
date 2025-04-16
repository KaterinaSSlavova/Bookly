using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bookly.ViewModels
{
    public class DateWithABookViewModel
    {
        public List<BookViewModel> FilteredBooks{  get; set; } 
        public List<SelectListItem> Genres { get; set; } 
        public List<SelectListItem> Ratings { get; set; } 

        public DateWithABookViewModel(List<BookViewModel> books, List<SelectListItem> genres, List<SelectListItem> ratings)
        {
            this.FilteredBooks= books;
            this.Genres= genres;
            this.Ratings= ratings;
        }

    }
}
