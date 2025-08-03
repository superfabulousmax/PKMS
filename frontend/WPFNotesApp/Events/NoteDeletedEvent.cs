using WPFNotesApp.ViewModels;

namespace WPFNotesApp.Events
{
    public class NoteDeletedEvent : PubSubEvent<NoteViewModel>
    {
    }
}
