using System.Text.Json.Serialization;

namespace WPFNotesApp.Models
{
    public class NoteRead
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}