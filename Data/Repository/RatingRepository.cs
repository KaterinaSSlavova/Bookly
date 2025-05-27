using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Enums;
using Interfaces;
using Exceptions;
using Microsoft.Extensions.Logging;

namespace Bookly.Data.Repository
{
    public class RatingRepository : Repository, IRatingRepostiory
    {
        private readonly ILogger<RatingRepository> _logger;
        public RatingRepository(IConfiguration configuration, ILogger<RatingRepository> logger) : base(configuration) 
        {
            _logger = logger;
        }

        public void RateBook(int bookId, int ratingId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO BookRating (RatingId, BookId)
                                VALUES (@RatingId, @BookId)";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@RatingId", ratingId);
                command.Parameters.AddWithValue("@BookId", bookId);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while rating a book.");
                throw new RepositoryException("Could not rate this book. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while rating a book.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public void ConnectUserWithRating(int userId, int ratingId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO UserRating (UserId, RatingId)
                                VALUES (@UserId, @RatingId)";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RatingId", ratingId);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while connecting user with rating.");
                throw new RepositoryException("Could not rate this book. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while connecting user with rating.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public List<Ratings> GetAllRatingsForBook(int bookId)
        {
            try
            {
                List<Ratings> ratings = new List<Ratings>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT r.Id
                                FROM Rating as r
                                INNER JOIN BookRating as br
                                ON br.RatingId = r.Id
                                WHERE br.BookId = @BookId";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@BookId", bookId);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ratings.Add((Ratings)reader.GetInt32(0));
                }
                return ratings;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while loading all ratings for a book.");
                throw new RepositoryException("Could not load ratings. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while loading all ratings for a book.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public Ratings? GetUserRatingForBook(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT r.Id
                                FROM Rating as r
                                INNER JOIN UserRating as ur
                                ON ur.RatingId = r.Id
                                INNER JOIN BookRating as br
                                ON br.RatingId = ur.RatingId
                                WHERE ur.UserId = @UserId AND br.BookId = @BookId;";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@BookId", bookId);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                     return (Ratings)reader.GetInt32(0);
                }
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while loading user rating for a book.");
                throw new RepositoryException("Could not load this rating. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while loading user rating for a book.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public int CheckForRating(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT COUNT(*)
                            FROM UserRating ur
                            INNER JOIN BookRating br ON ur.RatingId = br.RatingId
                            WHERE ur.UserId = @UserId AND br.BookId = @BookId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@BookId", bookId);

                int count = (int)command.ExecuteScalar();
                return count;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while checking for a rating.");
                throw new RepositoryException("Could not check for rating. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while checking for a rating.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public void RemoveRating(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"DELETE FROM UserRating 
                                 WHERE UserId = @UserId AND RatingId IN
                                 (SELECT RatingId FROM UserRating WHERE UserId = @UserId);

                               DELETE FROM BookRating
                                WHERE BookId = @BookId AND RatingId IN
                                 (SELECT RatingId FROM BookRating WHERE BookId = @BookId);";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@BookId", bookId);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while removing this rating.");
                throw new RepositoryException("Could not remove this rating. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while removing this rating.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }
    }
}
