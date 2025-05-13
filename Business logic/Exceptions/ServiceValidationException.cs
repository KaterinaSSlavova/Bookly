namespace Business_logic.Exceptions
{
    public class ServiceValidationException: Exception
    {
        public ServiceValidationException(string message): base(message) { }

    }
}
