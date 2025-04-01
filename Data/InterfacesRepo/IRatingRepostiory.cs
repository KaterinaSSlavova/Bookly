using Models.Enums;

namespace Bookly.Data.InterfacesRepo
{
    public interface IRatingRepostiory
    {
        bool RateBook(int bookId, int ratingId);
        bool ConnectUserWithRating(int userId, int ratingId);
        List<Ratings> GetAllRatingsForBook(int bookId);
        Ratings? GetUserRatingForBook(int userId, int bookId);
        int CheckForRating(int userId, int bookId);
        void RemoveRating(int userId, int bookId, int ratingId);
    }
}
