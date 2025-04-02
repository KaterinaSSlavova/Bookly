namespace ViewModels.Model
{
    public class BookDetailsViewModel
    {
        public BookViewModel Book {  get; set; }
        public List<ShelfViewModel> Shelves { get; set; } = new List<ShelfViewModel>();
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        public string RatingFromUser { get; set; }  

        public BookDetailsViewModel(BookViewModel book, List<ShelfViewModel> shelves, List<ReviewViewModel> reviews, string ratingFromUser)
        {
            Book = book;
            Shelves = shelves;
            Reviews = reviews;
            RatingFromUser = ratingFromUser;
        }
    }
}
