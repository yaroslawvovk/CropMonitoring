using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.Model
{
    class VHIData
    {
        public int Year { get; set; }
        public int Week { get; set; }
        public double SMN { get; set; }
        public double SMT { get; set; }
        public double VCI { get; set; }
        public double TCI { get; set; }
        public double VHI { get; set; }
    }
}
