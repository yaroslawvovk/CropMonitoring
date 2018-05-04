using CropMonitoring.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.DataWorkers
{
    class VHIDataWorker
    {

        public Extremums GetExtremums(int year, ObservableCollection<VHIData> VHIData)
        {
            Extremums _extremums = new Extremums() { _maxExt = 0, _minExt = 0 };
            System.Collections.Generic.List<double> list = new List<double>();

            if (VHIData.Count != 0)
            {
                for (int i = 0; i < VHIData.Count; i++)
                {
                    if (VHIData[i].Year == year)
                    {
                        list.Add(VHIData[i].VHI);

                    }

                }
            }
            if (list.Count != 0)
            {
                _extremums ._maxExt= list.Max<double>();
                _extremums._minExt = list.Min<double>();
            }
            return _extremums;
        }
    }
}
