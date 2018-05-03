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


    }
}
