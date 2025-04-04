using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Enums;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IRatingServices
    {
        bool RateBook(int bookId, int ratingId);
        List<Ratings> GetBookRatings(int bookId);
        Ratings? GetUserRatingForBook(int bookId);
        List<SelectListItem> GetAllRatings();
        Ratings GetMostPopularRating(int bookId);
    }
}
