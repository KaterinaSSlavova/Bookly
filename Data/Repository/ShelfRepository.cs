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

        public bool CreateShelf(string name, int id)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"INSERT INTO Shelves ([Name], UserId)
                               VALUES (@Name, @UserId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@UserId", id);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public List<Shelf> GetUserShelves(int id)
        {
            try
            {
                List<Shelf> shelves = new List<Shelf>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT * 
                               FROM Shelves
                               WHERE UserId = @Id and isArchived=@isArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@isArchived", 0);
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
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

        public List<Book> GetBooksFromShelf(int id)
        {
            try
            {
                List<Book> books = new List<Book>();
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT * 
                               FROM Books as b
                               INNER JOIN ShelfBook as sb
                               ON b.Id=sb.BookId
                               WHERE sb.ShelfId=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    books.Add(new Book
                    {
                        Id = reader.GetInt32(0),
                        Picture = reader.GetString(1),
                        Title = reader.GetString(2),
                        Author = reader.GetString(3),
                        Description = reader.GetString(4),
                        ISBN = reader.GetString(5),
                        Genre = (Genre)Enum.Parse(typeof(Genre), reader.GetString(6))
                    });
                }
                return books;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public Shelf? GetShelfById(int id)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                string sql = @"SELECT *
                               FROM Shelves
                               WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);
                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Shelf
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public bool AddBookToShelf(int bookId, int shelfId, int userId)
        {
            try
            {
                using SqlConnection connection = GetSqlConnection();
                connection.Open();

                RemoveBookFromShelf(userId, bookId);

                string sql = @"INSERT INTO ShelfBook(ShelfId, BookId)
                               VALUES (@ShelfId, @BookId)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@ShelfId", shelfId);
                command.Parameters.AddWithValue("@BookId", bookId);

                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public bool RemoveBookFromShelf(int userId, int bookId)
        {
            try
            {
                using SqlConnection connection= GetSqlConnection();
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
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message); 
            }
        }

        public void RemoveShelf(int id)
        {
            try
            {
                using SqlConnection connection= GetSqlConnection();
                connection.Open();

                string sql = @"UPDATE Shelves
                                SET isArchived=@isArchived
                                WHERE Id=@Id";
                using SqlCommand command= new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived",1);
                command.Parameters.AddWithValue("@Id",id);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
    }
}
