using Labb3_HenrikVu.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Labb3_HenrikVu.View
{
    /// <summary>
    /// Interaction logic for MenuView.xaml
    /// </summary>
    public partial class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        public void NewQuestionPack_Click(object sender, RoutedEventArgs e)
        {
            CreatePackView createPackView = new CreatePackView();
            createPackView.ShowDialog();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = App.Current.MainWindow;
            mainWindow.Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            EditPackView editPackView = new EditPackView();
            editPackView.ShowDialog();
        }

        private void PlayWindow_Click(object sender, RoutedEventArgs e)
        {
            PlayWindow.IsEnabled = !PlayWindow.IsEnabled;
            EditWindow.IsEnabled = !PlayWindow.IsEnabled;
            HiddenBoolean.IsChecked = !HiddenBoolean.IsChecked;
        }
    }   
}
