using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.Forecast
{
   abstract class NDVIForecast
    {
       public abstract Model.NDVIData GetForecastedData(List<Model.NDVIData> list);

    }
}
