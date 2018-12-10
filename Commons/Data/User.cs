﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common {

    [Table("user")]
    public class User : Entity {
        [Key]
        [Required]
        [Column("user_id")]
        public override int Id { get; protected set; }

        [Column("first_name")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Column("last_name")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        [Column("password")]
        public string Password { get; set; }

        [Column("email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Column("remember_me")]
        public bool RememberMe { get; set; } = false;

        [Column("is_admin")]
        public bool IsAdmin { get; set; }

        [Column("reset_answer")]
        [Required]
        [DataType(DataType.Text)]
        public string ResetAnswer { get; set; }

        [Column("notes")]
        public virtual HashSet<Notes> Note { get; set; }

    }
}
