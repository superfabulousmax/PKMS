using System.Text.Json.Serialization;

namespace WPFNotesApp.Models
{
    public class NoteCreate
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}