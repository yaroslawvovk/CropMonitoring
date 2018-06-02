using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CropMonitoring.Helpers
{
    class OutputDirectoryCreator
    {
        private static string direcoryPath = @"D:\VHIData";
        public static string GetOutVHIPath(string fileName)
        {
            if (Directory.Exists(direcoryPath))
                return direcoryPath +"\\"+ fileName + ".dat";
            else
            {
                Directory.CreateDirectory(direcoryPath);
                return direcoryPath + "\\" + fileName + ".dat";
            }
        }
        public static string GetOutVHIPercentagePath(string fileName)
        {
            if (Directory.Exists(direcoryPath))
                return direcoryPath + "\\" + fileName + "Percentage.dat";
            else
            {
                Directory.CreateDirectory(direcoryPath);
                return direcoryPath + "\\" + fileName + "Percentage.dat";
            }
        }
    }
}
