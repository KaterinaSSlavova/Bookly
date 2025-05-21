namespace Business_logic.DTOs
{
    public class CurrentBookShelfDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDTO User { get; set; }
        public List<CurrentBookDTO> CurrentBooks { get; set; }

        public CurrentBookShelfDTO(int id, string name, UserDTO user, List<CurrentBookDTO> Books)
        {
            this.Id = id;
            this.Name = name;
            this.User = user;
            this.CurrentBooks = Books;
        }

        public CurrentBookShelfDTO(int id, string name, List<CurrentBookDTO> Books)
        {
            this.Id = id;
            this.Name = name;
            this.CurrentBooks = Books;
        }

        public CurrentBookShelfDTO(int id)
        { 
            this .Id = id;  
        }
    }
}
