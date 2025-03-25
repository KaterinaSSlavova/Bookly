using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Bookly.Data.Repository
{
    public class ReviewRepository
    {
        private readonly string _connectionString;
        public ReviewRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void AddReview(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
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
