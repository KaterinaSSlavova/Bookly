using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Bookly.Data.Repository
{
    public class UserRepository: Repository,IUserRepository
    {
        private readonly IShelfRepository _ishelfRepo;

        public UserRepository(IConfiguration configuration, IShelfRepository ishelfRepo) : base(configuration) 
        { 
            this._ishelfRepo = ishelfRepo;
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
                    {
                        Id = reader.GetInt32(0),
                        Picture = reader.IsDBNull(1) ? null : Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                        Username = reader.GetString(2),
                        Age = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Email = reader.GetString(4),
                        Password = reader.GetString(5)
                    };
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
                    {
                        Id = reader.GetInt32(0),
                        Picture = reader.IsDBNull(1) ? null : Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                        Username = reader.GetString(2),
                        Age = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Email = reader.GetString(4),
                        Password = reader.GetString(5)
                    };
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
                    {
                        Id = reader.GetInt32(0),
                        Picture = reader.IsDBNull(1) ? null : Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                        Username = reader.GetString(2),
                        Age = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                        Email = reader.GetString(4),
                        Password = reader.GetString(5)
                    };
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public bool UpdateProfile(User user, byte[] picture, string newUsername, int age, string email, string password)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                
                string sql = @"UPDATE Users 
                               SET Picture = @Picture, 
                                [Username] = @Username, 
                                Age = @Age, 
                                Email = @Email, 
                                Password = @Password
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Picture", picture);
                command.Parameters.AddWithValue("@Username", newUsername);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Id", user.Id);

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
