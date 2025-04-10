using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Enums;

namespace Bookly.Data.Repository
{
    public class GoalRepository: Repository, IGoalRepository
    {
        public GoalRepository(IConfiguration configuration): base(configuration) 
        {    
        }

        public bool CreateGoal(Goal goal)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO Goals ([Start], [End], ReadingGoal, CurrentProgress, [Status], UserId)
                            VALUES (@Start, @End, @ReadingGoal, @CurrentProgress, @Status, @UserId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Start", goal.Start.Date);
                command.Parameters.AddWithValue("@End", goal.End);
                command.Parameters.AddWithValue("@ReadingGoal", goal.ReadingGoal);
                command.Parameters.AddWithValue("@CurrentProgress", 0);
                command.Parameters.AddWithValue("@Status", goal.Status.ToString());
                command.Parameters.AddWithValue("@UserId", goal.User.Id);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Goal GetGoalById(User user, int goalId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT Id, [Start], [End], ReadingGoal, CurrentProgress, [Status], UserId
                                FROM Goals
                                WHERE isArchived = @isArchived and UserId = @UserId and Id = @Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 0);
                command.Parameters.AddWithValue("@UserId", user.Id);
                command.Parameters.AddWithValue("@Id", goalId);

                using SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    return new Goal
                        (
                                reader.GetInt32(0),
                                reader.GetDateTime(1),
                                reader.GetDateTime(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                (Status)Enum.Parse(typeof(Status), reader.GetString(5)),
                                user
                        );
                }
                return null;
            }
            catch(Exception ex) 
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Goal> GetPersonalGoals(User user)
        {
            try
            {
                List<Goal> goals = new List<Goal>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT Id, [Start], [End], ReadingGoal, CurrentProgress, [Status]
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
                                (Status)Enum.Parse(typeof(Status), reader.GetString(5)),
                                user
                        ));
                }
                return goals;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Goal? GetNewestGoal(bool isIncreasing, User user)
        {
            try
            {
                using SqlConnection connection= GetSqlConnection();
                connection.Open();

                string sql = @"SELECT TOP 1 Id, [Start], [End], ReadingGoal, CurrentProgress, [Status]
                                FROM Goals
                                WHERE isArchived=@isArchived AND UserId = @Id";
                if (isIncreasing)
                {
                    sql += @" AND [Status] <> @Status AND [Status] <> @StatusExp";
                }
                if(!isIncreasing)
                {
                    sql += @" AND [Status] = @Status";
                }
                sql += @" ORDER BY [END] ASC";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 0);
                command.Parameters.AddWithValue("@Id", user.Id);
                if (isIncreasing)
                {
                    command.Parameters.AddWithValue("@Status", Status.Completed.ToString());
                    command.Parameters.AddWithValue("@StatusExp", Status.Expired.ToString());
                }
                if (!isIncreasing)
                {
                    command.Parameters.AddWithValue("@Status", Status.In_progress.ToString());
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
                                (Status)Enum.Parse(typeof(Status), reader.GetString(5)),
                                user
                        );
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Goal? GetLatestCompletedGoal(User user)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();
                string sql = @"SELECT TOP 1 Id, [Start], [End], ReadingGoal, CurrentProgress, [Status]
                                    FROM Goals
                                    WHERE isArchived = @isArchived AND [Status] = @CompletedStatus AND UserId = @Id
                                    ORDER BY [End] ASC";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 0);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@CompletedStatus", Status.Completed.ToString());

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
                        (Status)Enum.Parse(typeof(Status), reader.GetString(5)),
                        user
                    );
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProgress(int userId, Goal goal, int progress)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"UPDATE Goals
                                SET CurrentProgress=@Progress
                                WHERE Id=@goalId and UserId=@UserId and CurrentProgress <= ReadingGoal";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Progress", progress);
                command.Parameters.AddWithValue("@goalId", goal.Id);
                command.Parameters.AddWithValue("@UserId", userId);

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
                using SqlConnection connection= GetSqlConnection();
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

        public bool RemoveGoal(int id)
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
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

 
    }
}