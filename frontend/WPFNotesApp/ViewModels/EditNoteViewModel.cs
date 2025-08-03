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

        private string _saveStatus;
        public string SaveStatus
        {
            get => _saveStatus;
            set => SetProperty(ref _saveStatus, value);
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
            _eventAggregator.GetEvent<NoteSavedSuccesfully>().Subscribe(async () => await OnNoteSavedAsync());
        }

        private void SaveNote()
        {
            _eventAggregator.GetEvent<NoteSaveRequestedEvent>().Publish(new NoteRead() { Id = Id, Title = Title, Body = Body });
            SaveStatus = "Saving...";

        }

        private async Task OnNoteSavedAsync()
        {
            await Task.Delay(250);
            SaveStatus = "Saved ✓";
            await Task.Delay(1500);
            SaveStatus = string.Empty;
        }

    }
}
