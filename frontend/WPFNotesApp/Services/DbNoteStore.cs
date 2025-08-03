using System.Net.Http;
using WPFNotesApp.Connector;
using WPFNotesApp.Models;
using System.Net.Http.Json;

namespace WPFNotesApp.Services
{
    public interface INoteStore
    {
        public Task<List<NoteRead>> GetAllNotes();
        public Task<HttpResponseMessage> AddNote(NoteCreate note);
        public Task<HttpResponseMessage> UpdateNote(NoteRead noteRead);
        public Task<HttpResponseMessage> DeleteNote(int id);
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

        public async Task<HttpResponseMessage> UpdateNote(NoteRead note)
        {
            return await _httpClient.PutAsJsonAsync($"notes/{note.Id}", note);
        }

        public async Task<HttpResponseMessage> DeleteNote(int id)
        {
            return await _httpClient.DeleteAsync($"notes/{id}");
        }

        public async Task<HttpResponseMessage> AddNote(NoteCreate note)
        {
            return await _httpClient.PostAsJsonAsync("notes/", note);
        }
    }
}
