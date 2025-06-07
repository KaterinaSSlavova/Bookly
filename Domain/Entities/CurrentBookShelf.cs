namespace Models.Entities
{
    public class CurrentBookShelf
    {
        public Shelf Shelf { get; private set; }
        public List<CurrentBook> CurrentBooks { get; private set; }

        public CurrentBookShelf(Shelf shelf, List<CurrentBook> books)
        {
            this.Shelf = shelf;
            this.CurrentBooks = books;
        }

        public CurrentBookShelf(Shelf shelf)
        {
            this.Shelf = shelf;
            this.CurrentBooks = new List<CurrentBook>();
        }
    }
}
