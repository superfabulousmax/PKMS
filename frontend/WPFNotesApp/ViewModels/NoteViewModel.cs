using System.Windows.Input;
using WPFNotesApp.Events;
using WPFNotesApp.Models;
using WPFNotesApp.Services;
using WPFNotesApp.Views;

namespace WPFNotesApp.ViewModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string PreviewBody =>
            Body.Length > PreviewLength ? Body.Substring(0, PreviewLength) + "..." : Body;

        public ICommand DeleteNoteCommand { get; }
        public ICommand OpenNoteCommand { get; }

        private readonly IEventAggregator _eventAggregator;
        private readonly INoteStore _noteStore;
        private const int PreviewLength = 100;

        private EditNoteViewModel _editViewModel;
        private EditNoteView _editWindow;

        private NoteRead _note;
        public NoteViewModel(NoteRead note, IEventAggregator eventAggregator, INoteStore noteStore)
        {
            DeleteNoteCommand = new RelayCommand(DeleteNote);
            OpenNoteCommand = new RelayCommand(OpenNote);
            Id = note.Id;
            Title = note.Title;
            Body = note.Body;
            _note = note;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<NoteSaveRequestedEvent>().Subscribe(OnNoteSaved);
            _noteStore = noteStore;
        }

        private void OnNoteSaved(NoteRead note)
        {
            _note = note;
            Title = note.Title;
            Body = note.Body;
        }

        private void DeleteNote()
        {
            _eventAggregator.GetEvent<NoteDeletedEvent>().Publish(this);
        }

        private void OpenNote()
        {
            _editViewModel = new EditNoteViewModel(_note, _eventAggregator, _noteStore);
            _editWindow = new EditNoteView { DataContext = _editViewModel };
            _editWindow.ShowDialog();
        }

    }
}
