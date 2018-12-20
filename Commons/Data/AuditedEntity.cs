using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common {

    [Owned]
    public class AuditedEntity : ITrack {

        internal static readonly AuditedEntity Empty = new AuditedEntity();

        [Required]
        [Column("create_on")]
        private DateTime _CreatedOn;

        [Column("updated_on")]
        private DateTime? _UpdatedOn;

        [Required]
        [Column("created_by")]
        private string _CreatedBy;

        public virtual DateTime CreatedOn {

            get {
                return _CreatedOn;
            }
            set {
                _CreatedOn = value;
            }
        }
        public virtual DateTime? UpdatedOn {
            get {
                return _UpdatedOn;
            }
            set {
                if (value != null) {
                    _UpdatedOn = value;
                }
            }
        }
        public virtual string CreatedBy {
            get {
                return _CreatedBy;
            }
            set {
                _CreatedBy = value;
            }
        }
    }

    public interface ITrack {

        DateTime CreatedOn { get; set; }

        DateTime? UpdatedOn { get; set; }

        string CreatedBy { get; set; }

    }
}