using System.Net.Http;
using WPFNotesApp.Connector;
using WPFNotesApp.Models;
using System.Net.Http.Json;
using System.Net;

namespace WPFNotesApp.Services
{
    public interface INoteStore
    {
        public Task<List<NoteRead>> GetAllNotes();
        public Task<HttpResponseMessage> AddNote(NoteCreate note);
        public void UpdateNote(NoteRead noteRead);
        public void DeleteNote(int id);
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

        public async void UpdateNote(NoteRead note)
        {
            var _ = await _httpClient.PutAsJsonAsync($"notes/{note.Id}", note);
        }

        public async void DeleteNote(int id)
        {
            var _ = await _httpClient.DeleteAsync($"notes/{id}");
        }

        public async Task<HttpResponseMessage> AddNote(NoteCreate note)
        {
            return await _httpClient.PostAsJsonAsync("notes/", note);
        }
    }
}
