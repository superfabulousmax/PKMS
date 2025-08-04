using System.Collections.ObjectModel;
using WPFNotesApp.Events;
using WPFNotesApp.Models;

namespace WPFNotesApp.ViewModels
{
    public class AutoCompletePopupViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;

        public AutoCompletePopupViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        public ObservableCollection<NoteRead> Suggestions { get; set; } = new();

        private NoteRead _selectedSuggestion;
        public NoteRead SelectedSuggestion
        {
            get => _selectedSuggestion;
            set
            {
                if (SetProperty(ref _selectedSuggestion, value) && value != null)
                {
                    _eventAggregator.GetEvent<NoteLinkRequestedEvent>().Publish(value);
                }
            }
        }

        private bool _isOpen;
        public bool IsOpen
        {
            get => _isOpen;
            set => SetProperty(ref _isOpen, value);
        }

        private double _offsetX;
        public double OffsetX
        {
            get => _offsetX;
            set => SetProperty(ref _offsetX, value);
        }

        private double _offsetY;
        public double OffsetY
        {
            get => _offsetY;
            set => SetProperty(ref _offsetY, value);
        }
    }

}
