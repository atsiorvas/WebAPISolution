using Common.Info;
using MediatR;
using Newtonsoft.Json;
using System;

namespace Common.Info {

    [Serializable]
    public class NotesModel : DTO {

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("lang")]
        public int Lang { get; set; }

        public static NotesModel operator +(NotesModel b, NotesModel c) {
            NotesModel note = new NotesModel {
                Text = b.Text + c.Text,
                Lang = 3
            };
            return note;
        }

    }
}