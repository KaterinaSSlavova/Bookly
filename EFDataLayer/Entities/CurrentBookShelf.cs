namespace EFDataLayer.DBContext
{
    public class CurrentBookShelf
    {
        public Shelf Shelf { get; set; }

        public virtual ICollection<CurrentBook> Books { get; set; } = new List<CurrentBook>();
    }
}
