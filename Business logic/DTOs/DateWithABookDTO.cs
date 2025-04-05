using Models.Entities;
using Models.Enums;

namespace Business_logic.DTOs
{
    public class DateWithABookDTO
    {
        public List<Book> FilteredBooks { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Ratings> Ratings { get; set; }

        public DateWithABookDTO(List<Book> books)
        {
            this.FilteredBooks = books;
            this.Genres = new List<Genre>();   
            this.Ratings = new List<Ratings>();
        }
    }
}
