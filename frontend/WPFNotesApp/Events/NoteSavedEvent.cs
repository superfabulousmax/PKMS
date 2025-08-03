using WPFNotesApp.Models;

namespace WPFNotesApp.Events
{
    public class NoteSavedEvent: PubSubEvent<NoteRead>
    {
    }
}
