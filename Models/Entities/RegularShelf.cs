namespace Models.Entities
{
    public class RegularShelf: Shelf
    {
        public List<Book> BooksOnShelf { get;  private set; }

        public RegularShelf(int id, string name, List<Book> books) : base(id, name)
        {
            this.BooksOnShelf = books;
        }
        public RegularShelf(int id, string name, User user, List<Book> books): base(id, name, user)
        {
            this.BooksOnShelf = books; 
        }

        public RegularShelf(string name, User user, List<Book> books) : base(name, user)
        {
            this.BooksOnShelf = books;
        }

        public RegularShelf(string name): base (name) 
        {
            BooksOnShelf = new List<Book>();
        }
    }
}
