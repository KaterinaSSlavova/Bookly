using AutoMapper;
using Bookly.Business_logic.InterfacesServices;
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
        private readonly IBookServices _bookService;

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
        public void LoadBooks_ShouldReturnAllBooksFromTheDatabase_WhenNoBooksExist()
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
            BookDTO bookDTO = _bookService.GetBookById(1);

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
    }
}