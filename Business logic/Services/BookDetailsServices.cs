using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Business_logic.DTOs;
using Models.Entities;
using Models.Enums;
using ViewModels.Model;

namespace Business_logic.Services
{
    public class BookDetailsServices: IBookDetailsService
    {
        private readonly IBookServices _bookServices;
        private readonly IShelfServices _shelfServices;
        private readonly IReviewServices _reviewServices;
        private readonly IRatingServices _ratingServices;
        private readonly IMapper _mapper;

        public BookDetailsServices(IBookServices bookServices, IShelfServices shelfServices, IReviewServices reviewServices, IRatingServices ratingServices, IMapper mapper)
        {
            _bookServices = bookServices;
            _shelfServices = shelfServices;
            _reviewServices = reviewServices;
            _ratingServices = ratingServices;
            _mapper = mapper;
        }

        public BookDetailsDTO CreateDTO(int bookId)
        {
            Book? book = _bookServices.GetBookById(bookId);
            Ratings? rating = _ratingServices.GetUserRatingForBook(bookId);
            List<Shelf> userShelves = _shelfServices.GetUserShelves();
            List<Review> bookReviews = _reviewServices.GetBookReviews(book);
            return new BookDetailsDTO(book, userShelves, bookReviews, rating);
        }

        public BookDetailsViewModel GetBookDetails(int bookId)
        {
            return _mapper.Map<BookDetailsViewModel>(CreateDTO(bookId));
        }
    }
}
