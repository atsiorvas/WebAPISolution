﻿using System;

namespace Common {
    public abstract class Entity {

        int _Id;
        private int? _requestedHashCode;

        public virtual int UserId {
            get {
                return _Id;
            }
            protected set {
                _Id = value;
            }
        }

        public bool IsTransient() {
            return this._Id == default(Int32);
        }

        public override bool Equals(object obj) {
            if (obj == null || !(obj is Entity))
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.UserId == this.UserId;
        }
        public override int GetHashCode() {
            if (!IsTransient()) {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.UserId.GetHashCode() ^ 31; // XOR for random distribution (http://blogs.msdn.com/b/ericlippert/archive/2011/02/28/guidelines-and-rules-for-gethashcode.aspx)

                return _requestedHashCode.Value;
            } else
                return base.GetHashCode();

        }
        public static bool operator ==(Entity left, Entity right) {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right) {
            return !(left == right);
        }
    }
}