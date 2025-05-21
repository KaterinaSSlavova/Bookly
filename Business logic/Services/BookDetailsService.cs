using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using Models.Enums;

namespace Business_logic.Services
{
    public class BookDetailsService: IBookDetailsService
    {
        private readonly IBookServices _bookServices;
        private readonly IShelfServices _shelfServices;
        private readonly IReviewServices _reviewServices;
        private readonly IRatingServices _ratingServices;

        public BookDetailsService(IBookServices bookServices, IShelfServices shelfServices, IReviewServices reviewServices, IRatingServices ratingServices) 
        {
            _bookServices = bookServices;
            _shelfServices = shelfServices;
            _reviewServices = reviewServices;
            _ratingServices = ratingServices;
        }   
        public BookDetailsDTO CreateDetailsDTO(int bookId) 
        {
            BookDTO? book = _bookServices.GetBookById(bookId);
            Ratings? rating = _ratingServices.GetUserRatingForBook(bookId);
            List<ShelfDTO> userShelves = _shelfServices.GetUserShelves();
            List<ReviewDTO> bookReviews = _reviewServices.GetBookReviews(book);
            return new BookDetailsDTO(book, userShelves, bookReviews, rating);
        }
    }
}
