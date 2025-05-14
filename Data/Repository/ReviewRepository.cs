using Bookly.Data.InterfacesRepo;
using Data.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models.Entities;
using Models.Enums;

namespace Bookly.Data.Repository
{
    public class ReviewRepository: Repository, IReviewRepository
    {
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewRepository(IConfiguration configuration, ILogger<ReviewRepository> logger) : base(configuration)
        {
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

                string sql = @"SELECT r.Id, r.[Description], r.[Date], u.Id, u.Picture, u.Username, u.Email, u.[Password], u.BirthDate, u.RoleId
                                FROM Reviews as r
                                INNER JOIN Users as u
                                ON u.Id = r.UserId
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
                           new User(reader.GetInt32(3),
							  reader.IsDBNull(4) ? null : reader.GetSqlBinary(4).Value,
							  reader.GetString(5),
							  reader.IsDBNull(8) ? (DateTime?)null : reader.GetDateTime(8),
							  reader.GetString(6),
							  reader.GetString(7),
							  (Role)reader.GetInt32(9)),
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
