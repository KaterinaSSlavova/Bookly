using System.Reflection.PortableExecutable;
using Bookly.Models;

using Microsoft.Data.SqlClient;

namespace Bookly.Repository
{
    public class GoalRepository
    {
        private readonly string _connectionString;
        public GoalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool CreateGoal(Goal goal, int userId)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"INSERT INTO Goals ([Start], [End], ReadingGoal, CurrentProgress, [Status], UserId)
                            VALUES (@Start, @End, @ReadingGoal, @CurrentProgress, @Status, @UserId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Start", goal.Start.Date);
                command.Parameters.AddWithValue("@End", goal.End.Date);
                command.Parameters.AddWithValue("@ReadingGoal", goal.ReadingGoal);
                command.Parameters.AddWithValue("@CurrentProgress", 0);
                command.Parameters.AddWithValue("@Status", Status.Not_started.ToString());
                command.Parameters.AddWithValue("@UserId", userId);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Goal> GetPersonalGoals(int id)
        {
            try
            {
                List<Goal> goals = new List<Goal>();
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"SELECT Id, [Start], [End], ReadingGoal, CurrentProgress, [Status]
                                FROM Goals
                                WHERE UserId=@Id and isArchived=@isArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@isArchived",0);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    goals.Add(new Goal
                    {
                        Id = reader.GetInt32(0),
                        Start = reader.GetDateTime(1),
                        End = reader.GetDateTime(2),
                        ReadingGoal = reader.GetInt32(3),
                        CurrentProgress = reader.GetInt32(4),
                        Status = (Status)Enum.Parse(typeof(Status), reader.GetString(5))
                    });
                }
                return goals;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Goal? GetNewestGoal()
        {
            try
            {
                using SqlConnection connection= new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"SELECT TOP 1 Id, [Start], [End], ReadingGoal, CurrentProgress, [Status]
                                FROM Goals
                                WHERE isArchived=@isArchived and [Status] <> @Status
								ORDER BY [END] ASC";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Status", Status.Completed.ToString());
                command.Parameters.AddWithValue("@isArchived", 0);

                using SqlDataReader reader= command.ExecuteReader();
                if (reader.Read())
                {
                     return new Goal
                    {
                        Id = reader.GetInt32(0),
                        Start = reader.GetDateTime(1),
                        End = reader.GetDateTime(2),
                        ReadingGoal = reader.GetInt32(3),
                        CurrentProgress = reader.GetInt32(4),
                        Status = (Status)Enum.Parse(typeof(Status), reader.GetString(5))
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProgress(int userId, int goalId, int progress, Status status)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"UPDATE Goals
                                SET Status=@Status, CurrentProgress=@Progress
                                WHERE Id=@goalId and UserId=@UserId and [Status] <> @CurrentStatus";
                using SqlCommand command = new SqlCommand(sql,connection);
                command.Parameters.AddWithValue("@Status", status.ToString());
                command.Parameters.AddWithValue("@Progress", progress);
                command.Parameters.AddWithValue("@goalId", goalId);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@CurrentStatus", Status.Completed.ToString());

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateStatus(Status status, int goalId, int userId)
        {
            try
            {
                using SqlConnection connection= new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"UPDATE Goals
                                SET Status=@Status
                                WHERE Id=@goalId and UserId=@UserId";
                using SqlCommand command = new SqlCommand(sql,connection);
                command.Parameters.AddWithValue("@Status", status.ToString());
                command.Parameters.AddWithValue("@goalId", goalId);
                command.Parameters.AddWithValue("@UserId", userId);

                command.ExecuteNonQuery();  
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void RemoveGoal(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"Update Goals
                                SET isArchived = @isArchived
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@isArchived", 1);

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

 
    }
}