using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Data;

namespace Common {
    public class AlertEntityConfiguration
            : IEntityTypeConfiguration<Alert> {

        public void Configure(EntityTypeBuilder<Alert> builder) {

            //AlertId
            builder
                .Property(x => x.Id)
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd();

            //Text
            builder
                .Property(n => n.Text)
                .HasColumnType("TEXT")
                .IsRequired();

            //Lang
            builder
               .Property(n => n.Arguments)
               .HasColumnType("nvarchar(100)")
               .IsRequired();

            //DateCreated
            builder
               .Property(n => n.DateCreated)
               .HasColumnType("datetime2")
               .IsRequired();

            builder
             .Property(n => n.DateSent)
             .HasColumnType("datetime2")
             .IsRequired();

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

            builder
                 .ToTable("alert", "dbo");
        }
    }
}
