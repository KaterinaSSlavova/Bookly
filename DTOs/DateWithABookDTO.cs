using EFDataLayer.DBContext;

namespace Business_logic.DTOs
{
    public class DateWithABookDTO
    {
        public List<BookDTO> FilteredBooks { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Ratings> Ratings { get; set; }

        public DateWithABookDTO(List<BookDTO> books, List<Genre> genres, List<Ratings> ratings)
        {
            this.FilteredBooks = books;
            this.Genres = genres;
            this.Ratings = ratings;
        }
    }
}
