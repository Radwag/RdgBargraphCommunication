namespace RdgBargraphCommunication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            MainViewModel mainViewModel = new MainViewModel();
            InitializeComponent();
            DataContext = mainViewModel;
            CommandBindings.AddRange(mainViewModel.Commands);
        }
    }
}