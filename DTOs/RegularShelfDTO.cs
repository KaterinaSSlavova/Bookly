namespace Business_logic.DTOs
{
	public class RegularShelfDTO
	{
        public ShelfDTO Shelf { get; set; }
        public List<BookDTO> BooksOnShelf { get; set; }

		public RegularShelfDTO(ShelfDTO shelf, UserDTO user)
		{
			this.Shelf = shelf;
			this.BooksOnShelf = new List<BookDTO>();
		}

		public RegularShelfDTO(ShelfDTO shelf, List<BookDTO> books)
		{
			this.Shelf = shelf;
			this.BooksOnShelf = books;
		}

		public RegularShelfDTO()
		{

		}
	}
}
