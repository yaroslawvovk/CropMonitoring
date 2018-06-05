using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CropMonitoring.Model;

namespace CropMonitoring.Forecast
{
    class ExponentialSmooth : NDVIForecast
    {
        protected double smooth;
        protected double expWeight;

        public override NDVIData GetForecastedData(List<Model.NDVIData> list)
        {
            if (list == null || list.Count == 0)
                return null;
            smooth = CalculateSmooth(list.Count);
            expWeight = CalculateExpWeight(list);
            list.OrderBy(x => x.Year);
            double a = 2.99 * 0.2 + (1.0 - 0.2) * 2.21;
            double forecastedNDVi = CalculateForecast(list);
            NDVIData forecastedData = new NDVIData { NDVI = forecastedNDVi, Year = list[list.Count - 1].Year+1 };
            return forecastedData;
        }
        private double CalculateForecast(List<Model.NDVIData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                expWeight = list[i].NDVI * smooth + (1.0 - smooth) * expWeight;
            }
            return expWeight;
        }

        private double CalculateSmooth(int n)
        {
            return (2.0 / (n + 1.0));
        }
        private double CalculateExpWeight(List<Model.NDVIData> list)
        {
            int count = list.Count;
            double sum = list.Sum(x=>x.NDVI);
            return sum / (double)count;
        }

    }
}
