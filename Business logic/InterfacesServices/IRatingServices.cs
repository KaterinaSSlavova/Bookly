using Models.Enums;

namespace Bookly.Business_logic.InterfacesServices
{
    public interface IRatingServices
    {
        bool RateBook(int userId, int bookId, int ratingId);
        List<Ratings> GetBookRatings(int bookId);
        Ratings? GetUserRatingForBook(int userId, int bookId);
        List<Ratings> GetAllRatings();
        Ratings GetMostPopularRating(int bookId);
    }
}
