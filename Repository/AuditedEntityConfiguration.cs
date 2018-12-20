using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Common {
    public class AuditedEntityConfiguration
            : IEntityTypeConfiguration<AuditedEntity> {

        public void Configure(EntityTypeBuilder<AuditedEntity> builder) {
        }
    }

}
