using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Data {
    [Table("alert")]
    public class Alert : Entity {

        [Key]
        [Column("alert_id")]
        public override long Id { get; set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("arguments")]
        public string Arguments { get; set; }

        [Required]
        [Column("date_created")]
        public DateTime DateCreated { get; set; }

        [Column("date_sent")]
        public DateTime DateSent { get; set; }

        [Column("user_id")]
        [ForeignKey("user")]
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public AuditedEntity AuditedEntity { get; set; } = AuditedEntity.Empty;
    }
}
