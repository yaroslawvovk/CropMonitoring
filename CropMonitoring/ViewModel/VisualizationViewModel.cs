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

            int year = 2017;
            int week = 40;
            MapColorWorker mapWorker = new MapColorWorker();
            Dictionary<int, string> provinces = InitialProvinces.GetProvinces();
            //Dictionary<int, double> provinceVHI = new Dictionary<int, double>();
            foreach (KeyValuePair<int, string> item in provinces)
            {
               double _vhi =  VHIDataContext.SelectByYearWeek(year, week, item.Value);
                mapWorker.ApplyColors(svgDocument,item.Key,_vhi);
                //provinceVHI.Add(item.Key, _vhi);
            }

            BitmapImage = SVGParser.BitmapToImageSource(svgDocument.Draw());



            OnPropertyChanged("BitmapImage");
        }
       
        public bool CanExecuteGetBitmapImageCommand(object parameter)
        {
            return true;
        }

        protected override void OnDispose()
        {
            
        }

    }
}
