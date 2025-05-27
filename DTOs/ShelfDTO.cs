namespace Business_logic.DTOs
{
    public class ShelfDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserDTO User { get; set; } = null;

        public ShelfDTO(string name, UserDTO user)
        {
            this.Name = name;
            this.User = user;
        }

        public ShelfDTO(string name)
        {
            this.Name = name; 
        }

        public ShelfDTO(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public ShelfDTO()
        {
            
        }
    }
}
