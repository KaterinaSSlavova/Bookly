namespace Exceptions
{
    public class InvalidBookPagesException: Exception
    {
        public InvalidBookPagesException(int pages): base($"Book pages cannot be {pages}! Please enter a number bigger than 0!") { }
    }

    public class DuplicateISBNException: Exception
    {
        public DuplicateISBNException(string isbn): base($"This ISBN '{isbn}' already exists!") { }
    }
}
