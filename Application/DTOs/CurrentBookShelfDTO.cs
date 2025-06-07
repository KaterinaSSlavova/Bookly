namespace Business_logic.DTOs
{
    public class CurrentBookShelfDTO
    {
        public ShelfDTO Shelf { get; set; }
        public List<CurrentBookDTO> CurrentBooks { get; set; }

        public CurrentBookShelfDTO(ShelfDTO shelf, List<CurrentBookDTO> Books)
        {
            this.CurrentBooks = Books;
        }

        public CurrentBookShelfDTO()
        { 
            
        }
    }
}
