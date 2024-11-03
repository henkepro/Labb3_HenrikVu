using Labb3_HenrikVu.ViewModel;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PlayerView.xaml
    /// </summary>
    public partial class PlayerView : UserControl
    {
        public PlayerView()
        {
            InitializeComponent();
        }
        private void Button_Click0(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                button.CommandParameter = ((PlayerViewModel) DataContext).RandomizedAnswerList[0];
            }
        }
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                button.CommandParameter = ((PlayerViewModel) DataContext).RandomizedAnswerList[1];
            }
        }
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                button.CommandParameter = ((PlayerViewModel) DataContext).RandomizedAnswerList[2];
            }
        }
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                button.CommandParameter = ((PlayerViewModel) DataContext).RandomizedAnswerList[3];
            }
        }

        private void ButtonRestart_Click(object sender, RoutedEventArgs e)
        {
            //((PlayerViewModel) DataContext).RandomizedAnswerList[3];
        }
    }
}
