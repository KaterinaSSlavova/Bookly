using Models.Entities;
using ViewModels.Model;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IReviewServices
    {
        bool AddReview(string description, User user, BookViewModel book);
        List<ReviewViewModel> GetBookReviews(int bookId);
    }
}
