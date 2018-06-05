using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CropMonitoring.Contracts;
using CropMonitoring.Infrastructure;
using System.Windows.Input;
using OxyPlot;
using System.Globalization;

namespace CropMonitoring.ViewModel
{
    class ForecastViewModel : ViewModelBase
    {
        INDVIDataAccess dataAccees = new Model.NDVIDataAccess();

        RelayCommand _forecastCommand;
        public ICommand ForecastCommand
        {
            get
            {
                if (_forecastCommand == null)
                    _forecastCommand = new RelayCommand(MakeForecast, CanMakeForecast);
                return _forecastCommand;
            }
        }

        string _selectedNDVIFile;
        public string SelectedNDVIFile
        {
            get
            {
                return _selectedNDVIFile;
            }
            set
            {
                _selectedNDVIFile = value;
            }
        }

        RelayCommand _deleteNDVIFile;
        public ICommand DeleteNDVICommand
        {
            get
            {
                if (_deleteNDVIFile == null)
                    return new RelayCommand(DeleteNDVIFileCommand, CanDeleteNDVIFileCommand);
                return _deleteNDVIFile;
            }

        }

        public void DeleteNDVIFileCommand(object parameter)
        {

            dataAccees.DeleteNDVIFile(_selectedNDVIFile);
            OnPropertyChanged("DTime");
        }
        public bool CanDeleteNDVIFileCommand(object parameter)
        {
            return true;
        }


        DataPoint[] _dataSeries;
        public DataPoint[] DataSeries
        {
            get
            {
                return _dataSeries;
            }
            set
            {
                _dataSeries = value;
            }
        }

        DateTime _dTime;
        public DateTime DTime
        {
            get
            {
                FileNames = GetFilesByMonth(_dTime);
                Month = _dTime.ToString("MMMM", CultureInfo.CreateSpecificCulture("en"));
                OnPropertyChanged("Month ");
                OnPropertyChanged("FileNames");
                return _dTime;
            }
            set
            {
                _dTime = value;
            }
        }

    

        string _resultString;
        public string ResultString
        {
            get
            {
                if (_month == null || _month == ""||_resultString==null)
                    return "Forecast";
                return _resultString;
            }
            set
            {
                _resultString = value;
            }
        }

        string _month;
        public string Month
        {
            get
            {
                return _month;
            }
            set
            {
                _month = value;
            }
        }

       



        List<string> _fileNames;
        public List<string> FileNames
        {
            get
            {
                if (_fileNames == null)
                    return new List<string>();
                return _fileNames;
            }
            set
            {
                _fileNames = value;
            }

        }

        private List<string> GetFilesByMonth(DateTime date)
        {
            int month = date.Month;
            return dataAccees.GetNDVIFilesNames(month);
        }

        public void MakeForecast(object parameter)
        {
            List<Model.NDVIData> ndviList;
            Model.NDVIData forecastData;
            ndviList = dataAccees.GetNDVIData(_fileNames);
            DataSeries = ndviList.Select(x => new DataPoint(x.Year, x.NDVI)).ToArray();
            OnPropertyChanged("DataSeries");
            Forecast.NDVIForecast forecast = new Forecast.ExponentialSmooth();
            forecastData = forecast.GetForecastedData(ndviList);
            if (forecastData != null)
            {
                _resultString = "Forecast NDVI on " + _month + " " + forecastData.Year + " = " + forecastData.NDVI;
                OnPropertyChanged("ResultString");
            }

        }

        public bool CanMakeForecast(object parameter)
        {
            if (_fileNames != null && _fileNames.Count > 0)
                return true;
            return false;
        }



    }
}
