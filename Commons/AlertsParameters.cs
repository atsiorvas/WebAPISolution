using System;
using System.Collections.Generic;

namespace Common {
    public class AlertsParameters {

        public sealed class Builder {
            private Guid AlertMessageId { get; set; }

            private List<string> Arguments { get; set; }
            private DateTime FromDate { get; set; }
            private DateTime ToDate { get; set; }

            public Builder() {
                AlertMessageId = Guid.NewGuid();
                Arguments = new List<string>();
            }
            public Builder WithArguments(List<string> arguments) {

                if (arguments == null) {
                    this.Arguments = new List<string>();
                } else {
                    this.Arguments = arguments;
                }
                return this;
            }
            public Builder WithFromDate(DateTime fromDate) {
                this.FromDate = fromDate;
                return this;
            }
            public Builder WithToDate(DateTime fromDate) {
                this.ToDate = ToDate;
                return this;
            }
            public AlertsParameters build() {
                AlertsParameters amp = new AlertsParameters();
                amp.AlertMessageId = this.AlertMessageId;
                amp.Arguments = this.Arguments ?? new List<string>();
                amp.FromDate = this.FromDate;
                amp.ToDate = this.ToDate;

                return amp;
            }
        }

        public Guid AlertMessageId { get; set; }

        public List<string> Arguments { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}