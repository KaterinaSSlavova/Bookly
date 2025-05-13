namespace Business_logic.Exceptions
{
    public class ShelfAlreadyExistsException: Exception
    {
        public ShelfAlreadyExistsException(string name) : base($"A shelf named {name} already exists.") { }
    }

    public class BookIsAlreadyOnShelfException: Exception
    {
        public BookIsAlreadyOnShelfException(string shelf, string book): base($"{book} is already on {shelf} shelf.") { }
    }

    public class InvalidProgressException: Exception
    {
        public InvalidProgressException(int progress, int maxPages): base($"Progress value {progress} is not valid. Progress must be between 0 and {maxPages}.") { }
    }
}
