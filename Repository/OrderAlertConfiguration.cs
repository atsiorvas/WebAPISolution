using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Common.Data;

namespace Common {
    public class OrderAlertConfiguration
            : IEntityTypeConfiguration<OrderAlert> {

        public void Configure(EntityTypeBuilder<OrderAlert> builder) {

            //OrderAlertId
            //combine ids
            builder
                .HasKey(x => x.Id);

            //Many-to-Many relationship
            builder
               .HasOne(x => x.Alert)
               .WithMany(p => p.OrderAlert)
               .HasForeignKey(pc => pc.AlertId)
               .OnDelete(DeleteBehavior.Cascade);

            //builder
            //  .HasOne(x => x.Order)
            //  .WithMany(p => p.OrderAlert)
            //  .HasForeignKey(pc => pc.OrderId)
            //  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
