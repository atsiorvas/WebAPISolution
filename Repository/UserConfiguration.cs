using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository {
    public class UserConfiguration : IEntityTypeConfiguration<User> {

        public void Configure(EntityTypeBuilder<User> builder) {

            builder
                .HasKey(x => x.UserId);

            builder
                .Property(x => x.UserId)
                .ValueGeneratedOnAdd();
        }
    }

}
