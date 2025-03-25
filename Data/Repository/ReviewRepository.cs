using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Entities;

namespace Bookly.Data.Repository
{
    public class ReviewRepository: Repository
    {
        public ReviewRepository(IConfiguration configuration): base(configuration){ }

        public void AddReview(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO Reviews (Description, Date, UserId, BookId)
                                VALUES (@Description, @Date, @UserId, @BookId)";
                
            }
            catch(Exception ex) 
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
