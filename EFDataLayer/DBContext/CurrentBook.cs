using System.ComponentModel.DataAnnotations.Schema;

namespace EFDataLayer.DBContext;

public partial class CurrentBook
{
    public int UserId { get; set; }

    public int BookId { get; set; }

    public int Progress { get; set; }
    public int StatusId { get; set; }

    [NotMapped]
    public Status Status
    {
        get => (Status)StatusId;
        set => StatusId = (int)value;
    }

    public virtual Book Book { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
