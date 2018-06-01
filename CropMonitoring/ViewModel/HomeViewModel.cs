using CropMonitoring.DataWorkers;
using CropMonitoring.Helpers;
using CropMonitoring.Infrastructure;
using CropMonitoring.Model;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CropMonitoring.ViewModel
{
    public static class Extensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
        {
            return new ObservableCollection<T>(col);
        }
    }

    class HomeViewModel : ViewModelBase
    {
        ObservableCollection<VHIData> _vhiData;
        public ObservableCollection<VHIData> VHIData
        {
            get
            {
                if (_vhiData == null)
                    return new ObservableCollection<Model.VHIData>();
                return _vhiData;
            }
            set
            {
                _vhiData = value;
            }
        }
        RelayCommand _getVHIDataCommand;
        public ICommand GetVHIData
        {
            get
            {
                if (_getVHIDataCommand == null)
                    _getVHIDataCommand = new RelayCommand(ExecuteGetVHIDataCommand, CanExecuteGetVHIDataCommand);
                return _getVHIDataCommand;
            }
        }
        public void ExecuteGetVHIDataCommand(object parameter)
        {
            VHIDataContext.ReadFromFile((string)parameter);
            VHIData = VHIDataContext._VHIData;
            OnPropertyChanged("VHIData");
            VHIDataPercetageContext.ReadFromFile((string)parameter);
            VHIDataPercentage = VHIDataPercetageContext._VHIDataPercentage;
            OnPropertyChanged("VHIDataPercentage");

        }

        public bool CanExecuteGetVHIDataCommand(object parameter)
        {
            if (string.IsNullOrEmpty((string)parameter))
                return false;
            return true;
        }

        protected override void OnDispose()
        {
            this.VHIData.Clear();
        }

        ObservableCollection<VHIDataPercentage> _vhiDataPercentage;
        public ObservableCollection<VHIDataPercentage> VHIDataPercentage
        {
            get
            {
                if (_vhiDataPercentage == null)
                    return new ObservableCollection<Model.VHIDataPercentage>();
                return _vhiDataPercentage;
            }
            set
            {
                _vhiDataPercentage = value;
            }
        }

        Extremums _extremums;
        public Extremums Extremums
        {
            get
            {
                if (_extremums == null)
                    return new Extremums() { _maxExt = 0, _minExt = 0 };
                return _extremums;

            }
            set
            {
                _extremums = value;
            }

        }

        RelayCommand _getExtremumCommand;
        public ICommand GetExtremum
        {
            get
            {
                if (_getExtremumCommand == null)
                    _getExtremumCommand = new RelayCommand(ExecuteGetExtremumCommand, CanExecuteGetExtremumCommand);
                return _getExtremumCommand;
            }
        }
        public void ExecuteGetExtremumCommand(object parameter)
        {
            VHIDataWorker _dataWorker = new VHIDataWorker();
            if (VHIData.Count == 0)
                return;

            try
            {
                int year = Convert.ToInt32(parameter);
                if (year >= 1981 && year <= 2018)
                    Extremums = _dataWorker.GetExtremums(year, VHIData);
            }
            catch (Exception)
            {

            }


            OnPropertyChanged("Extremums");
        }
        public bool CanExecuteGetExtremumCommand(object parameter)
        {
            if (string.IsNullOrEmpty((string)parameter))
                return false;
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

        RelayCommand _makePlot;
        public ICommand MakePlot
        {
            get
            {
                if (_makePlot == null)
                    _makePlot = new RelayCommand(ExecuteGetPlotSeriesCommand, CanExecuteGetPlotSeriesCommand);
                return _makePlot;
            }
        }

        public void ExecuteGetPlotSeriesCommand(object parameter)
        {
            PlotSeriesWorker seriesWorker = new PlotSeriesWorker();

            if (VHIData == null)
                return;

            try
            {
                int year = Convert.ToInt32(parameter);
                List<VHIData> vhiList = VHIData.ToList<VHIData>();
                DataSeries = seriesWorker.GetSeries(vhiList, year).Select(x => new DataPoint(x.Rx, x.Ry)).ToArray();
            }

            catch (Exception)
            {

            }
            OnPropertyChanged("DataSeries");

        }

        public bool CanExecuteGetPlotSeriesCommand(object parameter)
        {
            if (string.IsNullOrEmpty((string)parameter))
                return false;
            return true;
        }

        DataPoint[] _dataSeries2;
        public DataPoint[] DataSeries2
        {
            get
            {
                return _dataSeries2;
            }
            set
            {
                _dataSeries2 = value;
            }
        }

        RelayCommand _makePlot2;
        public ICommand MakePlot2
        {
            get
            {
                if (_makePlot2 == null)
                    _makePlot2 = new RelayCommand(ExecuteGetPlotSeries2Command, CanExecuteGetPlotSeries2Command);
                return _makePlot2;
            }
        }

        public void ExecuteGetPlotSeries2Command(object parameter)
        {
            PlotSeriesWorker seriesWorker = new PlotSeriesWorker();

            if (VHIData == null)
                return;

            try
            {
                int year = Convert.ToInt32(parameter);
                List<VHIData> vhiList = VHIData.ToList<VHIData>();
                DataSeries2 = seriesWorker.GetSeries(vhiList, year).Select(x => new DataPoint(x.Rx, x.Ry)).ToArray();
            }

            catch (Exception)
            {

            }
            OnPropertyChanged("DataSeries2");

        }

        public bool CanExecuteGetPlotSeries2Command(object parameter)
        {
            if (string.IsNullOrEmpty((string)parameter))
                return false;
            return true;
        }


        RelayCommand _clearPlot;
        public ICommand ClearPlot
        {
            get
            {
                if (_clearPlot == null)
                    _clearPlot = new RelayCommand(ExecuteClearCommand, CanClearCommand);
                return _clearPlot;
            }
        }

        public void ExecuteClearCommand(object parameter)
        {
            DataSeries = null;
            DataSeries2 = null;

            OnPropertyChanged("DataSeries");
            OnPropertyChanged("DataSeries2");
        }

        public bool CanClearCommand(object parameter)
        {
            return true;
        }

        ObservableCollection<ComboboxData> _combData;
        public ObservableCollection<ComboboxData> CombData
        {
            get
            {
                if (_combData == null)
                    return new ObservableCollection<ComboboxData>();
                return _combData;
            }
            set
            {
                _combData = value;
            }
        }

        RelayCommand _getDroughtYear;
        public ICommand GetDroughtYear
        {
            get
            {
                if (_getDroughtYear == null)
                    _getDroughtYear = new RelayCommand(ExecuteGetDroughtYear, CanGetDroughtYear);
                return _getDroughtYear;
            }
        }

        public void ExecuteGetDroughtYear(object parameter)
        {
            var param = (Tuple<string, bool>)parameter;
            int percent = 0;
            bool isModerate = false;
            if (Int32.TryParse(param.Item1, out percent))
            {
                DroughtDataWorker dWorker = new DroughtDataWorker();
                isModerate = param.Item2;
                CombData = !isModerate ? dWorker.GetExtreamYears(VHIDataPercentage, VHIData, percent) : dWorker.GetModerateYears(VHIDataPercentage, VHIData, percent);
                OnPropertyChanged("CombData");
            }
        }
        public bool CanGetDroughtYear(object parameter)
        {
            return true;
        }

        object _selectedComBoxValue;
        public object SelectedComBoxValue
        {
            get
            {
                if (_selectedComBoxValue != null)
                    return _selectedComBoxValue;
                return null;
            }
            set
            {
                _selectedComBoxValue = value;
            }
        }

        string _selectedComBoxText;
        public string SelectedComBoxText
        {
            get
            {
                if (_selectedComBoxText != null)
                    return _selectedComBoxText;
                return null;
            }
            set
            {
                _selectedComBoxText = value;
            }
        }

        RelayCommand _downloadData;
        public ICommand DownloadData
        {
           get
            {
                if (_downloadData == null)
                    return new RelayCommand(DownloadDataAsync, CanDownloadDataAsync);
                return _downloadData;
            }
        }

        public async void DownloadDataAsync(object parameter)
        {
            await load.DownloadAndSaveData(_value, _key);
            await load2.DownloadAndSaveData(_value, _key);
        }
        public bool CanDownloadDataAsync(object parameter)
        {
            if (_selectedComBoxValue != null && _selectedComBoxText != null)
                return true;
            return false;
        }




    }
}
