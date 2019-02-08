using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Common.Info {

    [Serializable]
    public class UserModel : DTO {

        //[JsonConverter(typeof(EmailConverter))]
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("rememberMe")]
        public bool RememberMe { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("resetAnswer")]
        public string ResetAnswer { get; set; }

        [JsonProperty("note")]
        public HashSet<NotesModel> Note { get; set; }
    }
    public class EmailConverter : JsonConverter {

        public override object ReadJson(JsonReader reader, Type objectType,
            object existingValue, JsonSerializer serializer) {
            string email = (string)reader.Value;
            email = email.Trim();
            return email;
        }

        public override bool CanConvert(Type objectType) {
            return objectType == typeof(UserModel);
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}