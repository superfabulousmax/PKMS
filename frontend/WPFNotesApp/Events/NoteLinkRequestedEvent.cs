using WPFNotesApp.Models;

namespace WPFNotesApp.Events
{
    public class NoteLinkRequestedEvent: PubSubEvent<NoteRead>
    {
    }
}
