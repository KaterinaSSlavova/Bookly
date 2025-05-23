//using Interfaces;
//using Models.Entities;
//using Microsoft.Data.SqlClient;
//using Microsoft.Extensions.Configuration;
//using Models.Enums;
//using Microsoft.Extensions.Logging;
//using Exceptions;

//namespace Bookly.Data.Repository
//{
//    public class BookRepository: Repository, IBookRepository
//    {
//        private readonly ILogger<BookRepository> _logger;
//        public BookRepository(IConfiguration configuration, ILogger<BookRepository> logger) : base(configuration)
//        {
//            _logger = logger;
//        }

//        public void AddBook(Book book)
//        {
//            try
//            {
//                string picturePath = GetPicturePath(book.Picture);
//                using SqlConnection connection = GetSqlConnection();
//                connection.Open();

//                string sql = @"INSERT INTO Books(Picture, Title, Author, Description, ISBN, Genre, Pages) 
//                               VALUES (@Picture, @Title, @Author, @Description, @ISBN, @Genre, @Pages)";
//                using SqlCommand command = new SqlCommand(sql, connection);
//                command.Parameters.AddWithValue("@Picture", picturePath);
//                command.Parameters.AddWithValue("@Title", book.Title);
//                command.Parameters.AddWithValue("@Author", book.Author);
//                command.Parameters.AddWithValue("@Description", book.Description);
//                command.Parameters.AddWithValue("@ISBN", book.ISBN);
//                command.Parameters.AddWithValue("@Genre", book.Genre.ToString());
//                command.Parameters.AddWithValue("Pages", book.Pages);

//                command.ExecuteNonQuery();
//            }
//            catch (SqlException ex)
//            {
//                _logger.LogError(ex, "Sql error occurred while adding a book.");
//                throw new RepositoryException("Could not save the book. Please try again later.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unexpected error occurred while adding a book.");
//                throw new RepositoryException("An unexpected error occurred. Please try again later.");
//            }
//        }

//        public List<Book> LoadBooks()
//        {
//            try
//            {
//                List<Book> books = new List<Book>();
//                using SqlConnection connection = GetSqlConnection();
//                connection.Open();

//                string sql = @"SELECT Id, Picture, Title, Author, [Description], ISBN, Genre, Pages
//                               FROM Books
//                                WHERE isArchived=@IsArchived";
//                using SqlCommand command = new SqlCommand(sql, connection);
//                command.Parameters.AddWithValue("@IsArchived",0);
//                using SqlDataReader reader = command.ExecuteReader();
//                while (reader.Read())
//                {
//                    books.Add(new Book(
//                        reader.GetInt32(0),
//                        reader.GetString(1),
//                        reader.GetString(2),
//                        reader.GetString(3),
//                        reader.GetString(4),
//                        reader.GetString(5),
//                        (Genre)Enum.Parse(typeof(Genre), reader.GetString(6)),
//                        reader.GetInt32(7)
//                        ));
//                }
//                return books;
//            }
//            catch (SqlException ex)
//            {
//                _logger.LogError(ex, "Sql error occurred while loading all books.");
//                throw new RepositoryException("Could not load all books. Please try again later.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unexpected error occurred while loading all books.");
//                throw new RepositoryException("An unexpected error occurred while loading all books. Please try again later.");
//            }
//        }

//        public Book? GetBookById(int id)
//        {
//            try
//            {
//                using SqlConnection connection = GetSqlConnection();
//                connection.Open();

//                string sql = @"SELECT Id, Picture, Title, Author, [Description], ISBN, Genre, Pages
//                               FROM Books 
//                               WHERE Id=@Id";
//                using SqlCommand command = new SqlCommand(sql, connection);
//                command.Parameters.AddWithValue("@Id", id);

//                SqlDataReader reader = command.ExecuteReader();
//                if (reader.Read())
//                {
//                    return new Book(
//                        reader.GetInt32(0),
//                        reader.GetString(1),
//                        reader.GetString(2),
//                        reader.GetString(3),
//                        reader.GetString(4),
//                        reader.GetString(5),
//                        (Genre)Enum.Parse(typeof(Genre), reader.GetString(6)),
//                        reader.GetInt32(7)
//                        );
//                }
//                return null;
//            }
//            catch (SqlException ex)
//            {
//                _logger.LogError(ex, "Sql error occurred while getting a book by its id.");
//                throw new RepositoryException("Could not load this book. Please try again later.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unexpected error occurred while getting a book by its id.");
//                throw new RepositoryException("An unexpected error occurred while loading this book. Please try again later.");
//            }
//        }

//        public void UpdateBook(Book book)
//        {
//            try
//            {
//                using SqlConnection connection = GetSqlConnection();
//                connection.Open();

//                string sql = @"UPDATE Books
//                                 SET Picture = @Picture,
//                                 Title = @Title,
//                                 Author = @Author,
//                                 Description = @Description,
//                                 ISBN = @ISBN,
//                                 Genre = @Genre,
//                                 Pages = @Pages
//                                 WHERE Id = @Id";
//                using SqlCommand command = new SqlCommand(sql, connection);
//                command.Parameters.AddWithValue("@Id", book.Id);
//                command.Parameters.AddWithValue("@Picture", book.Picture);
//                command.Parameters.AddWithValue("@Title", book.Title);
//                command.Parameters.AddWithValue("@Author", book.Author);
//                command.Parameters.AddWithValue("@Description", book.Description);
//                command.Parameters.AddWithValue("@ISBN", book.ISBN);
//                command.Parameters.AddWithValue("@Genre", book.Genre);
//                command.Parameters.AddWithValue("@Pages", book.Pages);

//                command.ExecuteNonQuery();
//            }
//            catch (SqlException ex)
//            {
//                _logger.LogError(ex, "Sql error occurred while updating a book");
//                throw new RepositoryException("Could not update this book. Please try again later.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unexpected error occurred while updating a book.");
//                throw new RepositoryException("An unexpected error occurred while updating a book. Please try again later.");
//            }
//        }

//        public void RemoveBook(int id)
//        {
//            try
//            {
//                using SqlConnection connection = GetSqlConnection();
//                connection.Open();

//                string sql = @"UPDATE Books
//                                SET isArchived = @isArchived
//                                WHERE Id=@Id";
//                using SqlCommand command = new SqlCommand(sql, connection);
//                command.Parameters.AddWithValue("@isArchived", 1);
//                command.Parameters.AddWithValue("@Id", id);

//                command.ExecuteNonQuery();
//            }
//            catch (SqlException ex)
//            {
//                _logger.LogError(ex, "Sql error occurred while removing a book");
//                throw new RepositoryException("Could not remove this book. Please try again later.");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unexpected error occurred while removing a book.");
//                throw new RepositoryException("An unexpected error occurred while removing a book. Please try again later.");
//            }
//        }

//        private string GetPicturePath(string picture)
//        {
//            string path="/images/" + picture;
//            return path;
//        }
//    }
//}
