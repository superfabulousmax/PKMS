using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WPFNotesApp.Connector;
using WPFNotesApp.Models;
using System.Net.Http.Json;

namespace WPFNotesApp.Services
{
    public interface INoteStore
    {
        public Task<List<NoteRead>> GetAllNotes();
    }
    public class DbNoteStore: INoteStore
    {
        public DbNoteStore() { }

        private readonly HttpClient _httpClient = BackendConnector.Instance.Client;

        public async Task<List<NoteRead>> GetAllNotes()
        {
            var notes = await _httpClient.GetFromJsonAsync<List<NoteRead>>("notes/");
            return notes ?? new List<NoteRead>();
        }
    }
}
