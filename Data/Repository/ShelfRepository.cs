using Bookly.Data.InterfacesRepo;
using Microsoft.Extensions.Configuration;
using Models.Entities;
using Microsoft.Data.SqlClient;
using Models.Enums;

namespace Bookly.Data.Repository
{
    public class ShelfRepository: Repository, IShelfRepository
    {
        public ShelfRepository(IConfiguration configuration): base(configuration) { }

        public bool CreateShelf(RegularShelf shelf, int id)
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
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<RegularShelf> GetUserRegularShelves(User user)
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
                    shelves.Add(new RegularShelf(shelf.Id, shelf.Name,user, books));
                }
                return shelves;
        }

        public CurrentBookShelf? GetUserCurrentShelf(User user, string shelfName)
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
                return new CurrentBookShelf(reader.GetInt32(0), reader.GetString(1), user, GetBooksFromCurrentlyReadingShelf(user));
            }
            return null;
        }

        public List<Book> GetBooksFromShelf(int id)
        {
                List<Book> books = new List<Book>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT b.Id, b.Picture, b.Title, b.Author, b.[Description], b.ISBN, b.Genre, b.Pages 
                               FROM Books as b
                               INNER JOIN ShelfBook as sb
                               ON b.Id=sb.BookId
                               WHERE sb.ShelfId=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
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

        public RegularShelf? GetShelfById(int id)
        { 
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT s.Id, s.[Name], u.Id, u.Picture, u.Username, u.BirthDate, u.Email, u.[Password], u.RoleId
                               FROM Shelves as s
							   Inner Join Users as u
							   On u.Id = s.UserId
                               WHERE s.Id = @Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                     int shelfId = reader.GetInt32(0);
                return new RegularShelf(
                    shelfId,
                    reader.GetString(1),
                     new User(
                        reader.GetInt32(2),
                        reader.IsDBNull(3) ? null : reader.GetSqlBinary(3).Value,
                        reader.GetString(4),
                        reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5),
                        reader.GetString(6),
                        reader.GetString(7),
                        (Role)reader.GetInt32(8)),
                     GetBooksFromShelf(shelfId)
                     );
                }
            return null;
        }

        public bool AddBookToShelf(int bookId, int shelfId, int userId)
        {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                //RemoveBookFromShelf(userId, bookId);

                string sql = @"INSERT INTO ShelfBook(ShelfId, BookId)
                               VALUES (@ShelfId, @BookId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShelfId", shelfId);
                command.Parameters.AddWithValue("@BookId", bookId);

                command.ExecuteNonQuery();
                return true;
        }

        public List<CurrentBook> GetBooksFromCurrentlyReadingShelf(User user)
        {
                List<CurrentBook> currentBooks = new List<CurrentBook>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT b.Id, b.Picture, b.Title, b.Author,b.[Description], b.ISBN, b.Genre, b.Pages, ubp.Progress, ubp.StatusId
                                From Books as b
                                Inner join UserBookProgress as ubp
                                On b.Id = ubp.BookId
                                WHERE UserId = @UserId";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", user.Id);

                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    currentBooks.Add(new CurrentBook(
                        user,
                        reader.GetInt32(0), reader.GetString(1), reader.GetString(2), 
                        reader.GetString(3), reader.GetString(4), reader.GetString(5), 
                        (Genre)Enum.Parse(typeof(Genre), reader.GetString(6)), reader.GetInt32(7),
                        reader.GetInt32(8),
                        (Status)reader.GetInt32(9)
                        ));
                }
                return currentBooks;
        }

        public bool SetCurrentBookProgress(CurrentBook book)
        {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO UserBookProgress (UserId, BookId, Progress, StatusId)
                                VALUES (@UserId, @BookId, @Progress, @StatusId)";

                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", book.User.Id);
                command.Parameters.AddWithValue("@BookId", book.Id);
                command.Parameters.AddWithValue("@Progress", book.CurrentProgress);
                command.Parameters.AddWithValue("@StatusId", (int)book.Status);

                command.ExecuteNonQuery();
                return true;
        }

        public bool SaveCurrentBookProgress(CurrentBook book)
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
                command.Parameters.AddWithValue("@BookId", book.Id);
                command.Parameters.AddWithValue("@Progress", book.CurrentProgress);
                command.Parameters.AddWithValue("@StatusId", (int)book.Status);

                command.ExecuteNonQuery();
                return true;
        }

        public Shelf GetShelfContainingBook(int bookId, User user)
        {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT s.Id, s.Name
                                FROM Shelves as s
                                INNER JOIN ShelfBook as sb
                                ON sb.ShelfId = s.Id
                                WHERE s.UserId = @UserId and sb.BookId=@BookId and s.isArchived = @isArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@UserId", user.Id);
                command.Parameters.AddWithValue("@BookId", bookId);
                command.Parameters.AddWithValue("@isArchived", 0);

                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Shelf(
                        reader.GetInt32(0),
                        reader.GetString(1), 
                        user
                        );
                }
                return null;
        }

        public bool RemoveBookFromShelf(int userId, int bookId)
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
                return true;
        }

        public void RemoveFromCurrentBookShelf(int userId, int bookId)
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

        public bool RemoveShelf(int id)
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
                return true;
        }
    }
}
