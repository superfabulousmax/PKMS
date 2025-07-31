using System;
using System.Net.Http;

namespace WPFNotesApp.Connector
{
    public sealed class BackendConnector
    {
        private static readonly Lazy<BackendConnector> _instance =
            new(() => new BackendConnector());

        private readonly HttpClient _httpClient;

        private BackendConnector()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8000/")
            };
        }

        public static BackendConnector Instance => _instance.Value;

        public HttpClient Client => _httpClient;
    }
}
