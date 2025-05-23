namespace EFDataLayer.DBContext;

public partial class UserRating
{
    public int UserId { get; set; }

    public int RatingId { get; set; }

    public int Id { get; set; }

    public virtual Ratings Rating { get; set; } 

    public virtual User User { get; set; } = null!;
}
