using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Bookly.Data.Repository
{
    public class Repository
    {
        private readonly string _connectionString;
        protected Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected SqlConnection GetSqlConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
