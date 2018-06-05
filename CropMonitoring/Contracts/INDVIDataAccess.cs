using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CropMonitoring.Model;

namespace CropMonitoring.Contracts
{
    interface INDVIDataAccess
    {
        List<string> GetNDVIFilesNames(int month);
        List<NDVIData> GetNDVIData(List<string> fileNames);
        void CreateNDVIFile(string filePath,double ndvi);
        bool DeleteNDVIFile(string fileName);
    }
}
