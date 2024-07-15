using Pulisher.UI.ViewModel;
using System.Windows;

namespace Pulisher.UI
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

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainViewModel mvm)
            {
                await mvm.RefreshDataAsync();
            }
        }
    }
}