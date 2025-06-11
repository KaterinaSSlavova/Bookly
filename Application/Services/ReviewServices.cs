using AutoMapper;
using Interfaces;
using Models.Entities;
using Business_logic.DTOs;
using Exceptions;

namespace Bookly.Business_logic.Services
{
    public class ReviewServices: IReviewServices
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IMapper _mapper;
        private readonly IUserServices _userServices;
        private readonly IBookServices _bookServices;

        public ReviewServices(IReviewRepository reviewRepo, IMapper mapper, IUserServices userServices, IBookServices bookServices)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
            _userServices = userServices;
            _bookServices = bookServices;
        }

        public void AddReview(string description, int bookId)
        {
            UserDTO? user = _userServices.LoadUser();
            BookDTO? book = _bookServices.GetBookById(bookId);
            ReviewDTO review = new ReviewDTO(description, user, book);
            ValidateReview(review);
            _reviewRepo.AddReview(ConvertToEntity(review));    
        }

        public List<ReviewDTO> GetBookReviews(BookDTO book)
        {
            List<Review> reviews = _reviewRepo.GetBookReviews(_mapper.Map<Book>(book));
            return reviews.Select(r => ConvertToDTO(r)).ToList();
        }

        public void RemoveReview(int reviewId)
        {
            _reviewRepo.RemoveReview(reviewId);
        }

        private void ValidateReview(ReviewDTO review)
        {
            if (review == null || review.Description == null) throw new NullReferenceException("Review cannot be empty");
            if (review.Description.Length > 500) throw new InvalidReviewLengthException();
            if (review.Description.Length <= 1) throw new ReviewLengthShortException();
        }

        private Review ConvertToEntity(ReviewDTO review)
        {
            return new Review(review.Id, review.Description, review.Date, _userServices.ConvertToEntity(review.User), _mapper.Map<Book>(review.Book));
        }

        private ReviewDTO ConvertToDTO(Review review)
        {
            return new ReviewDTO(review.Id, review.Description, review.Date, _userServices.ConvertToDTO(review.User), _mapper.Map<BookDTO>(review.Book));
        }
    }
}
