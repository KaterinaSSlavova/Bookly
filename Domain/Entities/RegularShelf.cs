namespace Models.Entities
{
    public class RegularShelf
    { 
        public Shelf Shelf { get; private set; }    
        public List<Book> BooksOnShelf { get;  private set; }
        public RegularShelf(Shelf shelf, List<Book> books)
        {
            this.Shelf = shelf;
            this.BooksOnShelf = books;
        }

        public RegularShelf(Shelf shelf)
        {
            this.Shelf = shelf;
            this.BooksOnShelf = new List<Book>();
        }
    }
}
