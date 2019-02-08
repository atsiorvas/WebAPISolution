using System;
using System.Collections.Generic;
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

        [Required]
        [Column("from_date")]
        public DateTime FromDate { get; set; }

        [Required]
        [Column("to_date")]
        public DateTime ToDate { get; set; }

        [Required]
        [Column("date_sent")]
        public DateTime DateSent { get; set; }

        [Column("user_id")]
        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public virtual HashSet<OrderAlert> OrderAlert { get; set; } = new HashSet<OrderAlert>();

        public AuditedEntity AuditedEntity { get; set; } = AuditedEntity.Empty;
    }

    [Table("order_alert")]
    public class OrderAlert : Entity {

        [Key]
        [Column("order_alert_id")]
        public override long Id { get; set; }

        [Column("order_id")]
        public long OrderId { get; set; }

        public virtual Order Order { get; set; }

        [Column("alert_id")]
        public long AlertId { get; set; }

        public virtual Alert Alert { get; set; }
    }
}