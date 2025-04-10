using Microsoft.AspNetCore.Http;

namespace Business_logic.DTOs
{
    public class UserDTO
    {
        public byte[] Picture { get; set; }
        public string Username { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }

        public UserDTO(IFormFile picture, string username, int age, string email)
        {
            this.Picture = ConvertImageToBinary(picture);
            this.Username = username;
            this.Age = age;
            this.Email = email;
        }

        private byte[] ConvertImageToBinary(IFormFile picture)
        {
            if (picture != null)
            {
                using MemoryStream mStream = new MemoryStream();
                picture.CopyTo(mStream);
                return mStream.ToArray();
            }
            return null;
        }
    }
}
