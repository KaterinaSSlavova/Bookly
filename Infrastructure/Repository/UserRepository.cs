using Interfaces;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Enums;
using Microsoft.Extensions.Logging;
using Exceptions;

namespace Bookly.Data.Repository
{
    public class UserRepository: Repository, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(IConfiguration configuration, ILogger<UserRepository> logger) : base(configuration) 
        { 
            _logger = logger;
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
                //_logger.LogError(ex, "Sql error occurred while registering a user.");

                _logger.LogError(
                    ex,
                    "SQL error occurred while registering user {Username}, {Email}",
                    user.Username,
                    user.Email
                );
                throw new RepositoryException("Could not create an account. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while registering a user.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
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
				_logger.LogError(ex, "Sql error occurred while loading user.");
				throw new RepositoryException("Could not load this user. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while loading user.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
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
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while getting user by its id.");
				throw new RepositoryException("Could not get this user. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while getting user by its id.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
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
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while updating a profile.");
				throw new RepositoryException("Could not update this profile. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while updating a profile.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}
        
        public bool DoesUsernameExists(User user)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                if(user.Id != null || user.Id != 0) sql += " and Id <> @Id";

                using SqlCommand command = new SqlCommand(sql, connection);

                command.Parameters.AddWithValue("@Username", user.Username);
                if (user.Id != null || user.Id != 0) command.Parameters.AddWithValue("@Id", user.Id);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while checking for username.");
				throw new RepositoryException("Could not check for username. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while checking for username.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public bool DoesEmailExists(User user)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                if (user.Id != null || user.Id != 0) sql += " and Id <> @Id";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Email", user.Email);
                if (user.Id != null || user.Id !=0) command.Parameters.AddWithValue("@Id", user.Id);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while checking for email.");
				throw new RepositoryException("Could not checking for email. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while checking for email.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}
    }
}
