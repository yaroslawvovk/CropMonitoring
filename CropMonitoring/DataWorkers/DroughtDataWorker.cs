using CropMonitoring.Helpers;
using CropMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.DataWorkers
{
   
    class DroughtDataWorker
    {
        private ObservableCollection<ComboboxData> _combdata;
        public ObservableCollection<ComboboxData> CombData
        {
            get
            {
                if (_combdata != null)
                    return _combdata;
                return new ObservableCollection<ComboboxData>();
            }
        }

        public ObservableCollection<ComboboxData> GetExtreamYears(ObservableCollection<VHIDataPercentage> percentages, ObservableCollection<VHIData> vhi, int percent)
        {

            _combdata = new ObservableCollection<ComboboxData>();
            double sum;
            try
            {
                for (int i = 0; i < percentages.Count; i++)
                {
                    sum = 0;
                    sum = percentages[i]._0 + percentages[i]._5 + percentages[i]._10;

                    if (sum > percent)
                    {
                        _combdata.Add(new ComboboxData() { Year = percentages[i].Year, Week = percentages[i].Week, VHI = vhi[i].VHI });
                    }
                }
            }

            catch (Exception ex)
            {  }

            return _combdata;

        }

        public ObservableCollection<ComboboxData> GetModerateYears(ObservableCollection<VHIDataPercentage> percentages, ObservableCollection<VHIData> vhi, int percent)
        {
            _combdata = new ObservableCollection<ComboboxData>();
            double sum;
            try
            {
                for (int i = 0; i < percentages.Count; i++)
                {
                    sum = 0;

                    sum = percentages[i]._0 + percentages[i]._5 + percentages[i]._10 + percentages[i]._15 + percentages[i]._20 + percentages[i]._25 + percentages[i]._30;

                    if (sum > percent)
                    {
                        _combdata.Add(new ComboboxData() { Year = percentages[i].Year, Week = percentages[i].Week, VHI = vhi[i].VHI });
                    }
                }
            }

            catch (Exception ex)
            { }

            return _combdata;
        }
    }
}
