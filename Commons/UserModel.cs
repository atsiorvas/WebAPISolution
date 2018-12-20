using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Common {

    [Serializable]
    public class UserModel : DTO {

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

}