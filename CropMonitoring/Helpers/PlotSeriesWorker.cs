using CropMonitoring.Model;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.Helpers
{
    class DataSeries
    {
        public double Rx { set; get; }
        public double Ry { set; get; }
    }

    class PlotSeriesWorker
    {
        
        public IEnumerable<DataSeries> GetSeries(List<VHIData> list,int year)
        {
            return list.Where(x => x.Year == year)
                .Select(x => new DataSeries { Rx = x.Week, Ry = x.VHI });
        }

    }
}
