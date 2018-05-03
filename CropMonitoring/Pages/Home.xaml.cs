using CropMonitoring.Downloaders;
using CropMonitoring.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CropMonitoring.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();

            this.DataContext = new HomeViewModel();
            //this.DataGridXaml.ItemsSource = 
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Loader load = new VHIDataLoader(ProgressBar);
            Loader load2 = new VHIPercentageLoader(ProgressBar);
            await load.DownloadAndSaveData("Piska", "1");
            await load2.DownloadAndSaveData("Piska", "1");
        }
    }

}
