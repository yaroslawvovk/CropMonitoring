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
            ObjectDataProvider o = new ObjectDataProvider();
        }

        private async void Download_Click(object sender, RoutedEventArgs e)
        {
            ProgressBar.Value = 0;
            Loader load = new VHIDataLoader(ProgressBar);
            Loader load2 = new VHIPercentageLoader(ProgressBar);


            string _key = comBoxAreas.SelectedValue.ToString();
            string _value = comBoxAreas.Text;
            await load.DownloadAndSaveData(_value, _key);
            await load2.DownloadAndSaveData(_value, _key);
        }
    }
   

}
