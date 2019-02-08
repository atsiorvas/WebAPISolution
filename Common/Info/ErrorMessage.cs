using Common.Interface;

namespace Common.Info {
    class ErrorMessage : IBaseModel {
        public string Message { get; set; }

        public int? StatusCode { get; set; }

        public string Property { get; set; }

        public ErrorMessage(string message, string property) {
            Message = message;
            Property = !string.IsNullOrEmpty(property) ? property : "";
        }

        public ErrorMessage() { }


    }
}
