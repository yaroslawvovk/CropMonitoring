using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.Downloaders
{
    abstract class Loader
    {
        public abstract string Url { get; }
        

        public abstract Stream Download(string ProvinceId);
        public abstract Task DownloadAndSaveData(string FileName, string ProvinceId);

    }
}
