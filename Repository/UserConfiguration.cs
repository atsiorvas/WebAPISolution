using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository {
    public class UserConfiguration : IEntityTypeConfiguration<User> {

        public void Configure(EntityTypeBuilder<User> builder) {

            //builder
            //    .HasKey(x => x.Id);
            builder
               .Property(x => x.Id)
               .HasColumnType("BIGINT")
               .ValueGeneratedOnAdd();

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

            builder.ToTable("user", "dbo");
        }
    }

}
