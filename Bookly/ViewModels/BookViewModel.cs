using Models.Enums;
using Models.Entities;

namespace Bookly.ViewModels
{
    public class BookViewModel
    {
        public int BookId { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Picture {  get; set; }
        public string ISBN { get; set; }
        public string Genre { get; set; }
        public int ShelfId { get; set; }
        public string ShelfName { get; set; }
        public  Ratings UserRating { get; set; }
        public List<ReviewViewModel> ReviewViewModels { get; set; }
        public BookViewModel()
        {
            ReviewViewModels = new List<ReviewViewModel>(); 
        }
        //public static BookViewModel ConvertToViewModel(Book book, Shelf shelf, List<ReviewViewModel> reviewsUI, Ratings rating)
        //{
        //    BookViewModel viewModel = new BookViewModel()
        //    {
        //        BookId = book.Id,
        //        Title = book.Title,
        //        Description = book.Description,
        //        Author = book.Author,
        //        Picture = book.Picture,
        //        ISBN = book.ISBN,
        //        Genre = book.Genre,
        //        ShelfId = shelf.Id,
        //        ShelfName = shelf.Name,
        //        UserRating = rating,
        //        ReviewViewModels = reviewsUI
        //    };
        //    return viewModel;   
        //}
    }
}
