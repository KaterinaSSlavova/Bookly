using Bookly.Data.InterfacesRepo;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models.Enums;

namespace Bookly.Data.Repository
{
    public class GoalRepository: Repository, IGoalRepository
    {
        public GoalRepository(IConfiguration configuration): base(configuration) { }

        public bool CreateGoal(Goal goal, int userId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
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
                using SqlConnection connection = GetSqlConnection();
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

        public Goal? GetNewestGoal(bool isIncreasing)
        {
            try
            {
                using SqlConnection connection= GetSqlConnection();
                connection.Open();

                string sql = @"SELECT TOP 1 Id, [Start], [End], ReadingGoal, CurrentProgress, [Status]
                                FROM Goals
                                WHERE isArchived=@isArchived";
                if (isIncreasing)
                {
                    sql += @" and [Status] <> @Status";
                }
                if(!isIncreasing)
                {
                    sql += @" and [Status] = @Status";
                }
                sql += @" ORDER BY [END] ASC";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 0);
                if (isIncreasing)
                {
                    command.Parameters.AddWithValue("@Status", Status.Completed.ToString());
                }
                if (!isIncreasing)
                {
                    command.Parameters.AddWithValue("@Status", Status.In_progress.ToString());
                }

                using SqlDataReader reader= command.ExecuteReader();
                Goal goal= null;    
                if (reader.Read())
                {
                    goal = new Goal
                    {
                        Id = reader.GetInt32(0),
                        Start = reader.GetDateTime(1),
                        End = reader.GetDateTime(2),
                        ReadingGoal = reader.GetInt32(3),
                        CurrentProgress = reader.GetInt32(4),
                        Status = (Status)Enum.Parse(typeof(Status), reader.GetString(5))
                    };
                }
                reader.Close();

                if(!isIncreasing && goal == null)
                {
                    sql = @"SELECT TOP 1 Id, [Start], [End], ReadingGoal, CurrentProgress, [Status]
                                    FROM Goals
                                    WHERE isArchived = @isArchived AND [Status] = @CompletedStatus
                                    ORDER BY [End] ASC";
                    using SqlCommand completeCommand = new SqlCommand(sql, connection);
                    completeCommand.Parameters.AddWithValue("@isArchived", 0);
                    completeCommand.Parameters.AddWithValue("@CompletedStatus", Status.Completed.ToString());

                    using SqlDataReader completedReader = completeCommand.ExecuteReader();
                    if (completedReader.Read())
                    {
                         goal = new Goal
                        {
                            Id = completedReader.GetInt32(0),
                            Start = completedReader.GetDateTime(1),
                            End = completedReader.GetDateTime(2),
                            ReadingGoal = completedReader.GetInt32(3),
                            CurrentProgress = completedReader.GetInt32(4),
                            Status = (Status)Enum.Parse(typeof(Status), completedReader.GetString(5))
                        };
                    }
                }

                return goal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateProgress(int userId, int goalId, int progress)
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
                command.Parameters.AddWithValue("@goalId", goalId);
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

 
    }
}