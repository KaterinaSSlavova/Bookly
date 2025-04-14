namespace Business_logic.DTOs
{
    public class ShelfDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookDTO> BooksOnShelf { get; set; }

        public ShelfDTO(string name)
        {
            this.Name = name;
        }
        public ShelfDTO(string name, List<BookDTO> books)
        {
            this.Name = name;
            this.BooksOnShelf = books;
        }

        public ShelfDTO(int id, string name, List<BookDTO> books)
        {
            this.Id = id;
            this.Name = name;
            this.BooksOnShelf = books;
        }
    }
}
