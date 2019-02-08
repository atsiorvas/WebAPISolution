using Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository {
    public class OrderConfiguration : IEntityTypeConfiguration<Order> {

        public void Configure(EntityTypeBuilder<Order> builder) {

            //OrderId
            builder
                .Property(x => x.Id)
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd();

            builder
                .Property(x => x.Description)
                .HasColumnType("nvarchar(200)");

            //Embedded
            builder
                .OwnsOne(n => n.AuditedEntity)
                .Property(a => a.CreatedBy)
                .HasColumnName("created_by")
                .HasColumnType("nvarchar(100)")
                .IsRequired();

            builder
                .OwnsOne(n => n.AuditedEntity)
                .Property(a => a.CreatedOn)
                .HasColumnName("created_on")
                .HasColumnType("datetime2");

            builder
                .OwnsOne(n => n.AuditedEntity)
                .Property(a => a.UpdatedOn)
                .HasColumnName("updated_on")
                .HasColumnType("datetime2");

            //RelationShip
            builder
                .HasOne(n => n.User)
                .WithMany(x => x.Order)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                 .ToTable("order", "dbo");

        }
    }
}