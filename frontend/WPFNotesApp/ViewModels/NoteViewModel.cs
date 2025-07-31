using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFNotesApp.Connector;
using WPFNotesApp.Models;

namespace WPFNotesApp.ViewModels
{
    public class NoteViewModel
    {
        private readonly HttpClient _httpClient = BackendConnector.Instance.Client;
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        public ICommand DeleteNoteCommand { get; }

        public event EventHandler<NoteViewModel>? DeleteRequested;

        public NoteViewModel(int id, string title, string body)
        {
            DeleteNoteCommand = new RelayCommand(DeleteNote);
            Id = id;
            Title = title;
            Body = body;
        }

        private async void DeleteNote()
        {
            var response = await _httpClient.DeleteAsync($"notes/{Id}");
            if (response.IsSuccessStatusCode)
            {
                DeleteRequested?.Invoke(this, this);
                Console.WriteLine("Successfully deleted");
            }
        }

    }
}
