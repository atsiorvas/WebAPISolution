using Common.Interface;

namespace ViewModel {
    public class LoginViewModel : IBaseViewModel {
        public string Password { get; set; }

        public string Email { get; set; }

        public bool RememberMe { get; set; }
        public string Token { get; set; }

    }
}
