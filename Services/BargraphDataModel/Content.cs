using System.Collections.ObjectModel;

namespace Services.BargraphDataModel
{
    public class Content: PropertyChangedHelper
    {
        public int Address { get; set; } = 255;
        public bool IsRequest { get; set; } = true;
        public string Command { get; set; } = "BargraphSet";
        public int Power { get; set; } = 50;

        private ObservableCollection<int>_green = new ObservableCollection<int>(){0, 0, 0, 0, 0, 0, 0, 0, 0};
        public ObservableCollection<int> Green
        {
            get => _green;
            set
            {
                _green = value;
                OnPropertyChanged();
            }
            
        }
        private ObservableCollection<int> _red { get; set; } = new ObservableCollection<int>(){0, 0, 0, 0, 0, 0, 0, 0, 0};

        public ObservableCollection<int> Red
        {
            get => _red;
            set
            {
                _red = value;
                OnPropertyChanged();
            }
            
        }
    }
}