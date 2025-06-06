using Microsoft.EntityFrameworkCore;
using EFDataLayer.Entities;

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

    public void DetachIfTracked<TEntity, TKey>(TEntity entity, Func<TEntity, TKey> keySelector) where TEntity : class
    {
        var entityKey = keySelector(entity);

        var trackedEntity = ChangeTracker.Entries<TEntity>()
            .FirstOrDefault(e => keySelector(e.Entity).Equals(entityKey));

        if (trackedEntity != null)
        {
            trackedEntity.State = EntityState.Detached;
        }
    }
}
