using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;
using ViewModels.Model;

namespace Bookly.Business_logic.Services
{
    public class ReviewServices: IReviewServices
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly IMapper _mapper;
        public ReviewServices(IReviewRepository reviewRepo, IMapper mapper)
        {
            this._reviewRepo = reviewRepo;
            this._mapper = mapper;
        }

        public bool AddReview(string description, User user, BookViewModel bookModel)
        {
            Book book = _mapper.Map<Book>(bookModel);
            Review review = new Review(description, user, book);
            return _reviewRepo.AddReview(review);    
        }

        public List<ReviewViewModel> GetBookReviews(int bookId)
        {
            List<Review> reviews = _reviewRepo.GetBookReviews(bookId);
            List<ReviewViewModel> reviewViewModels = new List<ReviewViewModel>();
            foreach (Review review in reviews)
            {
                reviewViewModels.Add(_mapper.Map<ReviewViewModel>(review));
            }
            return reviewViewModels;
        }
    }
}
