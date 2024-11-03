using Labb3_HenrikVu.Model;
using Labb3_HenrikVu.View;
using Labb3_HenrikVu.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb3_HenrikVu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            this.StateChanged += OnWindowStateChanged;
        }

        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            if(DataContext is MainWindowViewModel mainWindowViewModel)
            {
                mainWindowViewModel.ConfigurationViewModel.IsFullScreen = (this.WindowState == WindowState.Maximized);
            }
        }

    }
}