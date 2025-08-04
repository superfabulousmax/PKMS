using System.Windows;

namespace WPFNotesApp.Events
{
    public class NoteSearchRequestedEvent: PubSubEvent<NoteSearchRequest>
    {
    }

    public class NoteSearchRequest
    {
        public string Query { get; set; }

        // screen position of caret
        public Point Position { get; set; } 
    }

}
