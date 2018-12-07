using Common;
using Common.Interface;

namespace Model {
    public class ErrorMessageViewModel : IBaseViewModel {

        public string Message { get; set; }

        public string Property { get; set; }

        public ErrorMessageViewModel(string message, string property) {
            Message = message;
            Property = !string.IsNullOrEmpty(property) ? property : "";
        }

        public ErrorMessageViewModel() { }

    }
}
