using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFNotesApp.Events;
using WPFNotesApp.Models;
using WPFNotesApp.ViewModels;

namespace WPFNotesApp.Views
{
    /// <summary>
    /// Interaction logic for AutoCompletePopupView.xaml
    /// </summary>
    public partial class AutoCompletePopupView : UserControl
    {
        public AutoCompletePopupView()
        {
            InitializeComponent();
            //DataContext = new AutoCompletePopupViewModel();
        }

        private void OnSuggestionSelected(object sender, MouseButtonEventArgs eventArgs)
        {
            if (((ListBox)sender).SelectedItem is NoteRead selectedNote)
            {
                var eventAggregator = ((PrismApplication)Application.Current).Container.Resolve<IEventAggregator>();
                eventAggregator.GetEvent<NoteLinkRequestedEvent>().Publish(selectedNote);
                AutoCompletePopup.IsOpen = false;

            }
        }
    }
   
}
