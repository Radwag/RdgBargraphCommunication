using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Services;

namespace RdgBargraphCommunication
{
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// List of available serial ports
        /// </summary>
        public string[] SerialPorts { get; set; } = SerialPort.GetPortNames();

        /// <summary>
        /// Selected serial port name
        /// </summary>
        private object _selectedSerialPort;

        public object SelectedSerialPort
        {
            get => _selectedSerialPort;
            set
            {
                _selectedSerialPort = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Initialize BargraphEngine Engine
        /// </summary>
        public BargraphEngine BargraphEngine { get; set; } = new BargraphEngine();


        /// <summary>
        /// UI commands
        /// </summary>
        private readonly CommandBindingCollection _commandBindingCollection = new CommandBindingCollection();

        public CommandBindingCollection Commands
        {
            get
            {
                _commandBindingCollection.Clear();
                _commandBindingCollection.Add(new CommandBinding(ConnectCommand, OnConnectCommandExecuted));
                _commandBindingCollection.Add(new CommandBinding(SetPowerCommand, OnSetPowerCommandExecuted));
                return _commandBindingCollection;
            }
        }

        public RoutedUICommand ConnectCommand { get; set; } =
            new RoutedUICommand("Connect", "ConnectCommand", typeof(MainWindow));

        void OnConnectCommandExecuted(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            if (BargraphEngine.Sender == null)
            {
                BargraphEngine.Connect(SelectedSerialPort.ToString()).Wait();
            }
            else
            {
                BargraphEngine.Dispose().Wait();
            }
        }

        public RoutedUICommand SetPowerCommand { get; set; } =
            new RoutedUICommand("SetPower", "SetPowerCommand", typeof(MainWindow));

        void OnSetPowerCommandExecuted(object sender, ExecutedRoutedEventArgs eventArgs)
        {
            BargraphEngine.SetBargraphSettings();
        }

        /// <summary>
        /// constructor of MainViewModel
        /// </summary>
        public MainViewModel()
        {
            BargraphEngine.QueryModel.Content.Green.CollectionChanged += (sender, args) =>
            {
                BargraphEngine.SetBargraphSettings();
            };
            BargraphEngine.QueryModel.Content.Red.CollectionChanged += (sender, args) =>
            {
                BargraphEngine.SetBargraphSettings();
            };
        }

        /// <summary>
        /// PropertyChangedEventHandler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}