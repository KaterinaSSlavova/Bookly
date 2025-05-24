namespace EFDataLayer.DBContext
{
    public class RegularShelf
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual User? User { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
