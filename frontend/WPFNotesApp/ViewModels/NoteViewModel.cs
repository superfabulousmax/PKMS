using System.Net.Http;
using System.Windows.Input;
using WPFNotesApp.Connector;
using WPFNotesApp.Events;
using WPFNotesApp.Models;
using WPFNotesApp.Views;

namespace WPFNotesApp.ViewModels
{
    public class NoteViewModel
    {
        private readonly HttpClient _httpClient = BackendConnector.Instance.Client;
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

        private async void DeleteNote()
        {
            var response = await _httpClient.DeleteAsync($"notes/{Id}");
            if (response.IsSuccessStatusCode)
            {
                _eventAggregator.GetEvent<NoteDeletedEvent>().Publish(this);
                
                Console.WriteLine("Successfully deleted");
            }
        }

        private void OpenNote()
        {
            editViewModel = new EditNoteViewModel(_note, _eventAggregator);

            editWindow = new EditNoteView { DataContext = editViewModel };
            editWindow.ShowDialog();
        }

    }
}
