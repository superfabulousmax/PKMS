using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFNotesApp.Connector;
using WPFNotesApp.Models;
using Prism.Unity;
using WPFNotesApp.Events;

namespace WPFNotesApp.ViewModels
{
    public class EditNoteViewModel: ObservableObject
    {
        private readonly HttpClient _httpClient = BackendConnector.Instance.Client;
        
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

        private async void SaveNote()
        {
            var note = new NoteCreate { Title = Title, Body = Body };
            var response = await _httpClient.PutAsJsonAsync($"notes/{Id}", note);
            if (response.IsSuccessStatusCode)
            {
                //
                _eventAggregator.GetEvent<NoteSavedEvent>().Publish(new NoteRead() { Id = Id, Title = Title, Body = Body }); 
            }
        }
    }
}
