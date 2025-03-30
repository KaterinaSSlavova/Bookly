using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Enums;

namespace Bookly.Data.Repository
{
    public class RatingRepository : Repository
    {
        public RatingRepository(IConfiguration configuration) : base(configuration) { }

        public bool RateBook(int bookId, int ratingId)
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
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public bool ConnectUserWithRating(int userId, int ratingId)
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
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public List<Ratings> GetAllRatingsForBook(int bookId)
        {
            try
            {
                List<Ratings> ratings = new List<Ratings>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT r.Stars
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
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public Ratings? GetUserRatingForBook(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT r.Stars
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
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
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
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public void DeleteRating(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"DELETE FROM UserRating 
                                 WHERE UserId = @UserId AND RatingId IN
                                 (SELECT RatingId FROM BookRating WHERE BookId = @BookId)";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@BookId", bookId);

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }
    }
}
