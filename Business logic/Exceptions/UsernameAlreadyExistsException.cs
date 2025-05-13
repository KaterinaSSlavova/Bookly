namespace Business_logic.Exceptions
{
    public class UsernameAlreadyExistsException: Exception
    {
        public UsernameAlreadyExistsException(string username): base($"User with the username: {username} already exists.") { }
    }

    public class EmailAlreadyExistsException: Exception
    {
        public EmailAlreadyExistsException(string email): base($"User with the email: {email} already exists.") { }
    }

    public class InvalidBirthdayException: Exception
    {
        public InvalidBirthdayException(): base($"Invalid birthday. Please enter your real birth date!") { }
    }

}
