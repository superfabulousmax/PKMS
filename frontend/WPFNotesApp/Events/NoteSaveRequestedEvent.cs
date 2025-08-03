using WPFNotesApp.Models;

namespace WPFNotesApp.Events
{
    public class NoteSaveRequestedEvent: PubSubEvent<NoteRead>
    {
    }
}
