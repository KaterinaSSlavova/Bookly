using Models.Entities;
using Models.Enums;

namespace Business_logic.DTOs
{
    public class BookDetailsDTO
    {
        public Book Book { get; set; }
        public List<Shelf> Shelves { get; set; }
        public List<Review> Reviews { get; set; }
        public Ratings? RatingFromUser { get; set; }

        public BookDetailsDTO(Book book, List<Shelf> shelves, List<Review> reviews, Ratings? rating)
        {
            this.Book = book;
            this.Shelves = shelves;
            this.Reviews = reviews;        
            this.RatingFromUser = rating;
        }
    }
}
