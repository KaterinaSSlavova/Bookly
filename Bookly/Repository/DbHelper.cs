using Bookly.Models;
using Bookly.ViewModels;
using Microsoft.Data.SqlClient;

namespace Bookly.Repository
{
    public class DbHelper
    {
        private const string connectionString = "Server=DESKTOP-GPBCRNQ;Database=BooklyDB;Trusted_Connection=True; TrustServerCertificate=True;";

        public static bool Register(AccountRegister user)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
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

        public static User? LogIn(string username, string password)
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

        public static User? LoadUser(string username)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
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

        public static bool UpdateProfile(User user, string picture, string newUsername, int age, string email, string password)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
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

        public static bool AddBooks(Book book)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string sql= @"INSERT INTO Books(Picture, Title, Author, Description, ISBN, Genre) 
                               VALUES (@Picture, @Title, @Author, @Description, @ISBN, @Genre)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Picture", book.Picture);
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Description);
                command.Parameters.AddWithValue("@Description", book.Description);
                command.Parameters.AddWithValue("@ISBN", book.ISBN);
                command.Parameters.AddWithValue("@Genre", book.Genre);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static List<Book> LoadBooks()
        {
            try
            {
                List<Book> books=new List<Book>();
                using SqlConnection connection=new SqlConnection(connectionString);
                connection.Open();

                string sql = @"SELECT *
                               FROM Books";
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetInt32(0),
                        Picture= Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                        Title= reader.GetString(2),
                        Author= reader.GetString(3),
                        Description= reader.GetString(4),
                        ISBN= reader.GetString(5),
                        Genre= reader.GetString(6)
                    });
                }
                return books;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static Book? GetBook(int id)
        {
            try
            {
                SqlConnection connection=new SqlConnection(connectionString);
                connection.Open();

                string sql = @"SELECT *
                               FROM Books 
                               WHERE Id=@Id";
                using SqlCommand command = new SqlCommand( sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader(); 
                if(reader.Read())
                {
                    return new Book
                    {
                        Id = reader.GetInt32(0),
                        Picture = Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                        Title = reader.GetString(2),
                        Author = reader.GetString(3),
                        Description = reader.GetString(4),
                        ISBN = reader.GetString(5),
                        Genre = reader.GetString(6)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static bool CreateShelf(string name, int id)
        {
            try
            {
                using SqlConnection connection=new SqlConnection(connectionString); 
                connection.Open();

                string sql = @"INSERT INTO Shelves ([Name], UserId)
                               VALUES (@Name, @UserId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@UserId", id);

                command.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static List<Shelf> GetUserShelves(int id)
        {
            try
            {
                List<Shelf> shelves = new List<Shelf>();
                using SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                string sql = @"SELECT * 
                               FROM Shelves
                               WHERE UserId = @Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using SqlDataReader reader = command.ExecuteReader();

                while(reader.Read())
                {
                    shelves.Add(
                        new Shelf
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                }
                return shelves;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static List<Book> GetBooksFromShelf(int id)
        {
            try
            {
                List<Book> books = new List<Book>();
                using SqlConnection connection=new SqlConnection(connectionString);
                connection.Open();

                string sql = @"SELECT * 
                               FROM Books as b
                               INNER JOIN ShelfBook as sb
                               ON b.Id=sb.BookId
                               WHERE sb.ShelfId=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id",id);
                using SqlDataReader reader = command.ExecuteReader();
                while(reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetInt32(0),
                        Picture = Convert.ToBase64String(reader.GetSqlBinary(1).Value),
                        Title = reader.GetString(2),
                        Author = reader.GetString(3),
                        Description = reader.GetString(4),
                        ISBN = reader.GetString(5),
                        Genre = reader.GetString(6)
                    });
                }
                return books;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static Shelf? GetShelfById(int id)
        {
            try
            {
                using SqlConnection connection=new SqlConnection(connectionString);
                connection.Open();

                string sql = @"SELECT *
                               FROM Shelves
                               WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using SqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    return new Shelf
                    {
                        Id = reader.GetInt32(0),
                        Name=reader.GetString(1)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public static bool AddBookToShelf(int bookId, int shelfId, int userId)
        {
            try
            {
                using SqlConnection connection=new SqlConnection(connectionString);   
                connection.Open();

                string deleteSql = @"DELETE sb
                                     FROM ShelfBook as sb 
                                     INNER JOIN Shelves as s
                                     ON sb.ShelfId=s.Id
                                     WHERE s.UserId=@UserId and sb.BookId=@BookId";
                using SqlCommand deleteCommand =new SqlCommand(deleteSql, connection);
                deleteCommand.Parameters.AddWithValue("@UserId", userId);
                deleteCommand.Parameters.AddWithValue("@BookId", bookId);

                deleteCommand.ExecuteNonQuery();

                string sql = @"INSERT INTO ShelfBook(ShelfId, BookId)
                               VALUES (@ShelfId, @BookId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShelfId", shelfId);
                command.Parameters.AddWithValue("@BookId", bookId);

                command.ExecuteNonQuery();
                return true;
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
