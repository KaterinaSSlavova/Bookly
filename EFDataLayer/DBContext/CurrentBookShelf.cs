namespace EFDataLayer.DBContext
{
    public class CurrentBookShelf
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public virtual User? User { get; set; }

        public virtual ICollection<CurrentBook> Books { get; set; } = new List<CurrentBook>();
    }
}
