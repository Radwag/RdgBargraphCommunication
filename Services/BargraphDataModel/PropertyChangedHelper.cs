using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Services.BargraphDataModel
{
    public abstract class PropertyChangedHelper : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}