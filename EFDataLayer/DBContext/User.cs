namespace EFDataLayer.DBContext;

public partial class User
{
    public int Id { get; set; }

    public byte[]? Picture { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime? BirthDate { get; set; }

    public Role Role { get; set; }

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Shelf> Shelves { get; set; } = new List<Shelf>();

    public virtual ICollection<CurrentBook> UserBookProgresses { get; set; } = new List<CurrentBook>();

    public virtual ICollection<UserRating> UserRatings { get; set; } = new List<UserRating>();
}
