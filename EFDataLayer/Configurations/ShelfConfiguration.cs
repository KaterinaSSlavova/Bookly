using EFDataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.Configurations
{
    public class ShelfConfiguration: IEntityTypeConfiguration<Shelf>
    {
        public void Configure(EntityTypeBuilder<Shelf> entity)
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
        }
    }
}
