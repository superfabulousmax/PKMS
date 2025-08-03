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
        private INoteStore _noteStore = null;

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
                .GetEvent<NoteSavedEvent>()
                .Subscribe(OnNoteSaved);

            AddNoteCommand = new RelayCommand(AddNote);
            LoadNotesAsync();
        }

        private void OnNoteSaved(NoteRead note)
        {
            _noteStore.UpdateNote(note);
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
                var vm = new NoteViewModel(note, _eventAggregator);
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
                var vm = new NoteViewModel(note, _eventAggregator);
                Notes.Add(vm);
                Title = string.Empty;
                Body = string.Empty;
            }
        }

        private void OnNoteDeleted(NoteViewModel note)
        {
            _noteStore.DeleteNote(note.Id);
            Notes.Remove(note);
        }
    }
}
