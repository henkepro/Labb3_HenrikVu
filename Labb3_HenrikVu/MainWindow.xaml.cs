using Labb3_HenrikVu.Command;
using Labb3_HenrikVu.Model;
using Labb3_HenrikVu.View;
using Labb3_HenrikVu.ViewModel;
using System.Diagnostics;
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

        public ICommand CreateNewWindowOnCommand { get; }
        public ICommand EditPackWindowOnCommand { get; }
        public ICommand ExitWindowOnCommand { get; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            CreateNewWindowOnCommand = new DelegateCommand(CreateNewWindow);
            EditPackWindowOnCommand = new DelegateCommand(EditPackWindow);
            ExitWindowOnCommand = new DelegateCommand(ExitWindow);

            this.StateChanged += OnWindowStateChanged;
        }

        public void CreateNewWindow(object obj)
        {
            CreatePackView createPackView = new CreatePackView();
            createPackView.ShowDialog();
        }
        private void EditPackWindow(object obj)
        {
            EditPackView editPackView = new EditPackView();
            editPackView.ShowDialog();
        }

        private void ExitWindow(object obj)
        {
            this.Close();
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