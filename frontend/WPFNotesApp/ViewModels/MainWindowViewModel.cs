using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Windows.Input;
using WPFNotesApp.Models;
using WPFNotesApp.Events;
using WPFNotesApp.Services;

namespace WPFNotesApp.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private INoteStore _noteStore;

        public ObservableCollection<NoteViewModel> Notes { get; set; } = new();

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

        public ICommand AddNoteCommand { get; }

        public MainWindowViewModel(INoteStore noteStore, IEventAggregator eventAggregator)
        {
            _noteStore = noteStore;
            _eventAggregator = eventAggregator;
            _eventAggregator
                .GetEvent<NoteDeletedEvent>()
                .Subscribe(OnNoteDeleted);
            _eventAggregator
                .GetEvent<NoteSaveRequestedEvent>()
                .Subscribe(OnNoteSaved);

            AddNoteCommand = new RelayCommand(AddNote);
            LoadNotesAsync();
        }

        private async void LoadNotesAsync()
        {
            var notes = await _noteStore.GetAllNotes();
            if (notes == null)
            {
                return;
            }
            Notes.Clear();
            foreach (var note in notes)
            {
                var vm = new NoteViewModel(note, _eventAggregator, _noteStore);
                Notes.Add(vm);
            }
            
        }

        private async void AddNote()
        {
            var newNote = new NoteCreate { Title = Title, Body = Body };

            var response = await _noteStore.AddNote(newNote);

            if (response.IsSuccessStatusCode)
            {
                var note = await response.Content.ReadFromJsonAsync<NoteRead>();
                if (note == null)
                {
                    return;
                }
                var vm = new NoteViewModel(note, _eventAggregator, _noteStore);
                Notes.Add(vm);
                Title = string.Empty;
                Body = string.Empty;
            }
        }

        private async void OnNoteSaved(NoteRead note)
        {
            var response = await _noteStore.UpdateNote(note);
            if (response.IsSuccessStatusCode)
            {
                _eventAggregator.GetEvent<NoteSavedSuccesfully>().Publish();
                LoadNotesAsync();
            }
        }

        private async void OnNoteDeleted(NoteViewModel note)
        {
            var response = await _noteStore.DeleteNote(note.Id);
            if (response.IsSuccessStatusCode)
            {
                Notes.Remove(note);
            }
        }
    }
}
