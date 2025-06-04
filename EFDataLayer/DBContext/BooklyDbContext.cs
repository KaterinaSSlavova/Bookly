using EFDataLayer.Configurations;
using EFDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace EFDataLayer.DBContext;

public partial class BooklyDbContext : DbContext
{
    public BooklyDbContext()
    {
    }

    public BooklyDbContext(DbContextOptions<BooklyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Goal> Goals { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Shelf> Shelves { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<CurrentBook> CurrentBooks { get; set; }

    public virtual DbSet<UserRating> UserRatings { get; set; }

    public virtual DbSet<BookRating> BookRatings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new GoalConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new CurrentBookConfiguration());
        modelBuilder.ApplyConfiguration(new UserRatingConfiguration());
        modelBuilder.ApplyConfiguration(new BookRatingConfiguration());
        modelBuilder.ApplyConfiguration(new ShelfConfiguration());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
