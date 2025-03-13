using Bookly.Models;
using Microsoft.Data.SqlClient;

namespace Bookly.Repository
{
    public class BookRepository
    {
        private readonly string _connectionString;

        public BookRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public bool AddBook(Book book)
        {
            try
            {
                string picturePath = GetPicturePath(book.Picture);
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"INSERT INTO Books(Picture, Title, Author, Description, ISBN, Genre) 
                               VALUES (@Picture, @Title, @Author, @Description, @ISBN, @Genre)";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Picture", picturePath);
                command.Parameters.AddWithValue("@Title", book.Title);
                command.Parameters.AddWithValue("@Author", book.Author);
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

        public List<Book> LoadBooks()
        {
            try
            {
                List<Book> books = new List<Book>();
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"SELECT *
                               FROM Books
                                WHERE isArchived=@IsArchived";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@IsArchived",0);
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
                        Genre = reader.GetString(6)
                    });
                }
                return books;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }

        public Book? GetBookById(int id)
        {
            try
            {
                SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"SELECT *
                               FROM Books 
                               WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", id);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Book
                    {
                        Id = reader.GetInt32(0),
                        Picture = reader.GetString(1),
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

        public void RemoveBook(int id)
        {
            try
            {
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string sql = @"UPDATE Books
                                SET isArchived = @isArchived
                                WHERE Id=@Id";
                using SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@isArchived", 1);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string GetPicturePath(string picture)
        {
            string path="/images/" + picture;
            return path;
        }
    }
}
