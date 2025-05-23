namespace EFDataLayer.DBContext;

public partial class Shelf
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? UserId { get; set; }

    public bool IsArchived { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
