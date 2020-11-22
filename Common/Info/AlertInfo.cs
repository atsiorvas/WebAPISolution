using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Info {

    [Serializable]
    public class AlertInfo : DTO {

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("arguments")]
        public IList<string> Arguments { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("fromDate")]
        public DateTime FromDate { get; set; }

        [JsonProperty("toDate")]
        public DateTime ToDate { get; set; }

        [JsonProperty("dateSent")]
        public DateTime DateSent { get; set; }

        [JsonProperty("userName")]
        public string UserName { get; set; }
        
    }
}