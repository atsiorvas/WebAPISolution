using System;

namespace Common.Interface {
    public interface ITrack {

        DateTime CreatedOn { get; set; }

        DateTime? UpdatedOn { get; set; }

        string CreatedBy { get; set; }

    }
}