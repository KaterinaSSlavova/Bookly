namespace Business_logic.Exceptions
{
    public class ServiceValidationException: Exception
    {
        public ServiceValidationException(string message): base(message) { }

    }

    public class ServiceNullException : Exception
    {
        public ServiceNullException(string message) : base(message) { }

    }
}
