using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository {
    public class UserConfiguration : IEntityTypeConfiguration<User> {

        public void Configure(EntityTypeBuilder<User> builder) {

            builder
                .HasKey(x => x.Id);
            builder
               .Property(x => x.Id)
               .HasColumnType("BIGINT")
               .ValueGeneratedOnAdd();

            builder
                .HasIndex(u => u.Email)
                .IsUnique();

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

            //builder
            //    .HasMany(e => e.Alert)
            //    .WithOne(u => u.User)
            //    .HasForeignKey(x => x.Id)
            //    .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(n => n.Note)
                .WithOne(n => n.User)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("user", "dbo");
        }
    }
}
