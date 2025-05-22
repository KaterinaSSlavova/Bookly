using Models.Enums;

namespace Business_logic.DTOs
{
    public class BookDetailsDTO
    {
        public BookDTO Book { get; set; }
        public List<RegularShelfDTO> Shelves { get; set; }
        public List<ReviewDTO> Reviews { get; set; }
        public Ratings? RatingFromUser { get; set; }

        public BookDetailsDTO(BookDTO book, List<RegularShelfDTO> shelves, List<ReviewDTO> reviews, Ratings? rating)
        {
            this.Book = book;
            this.Shelves = shelves;
            this.Reviews = reviews;        
            this.RatingFromUser = rating;
        }
    }
}
