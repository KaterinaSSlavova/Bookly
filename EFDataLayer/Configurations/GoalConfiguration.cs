using EFDataLayer.DBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFDataLayer.Configurations
{
    public class GoalConfiguration: IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> entity)
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
        }
    }
}
