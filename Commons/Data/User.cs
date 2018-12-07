using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common {

    [Table("User")]
    public class User : Entity {
        [Key]
        [Required]
        [Column("Id")]
        public override int UserId { get; protected set; }

        [Column("FirstName")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Column("LastName")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column("Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Column("RememberMe")]
        public bool RememberMe { get; set; } = false;

        public bool IsAdmin { get; set; }

        [Column("ResetAnswer")]
        [Required]
        [DataType(DataType.Text)]
        public string ResetAnswer { get; set; }

        public virtual List<Notes> Note { get; set; }

    }
}
