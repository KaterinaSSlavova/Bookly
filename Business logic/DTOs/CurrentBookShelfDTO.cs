namespace Business_logic.DTOs
{
    public class CurrentBookShelfDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CurrentBookDTO> BooksOnShelf { get; set; }

        public CurrentBookShelfDTO(int id, string name, List<CurrentBookDTO> Books)
        {
            this.Id = id;
            this.Name = name;
            this.BooksOnShelf = Books;
        }
    }
}
