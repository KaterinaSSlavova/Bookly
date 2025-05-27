namespace Business_logic.DTOs
{
    public class CurrentBookShelfDTO
    {
        public ShelfDTO Shelf { get; set; }
        public List<CurrentBookDTO> CurrentBooks { get; set; } = new List<CurrentBookDTO>();

        public CurrentBookShelfDTO(ShelfDTO shelf, List<CurrentBookDTO> Books)
        {
            this.Shelf = shelf;
            this.CurrentBooks = Books;
        }

        public CurrentBookShelfDTO(ShelfDTO shelf)
        {
            this.Shelf = shelf;
            CurrentBooks = new List<CurrentBookDTO>();
        }

        public CurrentBookShelfDTO()
        {
            
        }

    }
}
