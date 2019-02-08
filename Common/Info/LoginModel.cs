using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Info {
    public class LoginModel {

        public string Password { get; set; }

        public string Email { get; set; }

        public bool RememberMe { get; set; }
        public string Token { get; set; }
    }
}
