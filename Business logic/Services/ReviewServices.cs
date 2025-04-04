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
        private readonly IUserServices _userServices;

        public ReviewServices(IReviewRepository reviewRepo, IMapper mapper, IUserServices userServices)
        {
            _reviewRepo = reviewRepo;
            _mapper = mapper;
            _userServices = userServices;
        }

        public bool AddReview(string description, BookViewModel bookModel)
        {
            User user = _userServices.LoadUser();
            Book book = _mapper.Map<Book>(bookModel);
            Review review = new Review(description, user, book);
            return _reviewRepo.AddReview(review);    
        }

        public List<ReviewViewModel> GetBookReviews(BookViewModel model)
        {
            Book book = _mapper.Map<Book>(model);
            List<Review> reviews = _reviewRepo.GetBookReviews(book);
            List<ReviewViewModel> reviewViewModels = new List<ReviewViewModel>();
            foreach (Review review in reviews)
            {
                reviewViewModels.Add(_mapper.Map<ReviewViewModel>(review));
            }
            return reviewViewModels;
        }
    }
}
