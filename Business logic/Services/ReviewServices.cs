using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Business_logic.DTOs;

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

        public bool AddReview(string description, int bookId)
        {
            User? user = _userServices.LoadUser();
            BookDTO? book = _bookServices.GetBookById(bookId);
            Review review = new Review(description, user, _mapper.Map<Book>(book));
            return _reviewRepo.AddReview(review);    
        }

        public List<Review> GetBookReviews(BookDTO book)
        {
            List<Review> reviews = _reviewRepo.GetBookReviews(_mapper.Map<Book>(book));
            return reviews;
        }
    }
}
