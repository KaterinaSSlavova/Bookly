using Bookly.Data.InterfacesRepo;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Entities;

namespace Bookly.Data.Repository
{
    public class ReviewRepository: Repository
    {
        private readonly IUserRepository _iuserRepo;
        public ReviewRepository(IConfiguration configuration, IUserRepository iuserRepo): base(configuration)
        {
            this._iuserRepo = iuserRepo;
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

        public List<Review> GetBookReviews(int bookId)
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
                command.Parameters.AddWithValue("@BookId", bookId);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    reviews.Add(new Review
                    {
                        Id = reader.GetInt32(0),
                        Description = reader.GetString(1),
                        Date = reader.GetDateTime(2),
                        User = _iuserRepo.GetUserById(reader.GetInt32(3))
                    });
                }
                return reviews;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
