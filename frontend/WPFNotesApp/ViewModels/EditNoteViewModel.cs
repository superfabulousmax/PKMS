using System.Windows.Input;
using WPFNotesApp.Models;
using WPFNotesApp.Events;

namespace WPFNotesApp.ViewModels
{
    public class EditNoteViewModel: BindableBase
    {   
        public int Id { get;private set; }

        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private string _body;
        public string Body
        {
            get => _body;
            set => SetProperty(ref _body, value);
        }

        public ICommand SaveCommand { get; }

        private readonly IEventAggregator _eventAggregator;

        public EditNoteViewModel(NoteRead note, IEventAggregator eventAggregator)
        {
            Id = note.Id;
            Title = note.Title;
            Body = note.Body;
            SaveCommand = new RelayCommand(SaveNote);
            _eventAggregator = eventAggregator;
        }

        private void SaveNote()
        {
            _eventAggregator.GetEvent<NoteSavedEvent>().Publish(new NoteRead() { Id = Id, Title = Title, Body = Body });
        }
    }
}
