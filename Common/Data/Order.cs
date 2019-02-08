using Common.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common {
    public class Order : Entity {

        [Key]
        [Required]
        [Column("order_id")]
        public override long Id { get; set; }

        [Column("date_sent")]
        [DataType(DataType.DateTime)]
        [Required]
        public DateTime DateSent { get; set; }

        [Column("text")]
        [Required]
        public string Description { get; set; }



        [Column("user_id")]
        [ForeignKey("User")]
        public long UserId { get; set; }

        public virtual User User { get; set; }

        public virtual HashSet<OrderAlert> OrderAlert { get; set; } = new HashSet<OrderAlert>();

        public AuditedEntity AuditedEntity { get; set; } = new AuditedEntity();

    }
}
