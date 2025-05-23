namespace EFDataLayer.DBContext;

public partial class CurrentBook
{
    public int UserId { get; set; }

    public int BookId { get; set; }

    public int Progress { get; set; }

    public Status Status { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
