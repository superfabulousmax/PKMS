using System.Windows.Input;
using WPFNotesApp.Events;
using WPFNotesApp.Models;
using WPFNotesApp.Views;

namespace WPFNotesApp.ViewModels
{
    public class NoteViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public ICommand DeleteNoteCommand { get; }
        public ICommand OpenNoteCommand { get; }


        EditNoteViewModel editViewModel;
        EditNoteView editWindow;

        private readonly IEventAggregator _eventAggregator;
        private NoteRead _note;
        public NoteViewModel(NoteRead note, IEventAggregator eventAggregator)
        {
            DeleteNoteCommand = new RelayCommand(DeleteNote);
            OpenNoteCommand = new RelayCommand(OpenNote);
            Id = note.Id;
            Title = note.Title;
            Body = note.Body;
            _note = note;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<NoteSavedEvent>().Subscribe(OnNoteSaved);
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
            editViewModel = new EditNoteViewModel(_note, _eventAggregator);
            editWindow = new EditNoteView { DataContext = editViewModel };
            editWindow.ShowDialog();
        }

    }
}
