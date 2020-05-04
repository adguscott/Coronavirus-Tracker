using Coronavirus_Tracker.ViewModels;
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
using System.Windows.Shapes;

namespace Coronavirus_Tracker.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        private CoronavirusTrackerViewModel ViewModel;
        public MainView()
        {
            InitializeComponent();
            ViewModel = new CoronavirusTrackerViewModel();
            DataContext = ViewModel;
        }

        private async void Window_ContentRendered(object sender, EventArgs e)
        {
            await ViewModel.GetData();
        }

        private void Track_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.TrackCountry();
        }

        private void Untrack_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveTrackedCountry();
        }

        private void UntrackAll_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.RemoveAllTrackedCountries();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.Refresh();
        }
    }
}
