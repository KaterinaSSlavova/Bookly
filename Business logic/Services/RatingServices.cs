using Bookly.Business_logic.InterfacesServices;
using Bookly.Data.InterfacesRepo;
using Models.Enums;
using Models.Entities;

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

        public bool RateBook(int bookId, int ratingId)
        {
            User user = GetUser();
            int previousRatingCount = _ratingRepository.CheckForRating(user.Id, bookId);
            if (previousRatingCount > 0)
            {
                _ratingRepository.RemoveRating(user.Id, bookId, ratingId);
            }
            if (_ratingRepository.RateBook(bookId, ratingId) && _ratingRepository.ConnectUserWithRating(user.Id, ratingId))
            {
                return true;
            }
            return false;
        }

        public List<Ratings> GetBookRatings(int bookId)
        {
            return _ratingRepository.GetAllRatingsForBook(bookId);
        }

        public Ratings? GetUserRatingForBook(int bookId)
        {
            User user = GetUser();
            return _ratingRepository.GetUserRatingForBook(user.Id, bookId);
        }

        public Ratings GetMostPopularRating(int bookId)
        {
            List<Ratings> bookRatings = GetBookRatings(bookId);
            Ratings rating = bookRatings.GroupBy(r => r).
                OrderByDescending(gr => gr.Count()).Select(gr => gr.Key).FirstOrDefault();
            return rating;
        }

        private User GetUser()
        {
            return _userServices.LoadUser();
        }
    }
}
