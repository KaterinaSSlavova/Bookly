using Interfaces;
using EFDataLayer.DBContext;
using Business_logic.DTOs;

namespace Bookly.Business_logic.Services
{
    public class RatingServices : IRatingServices
    {
        private readonly IRatingRepostiory _ratingRepository;
        private readonly IUserServices _userServices;
        public RatingServices(IRatingRepostiory ratingRepository, IUserServices userServices)
        {
            _ratingRepository = ratingRepository;
            _userServices = userServices;
        }

        public void RateBook(int bookId, int ratingId)
        {
            UserDTO user = GetUser();
            int previousRatingCount = _ratingRepository.CheckForRating(user.Id, bookId);
            if (previousRatingCount > 0)
            {
                _ratingRepository.RemoveRating(user.Id, bookId, ratingId);
            }
            _ratingRepository.RateBook(bookId, ratingId);
            _ratingRepository.ConnectUserWithRating(user.Id, ratingId);
        }

        public List<Ratings> GetBookRatings(int bookId)
        {
            return _ratingRepository.GetAllRatingsForBook(bookId);
        }

        public Ratings? GetUserRatingForBook(int bookId)
        {
            UserDTO user = GetUser();
            return _ratingRepository.GetUserRatingForBook(user.Id, bookId);
        }

        public Ratings GetMostPopularRating(int bookId)
        {
            List<Ratings> bookRatings = GetBookRatings(bookId);
            Ratings rating = bookRatings.GroupBy(r => r).
                OrderByDescending(gr => gr.Count()).Select(gr => gr.Key).FirstOrDefault();
            return rating;
        }

        private UserDTO GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
