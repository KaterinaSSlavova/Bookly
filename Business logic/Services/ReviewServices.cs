using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Entities;

namespace Bookly.Business_logic.Services
{
    public class ReviewServices: IReviewServices
    {
        private readonly IReviewRepository _reviewRepo;
        public ReviewServices(IReviewRepository reviewRepo)
        {
            this._reviewRepo = reviewRepo;
        }

        public bool AddReview(Review review)
        {
            return _reviewRepo.AddReview(review);    
        }

        public List<Review> GetBookReviews(int bookId)
        {
           return _reviewRepo.GetBookReviews(bookId);
        }
    }
}
