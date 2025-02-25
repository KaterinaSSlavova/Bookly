using System.Runtime.Intrinsics.Arm;
using Bookly.Models;
using Microsoft.Data.SqlClient;

namespace Bookly.Repository
{
    public class DbHelper
    {
        private const string connectionString = "Server=DESKTOP-GPBCRNQ;Database=BooklyDB;Trusted_Connection=True; TrustServerCertificate=True;";

        internal static bool Register(User user)
        {
            try
            {
                using SqlConnection connection=new SqlConnection(connectionString);
                connection.Open();
                string checkSql= @"SELECT COUNT(*) FROM Users WHERE Username = @Username or Email=@Email";
                using SqlCommand commandCheck = new SqlCommand(checkSql, connection);
                commandCheck.Parameters.AddWithValue("@Username", user.Username);
                commandCheck.Parameters.AddWithValue("@Email", user.Email);
                int count = (int)commandCheck.ExecuteScalar();
                if (count > 0)
                {
                    throw new ApplicationException("Username or Password is already taken.");
                }

                string insertSql = @"INSERT INTO Users([Username], Email, [Password]) 
                               VALUES (@Username, @Email, @Password)";
                using SqlCommand commandInsert = new SqlCommand(insertSql,connection);
                commandInsert.Parameters.AddWithValue("@Username", user.Username);
                commandInsert.Parameters.AddWithValue("@Email", user.Email);
                commandInsert.Parameters.AddWithValue("@Password", user.Password);

                commandInsert.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error registering user: "+ex.Message);
            }
        }

        internal static User LogIn(string username, string password)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = @"SELECT *
                               FROM Users
                               Where [Username]=@Username and [Password]=@Password";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                using SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    User loggedUser = new User
                    {
                        Id = reader.GetInt32(0),
                        Picture = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Username = reader.GetString(2),
                        Age = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Email=reader.GetString(4),
                        Password=reader.GetString(5)
                    };
                    return loggedUser;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error logging in: "+ex.Message);
            }
        }

        internal static List<User> LoadUsers()
        {
            try
            {
                List<User> users=new List<User>();
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                string sql = @"SELECT *
                               FROM Users";
                using SqlCommand command = new SqlCommand(sql, connection);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Id = reader.GetInt32(0),
                        Picture = reader.IsDBNull(1) ? null : reader.GetString(1),
                        Username = reader.GetString(2),
                        Age = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Email = reader.GetString(4),
                        Password = reader.GetString(5)
                    });
                }
                return users;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error loading users: " + ex.Message);
            }
        }
    }
}
