using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common {
    [Table("note")]
    public class Notes : Entity {

        [Key]
        [Column("note_id")]
        public override int Id { get; protected set; }

        [Column("text")]
        public string Text { get; set; }

        [Column("lang")]
        public int Lang { get; set; }

        [ForeignKey("UserForeignKey")]
        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user")]
        public virtual User User { get; set; }

    }
}
