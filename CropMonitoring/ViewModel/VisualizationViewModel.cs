using CropMonitoring.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using CropMonitoring.Model;
using CropMonitoring.Helpers;
using System.IO;
using CropMonitoring.Graphics;
using System.Collections.ObjectModel;

namespace CropMonitoring.ViewModel
{
    class VisualizationViewModel:ViewModelBase
    {
        private Svg.SvgDocument svgDocument;
        private string selectedPath = @"D:\Projects\CropMonitoring\CropMonitoring\CropMonitoring\Images\ukraineHigh.svg";

        BitmapImage _bitmapImage;
        public BitmapImage BitmapImage
        {
            get
            {
                if (_bitmapImage == null)
                {
                    svgDocument = SVGParser.GetSvgDocument(selectedPath);
                    _bitmapImage =  SVGParser.BitmapToImageSource(SVGParser.GetBitmapFromSVG(selectedPath));

                    //_bitmapImage.UriSource = new Uri("pack://application:,,,/Images/ukraineHigh.svg");

                }

                return _bitmapImage;
            }
            set
            {
                _bitmapImage = value;
            }
        }

       RelayCommand _getBitmapImage;
       public ICommand GetBitmapImage
        {
          get
            {
                if (_getBitmapImage == null)
                    _getBitmapImage = new RelayCommand(ExecuteGetBitmapImage,CanExecuteGetBitmapImageCommand);
                return _getBitmapImage;
            }
        }

        public void ExecuteGetBitmapImage(object parameter)
        {
            var yw = parameter as YearWeek;

            if (yw != null)
            {
                int year = yw.year;
                int week = yw.week;
                MapColorWorker mapWorker = new MapColorWorker();
                Dictionary<int, string> provinces = InitialProvinces.GetProvinces();
                foreach (KeyValuePair<int, string> item in provinces)
                {
                    double _vhi = VHIDataContext.SelectByYearWeek(year, week, item.Value);
                    mapWorker.ApplyColors(svgDocument, item.Key, _vhi);
                }

                BitmapImage = SVGParser.BitmapToImageSource(svgDocument.Draw());
                OnPropertyChanged("BitmapImage");
            }
        }
       
        public bool CanExecuteGetBitmapImageCommand(object parameter)
        {
            return true;
        }

        protected override void OnDispose()
        {
            
        }

        ObservableCollection<YearWeek> _yearWeek;
        public ObservableCollection<YearWeek> YearWeek
        {
            get
            {
                if (_yearWeek == null)
                {
                    _yearWeek = VHIDataContext.GetYearWeekList();
                    return _yearWeek;
                }
                return _yearWeek;
            }
            set
            {
                _yearWeek = value;
            }
        }

        private YearWeek _selectedYearWeek;
        public YearWeek  SelectedYearWeek
        {
            get
            {
                if (_selectedYearWeek != null)
                    ExecuteGetBitmapImage(_selectedYearWeek);
                return _selectedYearWeek;
            }

            set
            {
                _selectedYearWeek= value;
            }
        }



    }
}
