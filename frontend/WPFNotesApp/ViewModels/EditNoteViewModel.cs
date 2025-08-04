using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WPFNotesApp.Events;
using WPFNotesApp.Models;
using WPFNotesApp.Services;

namespace WPFNotesApp.ViewModels
{
    public class EditNoteViewModel: BindableBase
    {
        private readonly INoteStore _noteStore;
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
            set =>  SetProperty(ref _body, value);
        }

        private string _saveStatus;
        public string SaveStatus
        {
            get => _saveStatus;
            set => SetProperty(ref _saveStatus, value);
        }

        public ICommand SaveCommand { get; }

        private readonly IEventAggregator _eventAggregator;

        public AutoCompletePopupViewModel PopupViewModel { get; }

        public EditNoteViewModel(NoteRead note, IEventAggregator eventAggregator, INoteStore noteStore)
        {
            Id = note.Id;
            Title = note.Title;
            Body = note.Body;
            SaveCommand = new RelayCommand(SaveNote);
            PopupViewModel = new AutoCompletePopupViewModel(eventAggregator);
            _noteStore = noteStore;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<NoteSavedSuccesfully>().Subscribe(async () => await OnNoteSavedAsync());
            _eventAggregator.GetEvent<NoteSearchRequestedEvent>().Subscribe(OnSearchNoteRequested);

        }

        private async void OnSearchNoteRequested(NoteSearchRequest request)
        {
            var suggestions = await _noteStore.SearchNotes(request.Query);
            Application.Current.Dispatcher.Invoke(() =>
            {
                PopupViewModel.Suggestions.Clear();
                PopupViewModel.Suggestions.AddRange(suggestions);

                PopupViewModel.OffsetX = request.Position.X;
                PopupViewModel.OffsetY = request.Position.Y;
                PopupViewModel.IsOpen = true;
            });
        }

        private void SaveNote()
        {
            _eventAggregator.GetEvent<NoteSaveRequestedEvent>().Publish(new NoteRead() { Id = Id, Title = Title, Body = Body });
            SaveStatus = "Saving...";
        }

        private async Task OnNoteSavedAsync()
        {
            await Task.Delay(250);
            SaveStatus = "Saved ✓";
            await Task.Delay(1500);
            SaveStatus = string.Empty;
        }

    }
}
