using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WPFNotesApp.Models;
using WPFNotesApp.Connector;

namespace WPFNotesApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient = BackendConnector.Instance.Client;

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

        public MainViewModel()
        {
            AddNoteCommand = new RelayCommand(AddNote);
            LoadNotesAsync();
        }

        private async void LoadNotesAsync()
        {
            var notes = await _httpClient.GetFromJsonAsync<List<NoteRead>>("notes/");
            Notes.Clear();
            foreach (var note in notes)
            {
                var vm = new NoteViewModel(note.Id, note.Title, note.Body);
                Notes.Add(vm);
                vm.DeleteRequested += OnNoteDeleteRequested;
            }
            
        }

        private async void AddNote()
        {
            var newNote = new NoteCreate { Title = Title, Body = Body };

            var response = await _httpClient.PostAsJsonAsync("notes/", newNote);

            if (response.IsSuccessStatusCode)
            {
                var note = await response.Content.ReadFromJsonAsync<NoteRead>();
                var vm = new NoteViewModel(note.Id, note.Title, note.Body);
                Notes.Add(vm);
                vm.DeleteRequested += OnNoteDeleteRequested;
                Title = string.Empty;
                Body = string.Empty;
                Console.WriteLine("Note successfully added");
            }
        }

        private void OnNoteDeleteRequested(object? sender, NoteViewModel note)
        {
            note.DeleteRequested -= OnNoteDeleteRequested;
            Notes.Remove(note);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
