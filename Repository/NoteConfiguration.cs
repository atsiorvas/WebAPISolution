using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository {
    public class NoteConfiguration : IEntityTypeConfiguration<Notes> {

        public void Configure(EntityTypeBuilder<Notes> builder) {
            //NoteId
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
               .Property(n => n.Lang)
               .HasColumnType("TINYINT")
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

            builder.OwnsOne(n => n.AuditedEntity)
                .Property(a => a.UpdatedOn)
                .HasColumnName("updated_on")
                 .HasColumnType("datetime2");

            //RelationShip
            builder
                .HasOne(n => n.User)
                .WithMany(x => x.Note)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                 .ToTable("note", "note");
        }
    }
}
