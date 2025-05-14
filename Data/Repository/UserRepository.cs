using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Enums;

namespace Bookly.Data.Repository
{
    public class UserRepository: Repository, IUserRepository
    {

        public UserRepository(IConfiguration configuration) : base(configuration) 
        { 
        }

        public void Register(User user)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string insertSql = @"INSERT INTO Users([Username], Email, [Password]) 
                               VALUES (@Username, @Email, @Password)";
                using SqlCommand commandInsert = new SqlCommand(insertSql, connection);
                commandInsert.Parameters.AddWithValue("@Username", user.Username);
                commandInsert.Parameters.AddWithValue("@Email", user.Email);
                commandInsert.Parameters.AddWithValue("@Password", user.Password);

                commandInsert.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public User LoadUser(string username)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                string sql = @"SELECT u.Id, u.Picture, u.Username, u.Email, u.[Password], u.BirthDate, u.RoleId
                                FROM Users as u
                                WHERE [Username] = @Username;";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Username", username);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                        (
                              reader.GetInt32(0),
                              reader.IsDBNull(1) ? null : reader.GetSqlBinary(1).Value,
                              reader.GetString(2),
                              reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                              reader.GetString(3),
                              reader.GetString(4),
                              (Role)reader.GetInt32(6)
                        );
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ;
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
                              reader.IsDBNull(1) ? null : reader.GetSqlBinary(1).Value,
                              reader.GetString(2),
                              reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                              reader.GetString(3),
                              reader.GetString(4),
                              (Role)reader.GetInt32(6)
                        );
                }
                return null;
            }
            catch(SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ;
            }
        }

        public void UpdateProfile(User newUser)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                
                string sql = @"UPDATE Users 
                               SET Picture = @Picture, 
                                [Username] = @Username,  
                                Email = @Email,
                                BirthDate = @BirthDate
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Picture", newUser.Picture);
                command.Parameters.AddWithValue("@Username", newUser.Username);
                command.Parameters.AddWithValue("@BirthDate", newUser.BirthDate);
                command.Parameters.AddWithValue("@Email", newUser.Email);
                command.Parameters.AddWithValue("@Id", newUser.Id);

                command.ExecuteNonQuery();
            }
            catch(SqlException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        
        public bool DoesUsernameExists(User user, int? excludedUserId = null)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                if(excludedUserId.HasValue) sql += " and Id <> @Id";

                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Username", user.Username);
                if (excludedUserId.HasValue) command.Parameters.AddWithValue("@Id", excludedUserId.Value);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
            catch(SqlException ex)
            {
                throw;
            }
        }

        public bool DoesEmailExists(User user, int? excludedUserId = null)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                if (excludedUserId.HasValue) sql += " and Id <> @Id";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                if (excludedUserId.HasValue) command.Parameters.AddWithValue("@Id", excludedUserId.Value);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
            catch (SqlException ex)
            {
                throw;
            }
        }
    }
}
