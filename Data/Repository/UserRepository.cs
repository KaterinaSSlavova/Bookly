using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace Bookly.Data.Repository
{
    public class UserRepository: Repository,IUserRepository
    {

        public UserRepository(IConfiguration configuration) : base(configuration) 
        { 
        }

        public bool Register(User user)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                string checkSql = @"SELECT COUNT(*) FROM Users WHERE Username = @Username or Email=@Email";
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
                using SqlCommand commandInsert = new SqlCommand(insertSql, connection);
                commandInsert.Parameters.AddWithValue("@Username", user.Username);
                commandInsert.Parameters.AddWithValue("@Email", user.Email);
                commandInsert.Parameters.AddWithValue("@Password", user.Password);

                commandInsert.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error registering user: " + ex.Message);
            }
        }

        public User? LogIn(User user)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                string sql = @"SELECT *
                               FROM Users
                               Where [Username]=@Username and [Password]=@Password";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                        (
                              reader.GetInt32(0),
                              reader.IsDBNull(1) ? null : Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                              reader.GetString(2),
                              reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                              reader.GetString(4),
                              reader.GetString(5)
                        );
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error logging in: " + ex.Message);
            }
        }

        public User? LoadUser(string username)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                string sql = @"SELECT *
                               FROM Users
                               Where [Username]=@Username";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", username);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                        (
                              reader.GetInt32(0),
                              reader.IsDBNull(1) ? null : Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                              reader.GetString(2),
                              reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                              reader.GetString(4),
                              reader.GetString(5)
                        );
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public User? GetUserById(int id)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT *
                                FROM Users
                                WHERE Id = @Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                        (
                              reader.GetInt32(0),
                              reader.IsDBNull(1) ? null : Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                              reader.GetString(2),
                              reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                              reader.GetString(4),
                              reader.GetString(5)
                        );
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public bool UpdateProfile(User newUser, byte[] image)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                
                string sql = @"UPDATE Users 
                               SET Picture = @Picture, 
                                [Username] = @Username, 
                                Age = @Age, 
                                Email = @Email 
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Picture", image);
                command.Parameters.AddWithValue("@Username", newUser.Username);
                command.Parameters.AddWithValue("@Age", newUser.Age);
                command.Parameters.AddWithValue("@Email", newUser.Email);
                command.Parameters.AddWithValue("@Id", newUser.Id);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        
        public List<string> GetAllUsernames(User user)
        {
            try
            {
                List<string> usernames = new List<string>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT Username 
                                FROM Users
                                WHERE Id <> @Id";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", user.Id);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    usernames.Add(reader.GetString(0));
                }
                return usernames;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<string> GetAllEmails(User user)
        {
            try
            {
                List<string> emails = new List<string>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT Email 
                                FROM Users
                                WHERE Id <> @Id";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", user.Id);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    emails.Add(reader.GetString(0));
                }
                return emails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
