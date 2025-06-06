using System.Reflection;
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
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BooklyDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
