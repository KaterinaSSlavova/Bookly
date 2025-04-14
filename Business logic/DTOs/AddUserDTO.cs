namespace Business_logic.DTOs
{
    public class AddUserDTO
    {
        public string Picture { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public AddUserDTO(string username, int age, string email)
        {
            this.Username = username;
            this.Age = age;
            this.Email = email;
        }
    }
}
