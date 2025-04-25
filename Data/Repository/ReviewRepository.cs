using Bookly.Data.InterfacesRepo;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Entities;

namespace Bookly.Data.Repository
{
    public class ReviewRepository: Repository, IReviewRepository
    {
        private readonly IUserRepository _userRepo;
        public ReviewRepository(IConfiguration configuration, IUserRepository userRepo) : base(configuration)
        {
            this._userRepo = userRepo;
        }

        public bool AddReview(Review review)
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
                return true;
            }
            catch(Exception ex) 
            {
                throw new ApplicationException(ex.Message);
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
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public bool RemoveReview(int reviewId)
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
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
