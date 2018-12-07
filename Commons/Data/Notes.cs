using System.ComponentModel.DataAnnotations.Schema;

namespace Common {
    public class Notes : Entity {
        public long NoteId { get; set; }

        public string Text { get; set; }

        public int Lang { get; set; }

        [ForeignKey("UserForeignKey")]
        public int UserId { get; set; }
        public virtual User user { get; set; }

    }
}
