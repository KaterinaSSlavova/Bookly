using AutoMapper;
using Bookly.Business_logic.Services;
using Bookly.Data.InterfacesRepo;
using Business_logic.DTOs;
using Business_logic.Mappers;
using Models.Entities;
using Models.Enums;
using Moq;

namespace Tests
{
    [TestClass]
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepo;
        private readonly IMapper _mapper;
        private readonly BookServices _bookService;

        public BookServiceTests()
        {
            _bookRepo = new Mock<IBookRepository>();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BookMapper());
            });
            _mapper = config.CreateMapper();
            _bookService = new BookServices(_bookRepo.Object, _mapper);
        }

        [TestMethod]
        public void AddBook_ShouldReturnTrue_WhenBookIsAddedSuccessfully()
        {
            //Arrange
            BookDTO bookDTO = new BookDTO
            {
                Id = 1,
                Picture = "/images/book.png",
                Title = "Happy Ending",
                Author = "Hope Writer",
                Description = "A story with a perfect ending.",
                ISBN = "999-9-99999-999-9",
                Genre = Genre.Romance,
                Pages = 300
            }; 
            _bookRepo.Setup(r => r.AddBook(It.IsAny<Book>())).Returns(true);

            //Act
            bool isAdded = _bookService.AddBook(bookDTO);

            //Assert
            Assert.IsTrue(isAdded);
        }

        [TestMethod]
        public void AddBook_ShouldReturnFalse_WhenBookIsNull()
        {
            //Arrange
            BookDTO? book = null;

            //Act
            bool isAdded = _bookService.AddBook(book);

            //Assert
            Assert.IsFalse(isAdded);
        }

        [TestMethod]
        public void AddBook_ShouldReturnFalse_WhenBookHasZeroPages()
        {
            //Arrange
            BookDTO bookDTO = new BookDTO
            {
                Id = 1,
                Picture = "/images/book.png",
                Title = "Happy Ending",
                Author = "Hope Writer",
                Description = "A story with a perfect ending.",
                ISBN = "999-9-99999-999-9",
                Genre = Genre.Romance,
                Pages = 0
            };

            //Act
            bool isAdded = _bookService.AddBook(bookDTO);

            //Assert
            Assert.IsFalse(isAdded);
        }

        [TestMethod]
        public void AddBook_ShouldReturnFalse_WhenBookWithTheSameISBNExists()
        {
            //Arrange
            BookDTO newBookDTO = new BookDTO
            {
                Id = 1,
                Picture = "/images/book.png",
                Title = "Happy Ending",
                Author = "Hope Writer",
                Description = "A story with a perfect ending.",
                ISBN = "999-9-99999-999-9",
                Genre = Genre.Romance,
                Pages = 100
            };

            List<Book> existingBooks = new List<Book>() { new Book(1, "/images/book1.png", "Book1", "Author1", "Description1", "999-9-99999-999-9", Genre.Thriller, 100) };
            _bookRepo.Setup(r => r.LoadBooks()).Returns(existingBooks);

            //Act
            bool isAdded = _bookService.AddBook(newBookDTO);

            //Assert
            Assert.IsFalse(isAdded);
        }

        [TestMethod]
        public void LoadBooks_ShouldReturnAllBooksFromTheDatabase_WhenBooksExist()
        {
            //Arrange 
            List<Book> allBooks = new List<Book>()
            {
                 new Book(1, "/images/book.jpg", "The Mockingbird Code", "Jane Devlin", "A thrilling journey into the world of AI and espionage.", "978-1-23456-789-7", Genre.Thriller, 384),
                 new Book(2, "/images/book2.jpg", "Empty Shell", "No One", "Empty book", "000-0-00000-000-0", Genre.Mystery, 1),
                 new Book(3, "/images/book3.jpg", "Just Words", "Ghost Writer", "Another mystery book", "111-1-11111-111-1", Genre.Fantasy, 150)
            };
            _bookRepo.Setup(r => r.LoadBooks()).Returns(allBooks);

            //Act
            List<BookDTO> allBooksDTO = _bookService.LoadBooks();

            //Assert
            Assert.IsNotNull(allBooksDTO);
            Assert.AreEqual(allBooks.Count, allBooksDTO.Count);
            for (int i=0; i<allBooks.Count; i++)
            {
                Assert.AreEqual(allBooks[i].Id, allBooksDTO[i].Id);
                Assert.AreEqual(allBooks[i].Picture, allBooksDTO[i].Picture);
                Assert.AreEqual(allBooks[i].Title, allBooksDTO[i].Title);
                Assert.AreEqual(allBooks[i].Author, allBooksDTO[i].Author);
                Assert.AreEqual(allBooks[i].Genre, allBooksDTO[i].Genre);
                Assert.AreEqual(allBooks[i].ISBN, allBooksDTO[i].ISBN);
                Assert.AreEqual(allBooks[i].Pages, allBooksDTO[i].Pages);
                Assert.AreEqual(allBooks[i].Description, allBooksDTO[i].Description);
            }
        }

        [TestMethod]
        public void LoadBooks_ShouldReturnEmptyList_WhenNoBooksExist()
        {
            //Arrange
            _bookRepo.Setup(r => r.LoadBooks()).Returns((List<Book>)null);

            //Act
            List<BookDTO> allBooksDTO = _bookService.LoadBooks();

            //Assert
            Assert.AreEqual(0, allBooksDTO.Count);
        }

        [TestMethod]
        public void GetBookById_ShouldReturnBookDTO_WhenBookExists()
        {
            //Arrange
            Book book = new Book(
                                    id: 1,
                                    picture: "/images/book.jpg",
                                    title: "The Mockingbird Code",
                                    author: "Jane Devlin",
                                    description: "A thrilling journey into the world of AI and espionage.",
                                    isbn: "978-1-23456-789-7",
                                    genre: Genre.Thriller,
                                    pages: 384
                                 );
            _bookRepo.Setup(r => r.GetBookById(1)).Returns(book);

            //Act
            BookDTO? bookDTO = _bookService.GetBookById(1);

            //Assert
            Assert.IsNotNull(bookDTO);
            Assert.AreEqual(book.Id, bookDTO.Id);
            Assert.AreEqual(book.Picture, bookDTO.Picture);
            Assert.AreEqual(book.Title, bookDTO.Title);
            Assert.AreEqual(book.Author, bookDTO.Author);
            Assert.AreEqual(book.Description, bookDTO.Description);
            Assert.AreEqual(book.ISBN, bookDTO.ISBN);
            Assert.AreEqual(book.Genre, bookDTO.Genre);
            Assert.AreEqual(book.Pages, bookDTO.Pages);
        }

        [TestMethod]
        public void GetBookById_ShouldReturnNull_WhenBookDoesNotExist()
        {
            //Arrange
            _bookRepo.Setup(r => r.GetBookById(It.IsAny<int>())).Returns((Book?)null);
            //Act
            BookDTO bookDTO = _bookService.GetBookById(999999999);

            //Assert
            Assert.IsNull(bookDTO);
        }

        [TestMethod]
        public void UpdateBook_ShouldReturnTrue_WhenValidUpdate()
        {
            //Arrange
            Book oldBookVersion = new Book(3, "/images/book3.jpg", "Just Words", "Ghost Writer", "Another mystery book", "111-1-11111-111-1", Genre.Fantasy, 150);
            BookDTO newBookVersion = new BookDTO()
            {
                Id = 3,
                Title = "New Title",
                Author = "Author",
                Description = "Updated",
                ISBN = "123",
                Genre = Genre.Mystery,
                Pages = 250,
                Picture = null
            };
            _bookRepo.Setup(r => r.GetBookById(3)).Returns(oldBookVersion);
            _bookRepo.Setup(r => r.UpdateBook(It.IsAny<Book>())).Returns(true);

            //Act
            bool isUpdated = _bookService.UpdateBook(newBookVersion);

            //Assert
            Assert.IsTrue(isUpdated);   
        }

        [TestMethod]
        public void UpdateBook_ShouldReturnFalse_WhenBookWithTheSameISBNExists()
        {
            //Arrange
            Book oldBookVersion = new Book(3, "/images/book3.jpg", "Just Words", "Ghost Writer", "Another mystery book", "111-1-11111-111-1", Genre.Fantasy, 150);
            List<Book> allBooks = new List<Book>()
            {
                 new Book(1, "/images/book.jpg", "The Mockingbird Code", "Jane Devlin", "A thrilling journey into the world of AI and espionage.", "978-1-23456-789-7", Genre.Thriller, 384),
                 new Book(2, "/images/book2.jpg", "Empty Shell", "No One", "Empty book", "000-0-00000-000-0", Genre.Mystery, 1),
                oldBookVersion
            };
            BookDTO newBookVersion = new BookDTO()
            {
                Id = 3,
                Title = "New Title",
                Author = "Author",
                Description = "Updated",
                ISBN = "123",
                Genre = Genre.Mystery,
                Pages = 250,
                Picture = null
            };
            _bookRepo.Setup(r => r.GetBookById(3)).Returns(oldBookVersion);
            _bookRepo.Setup(r => r.LoadBooks()).Returns(allBooks.Where(b => b.Id != 3).ToList());

            //Act
            bool isUpdated = _bookService.UpdateBook(newBookVersion);

            //Assert
            Assert.IsFalse(isUpdated);
        }

        [TestMethod]
        public void RemoveBook_ShouldReturnTrue_WhenBookExists()
        {
            //Arrange
            Book book = new Book(3, "/images/book3.jpg", "Just Words", "Ghost Writer", "Another mystery book", "111-1-11111-111-1", Genre.Fantasy, 150);
            _bookRepo.Setup(r => r.GetBookById(book.Id)).Returns(book);
            _bookRepo.Setup(r => r.RemoveBook(It.IsAny<int>())).Returns(true);

            //Act
            bool isRemoved = _bookService.RemoveBook(book.Id);

            //Assert
            Assert.IsTrue(isRemoved);   
        }

        [TestMethod]
        public void RemoveBook_ShouldReturnFalse_WhenBookDoesNotExist()
        {
            //Arrange 
            int bookId = 99999999;
            _bookRepo.Setup(r => r.GetBookById(bookId)).Returns((Book)null);

            //Act
            bool isRemoved =_bookService.RemoveBook(bookId);

            Assert.IsFalse(isRemoved);
        }
    }
}