using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common {
    [Table("note")]
    public class Notes : Entity {

        [Key]
        [Column("note_id")]
        public override long Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("lang")]
        public int Lang { get; set; }

        [Column("user_id")]
        [ForeignKey("user")]
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public AuditedEntity AuditedEntity { get; set; }
    }
}
