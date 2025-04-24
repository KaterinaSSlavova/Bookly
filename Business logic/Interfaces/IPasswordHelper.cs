namespace Business_logic.InterfacesHelpers
{
    public interface IPasswordHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);

    }
}
