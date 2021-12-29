using System.Windows;

namespace sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private MainViewModel ViewModel => (MainViewModel)DataContext;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Double1 = 2000.1001;
        }
    }
}
