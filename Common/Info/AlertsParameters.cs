using System;
using System.Collections.Generic;

namespace Common.Info {
    public sealed class AlertsParameters {

        public sealed class Builder {
            private Guid AlertMessageId { get; set; }

            private List<string> Arguments { get; set; }
            private DateTime FromDate { get; set; }
            private DateTime ToDate { get; set; }
            private DateTime DateSent { get; set; }
            public DateTime DateCreated { get; set; }
            public string Text { get; set; }
            public User User { get; set; }

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
            public Builder WithToDate(DateTime toDate) {
                this.ToDate = toDate;
                return this;
            }
            public Builder WithDateSent(DateTime dateSent) {
                this.DateSent = dateSent;
                return this;
            }
            public Builder WithDateCreated(DateTime dateCreated) {
                this.DateCreated = dateCreated;
                return this;
            }
            public Builder WithText(string text) {
                this.Text = text;
                return this;
            }

            public Builder WithUser(User user) {
                this.User = user;
                return this;
            }

            public AlertsParameters build() {
                AlertsParameters amp = new AlertsParameters();
                amp.AlertMessageId = this.AlertMessageId;
                amp.Arguments = this.Arguments ?? new List<string>();
                amp.FromDate = this.FromDate;
                amp.ToDate = this.ToDate;
                amp.DateCreated = this.DateCreated;
                amp.DateSent = this.DateSent;
                amp.Text = this.Text;
                amp.User = this.User;
                return amp;
            }
        }

        public Guid AlertMessageId { get; set; }

        public List<string> Arguments { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        private DateTime DateSent { get; set; }
        public DateTime DateCreated { get; set; }
        public string Text { get; set; }
        public User User { get; set; }
    }
}