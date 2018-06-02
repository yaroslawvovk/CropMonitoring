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
using CropMonitoring.UserNotify;


namespace CropMonitoring.ViewModel
{
    class VisualizationViewModel:ViewModelBase
    {
        INotifyUser notify = new NotifyMessage();
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
                     _bitmapImage = SVGParser.BitmapToImageSource(SVGParser.GetBitmapFromSVG(selectedPath));
                    //_bitmapImage = new BitmapImage(new Uri("pack://application:,,,/Images/DSC_0239.JPG"));              
                    
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
                if (_yearWeek == null||_yearWeek.Count==0)
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


        RelayCommand _fillAreaByWeek;
       public ICommand FillAreaByWeek
        {
            get
            {
                if (_fillAreaByWeek == null)
                    _fillAreaByWeek = new RelayCommand(ExecuteFillArea, CanExecuteFillArea);
                return _fillAreaByWeek;
            }

        }

        public void ExecuteFillArea(object parameter)
        {
            var items = (Tuple<string, string>)parameter;
            int year = 0;
            int week = 0;

           

            if (Int32.TryParse(items.Item1,out year)&&Int32.TryParse(items.Item2,out week))
            {
                if (year < 1981 || year > 2018)
                {
                    notify.Message("Year should be in range of 1981 to 2018");
                    return;
                }
                if (week < 1 || week > 52)
                {
                    notify.Message("Week should be in range 1 to 52");
                    return;
                }
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

        public bool CanExecuteFillArea(object parameter)
        {
            var items = (Tuple<string, string>)parameter;
            if (items == null || string.IsNullOrEmpty(items.Item1) || string.IsNullOrEmpty(items.Item2))
                 return false;
            return true;

        }
    }
}
