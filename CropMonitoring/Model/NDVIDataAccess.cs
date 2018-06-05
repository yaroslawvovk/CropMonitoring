using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CropMonitoring.Contracts;
using System.IO;
using System.Globalization;

namespace CropMonitoring.Model
{
    class NDVIDataAccess : INDVIDataAccess
    {
        private const string folderPath = "D:\\NDVIData";
        private const string outFormat = ".dat";

        public void CreateNDVIFile(string filePath,double ndvi)
        {
            string fileName = Path.GetFileName(filePath);
            string name = fileName.Split('.')[0];
            fileName = name + outFormat;
            string outPath = Path.Combine(folderPath, fileName);
            WriteNDVIData(ndvi, outPath);

        }

        public bool DeleteNDVIFile(string fileName)
        {
            string concpath = Path.Combine(folderPath, fileName);
            if (File.Exists(concpath))
            {
                try
                {
                    File.Delete(concpath);
                    return true;
                }
                catch (Exception e)
                {

                }
            }
            return false;
        }

        public List<NDVIData> GetNDVIData(List<string> fileNames)
        {
            List<NDVIData> ndviList = new List<NDVIData>();
            CropMonitoring.UserNotify.NotifyMessage notift = new UserNotify.NotifyMessage();
            try
            {
                if (!Directory.Exists(folderPath))
                    throw new DirectoryNotFoundException("Target directory is missing");
                if (fileNames.Count < 5)
                    throw new Exception("File count is less than 5 ");

                foreach (string fileName in fileNames)
                {
                    string filePath = Path.Combine(folderPath, fileName);

                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        double ndvi = double.Parse(sr.ReadLine(), CultureInfo.InstalledUICulture);
                        string fileName1 = Path.GetFileName(filePath);
                        int year = int.Parse(fileName1.Substring(3, 4));
                        ndviList.Add(new NDVIData() { NDVI = ndvi, Year = year });
                    }

                }
    
            }
            catch (Exception e)
            {
                notift.Message(e.Message);
            }
            return ndviList;
        }

        public List<string> GetNDVIFilesNames(int month)
        {
            List<int> years = new List<int>();
            List<string> fileNames = new List<string>();
            string[] allFiles =  Directory.GetFiles(folderPath);
            try
            {
                foreach (string path in allFiles)
                {
                    string fileName = Path.GetFileName(path);
                    int year = int.Parse(fileName.Substring(3, 4));
                    int month1 = int.Parse(fileName.Substring(7, 2));
                    if (!years.Contains(year) && month1 == month)
                    {
                        years.Add(year);
                        fileNames.Add(fileName);
                    }
                }
            }
            catch (Exception e) { }
            return fileNames;
        }

        private void WriteNDVIData(double ndvi,string outPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (StreamWriter sw = new StreamWriter(outPath))
            {
                sw.WriteLine(ndvi);
            }

        }

      
    }
}
