using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository {
    public class NoteConfiguration : IEntityTypeConfiguration<Notes> {

        public void Configure(EntityTypeBuilder<Notes> builder) {
            //NoteId
            builder
                .HasKey(x => x.NoteId);

            builder
                .Property(x => x.NoteId)
                .ValueGeneratedOnAdd();

            //Text
            builder
                .Property(n => n.Text)
                .HasColumnType("TEXT")
                .IsRequired();

            //Lang
            builder
               .Property(n => n.Lang)
               .HasColumnType("INT")
               .IsRequired();

            //RelationShip
            builder
                .HasOne(n => n.user)
                .WithMany(x => x.Note)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }


}
