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
        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Books__3214EC07FEA26F79");

            entity.Property(e => e.Author).HasMaxLength(255);
            entity.Property(e => e.Genre)
        .HasConversion<string>() 
        .HasMaxLength(255);
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Picture).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Goal>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Goals__3214EC07D2C03D8F");

            entity.Property(e => e.End).HasColumnType("datetime");
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");
            entity.Property(e => e.Start).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasDefaultValue(1);

            entity.Property(e => e.StatusId)
        .HasDefaultValue((int)Status.Not_started);

            entity.HasOne(d => d.User).WithMany(p => p.Goals)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Goals__UserId__49C3F6B7");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Reviews__3214EC07C65679B4");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsArchived).HasColumnName("isArchived");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reviews__BookId__45F365D3");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Reviews__UserId__44FF419A");
        });

        modelBuilder.Entity<Shelf>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Shelves__3214EC0723F06371");

            entity.Property(e => e.IsArchived).HasColumnName("isArchived");
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.User).WithMany(p => p.Shelves)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Shelves__UserId__3E52440B");

            entity.HasMany(d => d.Books).WithMany(p => p.Shelves)
                .UsingEntity<Dictionary<string, object>>(
                    "ShelfBook",
                    r => r.HasOne<Book>().WithMany()
                        .HasForeignKey("BookId")
                        .HasConstraintName("FK__ShelfBook__BookI__4222D4EF"),
                    l => l.HasOne<Shelf>().WithMany()
                        .HasForeignKey("ShelfId")
                        .HasConstraintName("FK__ShelfBook__Shelf__412EB0B6"),
                    j =>
                    {
                        j.HasKey("ShelfId", "BookId").HasName("PK__ShelfBoo__B80E4327D48F6871");
                        j.ToTable("ShelfBook");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07E48C32A0");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4A6F8AB57").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534B1A54B8A").IsUnique();

            entity.Property(e => e.BirthDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.RoleId)
        .HasDefaultValue((int)Role.Reader);
            entity.Property(e => e.Username).HasMaxLength(255);

        });

        modelBuilder.Entity<CurrentBook>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.BookId }).HasName("PK_UserBook");

            entity.ToTable("UserBookProgress");

            entity.Property(e => e.StatusId)
        .HasDefaultValue((int)Status.Not_started);

            entity.HasOne(d => d.Book).WithMany(p => p.UserBookProgresses)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Book");

            entity.HasOne(d => d.User).WithMany(p => p.UserBookProgresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User");
        });

        modelBuilder.Entity<UserRating>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserRati__3214EC07DA49AE34");

            entity.ToTable("UserRating");

            entity.HasOne(d => d.User).WithMany(p => p.UserRatings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRatin__UserI__151B244E");
        });

        modelBuilder.Entity<BookRating>(entity =>
        {
            entity.HasKey(br => new { br.RatingId, br.BookId });

            entity.ToTable("BookRating");

            entity.Property(e => e.RatingId)
        .HasDefaultValue((int)Ratings.Neutral);

            entity.HasOne(br => br.Book)
                  .WithMany(b => b.BookRatings)
                  .HasForeignKey(br => br.BookId);

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
