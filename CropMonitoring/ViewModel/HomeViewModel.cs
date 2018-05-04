using CropMonitoring.DataWorkers;
using CropMonitoring.Infrastructure;
using CropMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CropMonitoring.ViewModel
{
    class HomeViewModel:ViewModelBase
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
                if(year>=1981&&year<=2018)
                Extremums = _dataWorker.GetExtremums(year, VHIData);
            }
            catch(Exception)
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


    }
}
