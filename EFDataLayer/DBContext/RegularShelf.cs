namespace EFDataLayer.DBContext
{
    public class RegularShelf
    {
        public Shelf Shelf { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
