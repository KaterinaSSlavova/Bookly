//using AutoMapper;
//using EFDataLayer.DBContext;
//using Business_logic.DTOs;
//using Business_logic.Mappers;
//using Bookly.ViewModels;
//using Bookly.Mappers;

//namespace Tests
//{
//    [TestClass]
//    public class AutoMapperTests
//    {
//        private readonly IMapper _mapper;
//        public AutoMapperTests()
//        {
//            var config = new MapperConfiguration(cfg =>
//            {
//                cfg.AddProfile(new ShelfMapper());
//                cfg.AddProfile(new BookMapper());
//                cfg.AddProfile(new BookModelMapper());
//            });
//            _mapper = config.CreateMapper();
//        }


//        [TestMethod]
//        public void Map_FromBookDTO_ToBookViewModel()
//        {
//            //Arrange
//            BookDTO bookDTO = new BookDTO(1, "/images/book1.png", "Book1", "Author1", "Description1", "999-9-99999-999-9", Genre.Thriller, 100);

//            //Act
//            BookViewModel book = _mapper.Map<BookViewModel>(bookDTO);

//            //Assert
//            Assert.AreEqual(book.Id, bookDTO.Id);
//            Assert.AreEqual(book.Picture, bookDTO.Picture);
//            Assert.AreEqual(book.Title, bookDTO.Title);
//            Assert.AreEqual(book.Author, bookDTO.Author);
//            Assert.AreEqual(book.Description, bookDTO.Description);
//            Assert.AreEqual(book.ISBN, bookDTO.ISBN);
//            Assert.AreEqual(book.Genre, bookDTO.Genre.ToString());
//            Assert.AreEqual(book.Pages, bookDTO.Pages);
//        }

//        [TestMethod]
//        public void Map_FromBookViewModel_ToBookDTO()
//        {
//            //Arrange
//            BookViewModel book = new BookViewModel
//            {
//                Id = 1,
//                Picture = "/images/book1.png",
//                Title = "Book1",
//                Author = "Author1",
//                Description = "Description1",
//                ISBN = "999-9-99999-999-9",
//                Genre = "Thriller",
//                Pages = 100
//            };

//            //Act
//            BookDTO bookDTO = _mapper.Map<BookDTO>(book);

//            //Assert
//            Assert.AreEqual(book.Id, bookDTO.Id);
//            Assert.AreEqual(book.Picture, bookDTO.Picture);
//            Assert.AreEqual(book.Title, bookDTO.Title);
//            Assert.AreEqual(book.Author, bookDTO.Author);
//            Assert.AreEqual(book.Description, bookDTO.Description);
//            Assert.AreEqual(book.ISBN, bookDTO.ISBN);
//            Assert.AreEqual(book.Genre, bookDTO.Genre.ToString());
//            Assert.AreEqual(book.Pages, bookDTO.Pages);
//        }

//        [TestMethod]
//        public void Map_FromBook_ToBookDTO()
//        {
//            //Arrange
//            Book book = new Book(1, "/images/book1.png", "Book1", "Author1", "Description1", "999-9-99999-999-9", Genre.Thriller, 100);

//            //Act
//            BookDTO bookDTO = _mapper.Map<BookDTO>(book);

//            //Assert
//            Assert.AreEqual(book.Id, bookDTO.Id);
//            Assert.AreEqual(book.Picture, bookDTO.Picture);
//            Assert.AreEqual(book.Title, bookDTO.Title);
//            Assert.AreEqual(book.Author, bookDTO.Author);
//            Assert.AreEqual(book.Description, bookDTO.Description);
//            Assert.AreEqual(book.ISBN, bookDTO.ISBN);
//            Assert.AreEqual(book.Genre, bookDTO.Genre);
//            Assert.AreEqual(book.Pages, bookDTO.Pages);
//        }

//        [TestMethod]
//        public void Map_FromBookDTO_ToBook()
//        {
//            //Arrange
//            BookDTO bookDTO = new BookDTO(1, "/images/book1.png", "Book1", "Author1", "Description1", "999-9-99999-999-9", Genre.Thriller, 100);

//            //Act
//            Book book = _mapper.Map<Book>(bookDTO);

//            //Assert
//            Assert.AreEqual(book.Id, bookDTO.Id);
//            Assert.AreEqual(book.Picture, bookDTO.Picture);
//            Assert.AreEqual(book.Title, bookDTO.Title);
//            Assert.AreEqual(book.Author, bookDTO.Author);
//            Assert.AreEqual(book.Description, bookDTO.Description);
//            Assert.AreEqual(book.ISBN, bookDTO.ISBN);
//            Assert.AreEqual(book.Genre, bookDTO.Genre);
//            Assert.AreEqual(book.Pages, bookDTO.Pages);
//        }
//    }
//}
