using Interfaces;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Enums;
using Exceptions;
using Microsoft.Extensions.Logging;

namespace Bookly.Data.Repository
{
    public class GoalRepository: Repository, IGoalRepository
    {
        private readonly ILogger<GoalRepository> _logger;
        public GoalRepository(IConfiguration configuration, ILogger<GoalRepository> logger): base(configuration) 
        {    
            _logger = logger;
        }

        public void CreateGoal(Goal goal)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO Goals ([Start], [End], ReadingGoal, CurrentProgress, StatusId, UserId)
                            VALUES (@Start, @End, @ReadingGoal, @CurrentProgress, @StatusId, @UserId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Start", goal.Start.Date);
                command.Parameters.AddWithValue("@End", goal.End);
                command.Parameters.AddWithValue("@ReadingGoal", goal.ReadingGoal);
                command.Parameters.AddWithValue("@CurrentProgress", 0);
                command.Parameters.AddWithValue("@StatusId", (int)Status.Not_started);
                command.Parameters.AddWithValue("@UserId", goal.User.Id);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while creating a goal.");
                throw new RepositoryException("Could not create this goal. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating a goal.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public List<Goal> GetPersonalGoals(User user)
        {
            try
            {
                List<Goal> goals = new List<Goal>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT Id, [Start], [End], ReadingGoal, CurrentProgress, StatusId
                                FROM Goals
                                WHERE UserId=@Id and isArchived=@isArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@isArchived",0);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    goals.Add(new Goal
                        (
                                reader.GetInt32(0),
                                reader.GetDateTime(1),
                                reader.GetDateTime(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                (Status)reader.GetInt32(5),
                                user
                        ));
                }
                return goals;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while loading a goal by its id.");
                throw new RepositoryException("Could not load that goal. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while loading a goal by its id.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public Goal? GetNewestGoal(bool isIncreasing, User user)
        {
            try
            {
                using SqlConnection connection= GetSqlConnection();
                connection.Open();

                string sql = @"SELECT TOP 1 Id, [Start], [End], ReadingGoal, CurrentProgress, StatusId
                                FROM Goals
                                WHERE isArchived=@isArchived AND UserId = @Id";
                if (isIncreasing)
                {
                    sql += @" AND StatusId <> @Status AND StatusId <> @StatusExp";
                }
                if(!isIncreasing)
                {
                    sql += @" AND StatusId = @Status";
                }
                sql += @" ORDER BY [END] ASC";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 0);
                command.Parameters.AddWithValue("@Id", user.Id);
                if (isIncreasing)
                {
                    command.Parameters.AddWithValue("@Status", (int)Status.Completed);
                    command.Parameters.AddWithValue("@StatusExp", (int)Status.Expired);
                }
                if (!isIncreasing)
                {
                    command.Parameters.AddWithValue("@Status", (int)Status.In_progress);
                }

                using SqlDataReader reader= command.ExecuteReader(); 
                if (reader.Read())
                {
                         return new Goal
                        (
                                reader.GetInt32(0),
                                reader.GetDateTime(1),
                                reader.GetDateTime(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                (Status)reader.GetInt32(5),
                                user
                        );
                }
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while loading the newest goal.");
                throw new RepositoryException("Could not load the newest goal. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while loading the newest goal.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public Goal? GetLatestCompletedGoal(User user)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                string sql = @"SELECT TOP 1 Id, [Start], [End], ReadingGoal, CurrentProgress, StatusId
                                    FROM Goals
                                    WHERE isArchived = @isArchived AND [StatusId] = @CompletedStatus AND UserId = @Id
                                    ORDER BY [End] ASC";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 0);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@CompletedStatus", (int)Status.Completed);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Goal
                    (
                        reader.GetInt32(0),
                        reader.GetDateTime(1),
                        reader.GetDateTime(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        (Status)reader.GetInt32(5),
                        user
                    );
                }
                return null;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while loading the latest completed goal.");
                throw new RepositoryException("Could not load that goal. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while loading the latest completed goal.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public void UpdateGoal(Goal goal)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"UPDATE Goals
                                SET CurrentProgress=@Progress,
                                StatusId = @Status
                                WHERE Id=@goalId and UserId=@UserId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Progress", goal.CurrentProgress);
                command.Parameters.AddWithValue("@Status", (int)goal.Status);
                command.Parameters.AddWithValue("@goalId", goal.Id);
                command.Parameters.AddWithValue("@UserId", goal.User.Id);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while updating the progress of a goal.");
                throw new RepositoryException("Could not update the progress of your goal. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating the progress of a goal.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }

        public void RemoveGoal(int id)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"Update Goals
                                SET isArchived = @isArchived
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@isArchived", 1);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while removing a goal.");
                throw new RepositoryException("Could not remove this goal. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while removing a goal.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }
        
        public async Task<List<Goal>> GetAllGoals()
        {
            List<Goal> goals = new List<Goal>(); 
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT g.Id, g.[Start], g.[End], g.ReadingGoal, g.CurrentProgress, g.StatusId, u.Username, u.BirthDate, u.Email, u.RoleId
                                FROM Goals as g
								INNER JOIN Users as u
								ON u.Id = g.UserId
                                WHERE isArchived=0";
                using SqlCommand command = new SqlCommand(sql,connection);
                command.Parameters.AddWithValue("@isArchived", 0);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    goals.Add(new Goal
                    (
                        reader.GetInt32(0),
                        reader.GetDateTime(1),
                        reader.GetDateTime(2),
                        reader.GetInt32(3),
                        reader.GetInt32(4),
                        (Status)reader.GetInt32(5),
                        new User(reader.GetString(6), reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(7), reader.GetString(8), (Role)reader.GetInt32(9))
                    ));
                }
                return goals;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Sql error occurred while loading all goals.");
                throw new RepositoryException("Could not load all goals. Please try again later.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while loading all goals.");
                throw new RepositoryException("An unexpected error occurred. Please try again later.");
            }
        }
    }
}