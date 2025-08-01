using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WPFNotesApp.ViewModels
{
    public abstract class ObservableObject : BindableBase
    {
        protected ObservableObject()
        {
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }

        /// <summary>
        /// Manually raise it
        /// </summary>
        /// <param name="name"></param>

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
