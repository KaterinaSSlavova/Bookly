using Interfaces;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Models.Enums;
using Microsoft.Extensions.Logging;
using Exceptions;

namespace Bookly.Data.Repository
{
    public class ShelfRepository: Repository, IShelfRepository
    {
        private readonly ILogger<ShelfRepository> _logger;
        public ShelfRepository(IConfiguration configuration, ILogger<ShelfRepository> logger): base(configuration) 
        {
            _logger = logger;
        }

        public void CreateShelf(Shelf shelf, int id)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO Shelves ([Name], UserId)
                               VALUES (@Name, @UserId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", shelf.Name);
                command.Parameters.AddWithValue("@UserId", id);

                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
				_logger.LogError(ex, "Sql error occurred while creating a shelf.");
				throw new RepositoryException("Could not create this shelf. Please try again later.");
			}
            catch (Exception ex)
            {
				_logger.LogError(ex, "Unexpected error occurred while creating a shelf.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
        }

        public List<RegularShelf> GetUserRegularShelves(User user)
        {
            try
            {
                List<RegularShelf> shelves = new List<RegularShelf>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT s.Id, s.[Name]
                                FROM Shelves as s
                                WHERE s.UserId = @Id and s.isArchived=@isArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@isArchived", 0);
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Shelf shelf = new Shelf(reader.GetInt32(0), reader.GetString(1), user);
                    List<Book> books = GetBooksFromShelf(shelf.Id);
                    shelves.Add(new RegularShelf(shelf, books));
                }
                return shelves;
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while loading user shelves.");
				throw new RepositoryException("Could not load user shelves. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while loading user shelves.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public CurrentBookShelf? GetUserCurrentShelf(User user, string shelfName)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT s.Id, s.[Name]
                                FROM Shelves as s
                                WHERE s.UserId = @Id and s.[Name] = @shelfName and s.isArchived=@isArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", user.Id);
                command.Parameters.AddWithValue("@shelfName", shelfName);
                command.Parameters.AddWithValue("@isArchived", 0);
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new CurrentBookShelf(new Shelf(reader.GetInt32(0), reader.GetString(1), user), GetBooksFromCurrentlyReadingShelf(user));
                }
                return null;
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while loading user's current shelf.");
				throw new RepositoryException("Could not load this shelf. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while loading user's current shelf.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public List<Book> GetBooksFromShelf(int id)
        {
            try
            {
                List<Book> books = new List<Book>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT b.Id, b.Picture, b.Title, b.Author, b.[Description], b.ISBN, b.Genre, b.Pages 
                               FROM Books as b
                               INNER JOIN ShelfBook as sb
                               ON b.Id=sb.BookId
                               WHERE sb.ShelfId=@Id and b.isArchived = @IsArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@IsArchived", 0);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new Book(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        (Genre)Enum.Parse(typeof(Genre), reader.GetString(6)),
                        reader.GetInt32(7)
                        ));
                }
                return books;
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while loading books from shelf.");
				throw new RepositoryException("Could not get the books from this shelf. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while loading books from shelf.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public RegularShelf? GetShelfById(int id)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT s.Id, s.[Name], u.Id, u.Picture, u.Username, u.BirthDate, u.Email, u.[Password], u.RoleId
                               FROM Shelves as s
							   Inner Join Users as u
							   On u.Id = s.UserId
                               WHERE s.Id = @Id and isArchived = @IsArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@IsArchived", 0);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    int shelfId = reader.GetInt32(0);
                    return new RegularShelf(
                        new Shelf(shelfId,
                        reader.GetString(1),
                         new User(
                            reader.GetInt32(2),
                            reader.IsDBNull(3) ? null : reader.GetSqlBinary(3).Value,
                            reader.GetString(4),
                            reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                            reader.GetString(6),
                            reader.GetString(7),
                            (Role)reader.GetInt32(8))),
                         GetBooksFromShelf(shelfId)
                         );
                }
                return null;
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while getting shelf by its id.");
				throw new RepositoryException("Could not load shelf. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while getting shelf by its id.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public void AddBookToShelf(int bookId, int shelfId, int userId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO ShelfBook(ShelfId, BookId)
                               VALUES (@ShelfId, @BookId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShelfId", shelfId);
                command.Parameters.AddWithValue("@BookId", bookId);

                command.ExecuteNonQuery();
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while adding book to shelf.");
				throw new RepositoryException("Could not add this book to the shelf. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while adding book to shelf.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public List<CurrentBook> GetBooksFromCurrentlyReadingShelf(User user)
        {
            try
            {
                List<CurrentBook> currentBooks = new List<CurrentBook>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT b.Id, b.Picture, b.Title, b.Author,b.[Description], b.ISBN, b.Genre, b.Pages, ubp.Progress, ubp.StatusId
                                From Books as b
                                Inner join UserBookProgress as ubp
                                On b.Id = ubp.BookId
                                WHERE UserId = @UserId and isArchived = @IsArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", user.Id);
                command.Parameters.AddWithValue("@IsArchived", 0);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    currentBooks.Add(new CurrentBook(
                        new Book(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                        reader.GetString(3), reader.GetString(4), reader.GetString(5),
                        (Genre)Enum.Parse(typeof(Genre), reader.GetString(6)), reader.GetInt32(7)),
                        user,
                        reader.GetInt32(8),
                        (Status)reader.GetInt32(9)
                        ));
                }
                return currentBooks;
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while loading books from current shelf.");
				throw new RepositoryException("Could not load books from this shelf. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while loading books from current shelf.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public void SetCurrentBookProgress(CurrentBook book)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO UserBookProgress (UserId, BookId, Progress, StatusId)
                                VALUES (@UserId, @BookId, @Progress, @StatusId)";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", book.User.Id);
                command.Parameters.AddWithValue("@BookId", book.Book.Id);
                command.Parameters.AddWithValue("@Progress", book.CurrentProgress);
                command.Parameters.AddWithValue("@StatusId", (int)book.Status);

                command.ExecuteNonQuery();
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while setting current books progress.");
				throw new RepositoryException("Could not set book progress. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while setting current book progress.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public void SaveCurrentBookProgress(CurrentBook book)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"UPDATE UserBookProgress
                                SET UserId = @UserId,
                                    BookId = @BookId,
                                    Progress = @Progress,
                                    StatusId = @StatusId";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", book.User.Id);
                command.Parameters.AddWithValue("@BookId", book.Book.Id);
                command.Parameters.AddWithValue("@Progress", book.CurrentProgress);
                command.Parameters.AddWithValue("@StatusId", (int)book.Status);

                command.ExecuteNonQuery();
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while updating book progress.");
				throw new RepositoryException("Could not save book progress. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while updating book progress.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public void RemoveBookFromShelf(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"DELETE sb
                               FROM ShelfBook as sb 
                               INNER JOIN Shelves as s
                               ON sb.ShelfId=s.Id
                               WHERE s.UserId=@UserId and sb.BookId=@BookId";
                using SqlCommand deleteCommand = new SqlCommand(sql, connection);
                deleteCommand.Parameters.AddWithValue("@UserId", userId);
                deleteCommand.Parameters.AddWithValue("@BookId", bookId);

                deleteCommand.ExecuteNonQuery();
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while removing book from shelf.");
				throw new RepositoryException("Could not remove this book. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while removing book from shelf.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public void RemoveFromCurrentBookShelf(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"DELETE
                               FROM UserBookProgress
                               WHERE UserId=@UserId and BookId=@BookId";
                using SqlCommand deleteCommand = new SqlCommand(sql, connection);
                deleteCommand.Parameters.AddWithValue("@UserId", userId);
                deleteCommand.Parameters.AddWithValue("@BookId", bookId);

                deleteCommand.ExecuteNonQuery();
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while removing book from current shelf.");
				throw new RepositoryException("Could not remove this book. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while removing book from current shelf");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}

        public void RemoveShelf(int id)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"UPDATE Shelves
                                SET isArchived=@isArchived
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 1);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
			catch (SqlException ex)
			{
				_logger.LogError(ex, "Sql error occurred while removing shelf.");
				throw new RepositoryException("Could not remove this shelf. Please try again later.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unexpected error occurred while removing shelf.");
				throw new RepositoryException("An unexpected error occurred. Please try again later.");
			}
		}
    }
}
