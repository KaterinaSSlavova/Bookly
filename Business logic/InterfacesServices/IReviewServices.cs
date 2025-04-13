using Business_logic.DTOs;
using Models.Entities;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IReviewServices
    {
        bool AddReview(string description, int bookId);
        List<Review> GetBookReviews(BookDTO book);
    }
}
