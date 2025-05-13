using Bookly.Data.InterfacesRepo;
using Data.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models.Entities;

namespace Bookly.Data.Repository
{
    public class ReviewRepository: Repository, IReviewRepository
    {
        private readonly ILogger<ReviewRepository> _logger;
        private readonly IUserRepository _userRepo;
        public ReviewRepository(IConfiguration configuration, IUserRepository userRepo, ILogger<ReviewRepository> logger) : base(configuration)
        {
            this._userRepo = userRepo;
            _logger = logger;
        }

        public void AddReview(Review review)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO Reviews (Description, Date, UserId, BookId)
                                VALUES (@Description, @Date, @UserId, @BookId)";
                
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("Description", review.Description);
                command.Parameters.AddWithValue("Date", review.Date);
                command.Parameters.AddWithValue("UserId", review.User.Id);
                command.Parameters.AddWithValue("BookId", review.Book.Id);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while adding a review.");
                throw new RepositoryException("Could not create review. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while adding a review.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public List<Review> GetBookReviews(Book book)
        {
            try
            {
                List<Review> reviews = new List<Review>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT r.Id, r.[Description], r.[Date], r.UserId
                                    FROM Reviews as r
                                    WHERE isArchived = @isArchived AND BookId = @BookId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 0);
                command.Parameters.AddWithValue("@BookId", book.Id);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    reviews.Add(new Review
                        (
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetDateTime(2),
                           _userRepo.GetUserById(reader.GetInt32(3)),
                            book
                        ));
                }
                return reviews;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while loading all book reviews.");
                throw new RepositoryException("Could not load reviews. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while loading all book reviews.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public void RemoveReview(int reviewId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"UPDATE Reviews
                                SET isArchived = @isArchived
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 1);
                command.Parameters.AddWithValue("@Id", reviewId);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while removing a review.");
                throw new RepositoryException("Could not remove this review. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while removing a review.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
