using Common;
using Common.Data;
using Newtonsoft.Json;
using System;

namespace Common.Info {
    public class OrderInfo {

        [JsonProperty("dateSent")]
        public DateTime DateSent { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public User User { get; set; }

    }
}