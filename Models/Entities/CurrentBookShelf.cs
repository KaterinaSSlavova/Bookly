namespace Models.Entities
{
    public class CurrentBookShelf: Shelf
    {
        public List<CurrentBook> CurrentBooks { get;  private set; }
        public CurrentBookShelf(int id, string name,User user, List<CurrentBook> currentBooks): base(id, name, user)
        {
            this.CurrentBooks = currentBooks;
        }

        public CurrentBookShelf(string name,User user, List<CurrentBook> currentBooks) : base(name, user)
        {
            this.CurrentBooks = currentBooks;
        }
    }
}
