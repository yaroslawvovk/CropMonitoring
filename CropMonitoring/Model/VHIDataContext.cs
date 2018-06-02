using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CropMonitoring.Helpers;
using CropMonitoring.UserNotify;

namespace CropMonitoring.Model
{
    static class VHIDataContext
    {
        private static string _dateLoad;
        private static ObservableCollection<VHIData> _vhiData;
        private static ObservableCollection<YearWeek> _yearWeek;

        public static string _DateLoad
        {
            get
            {
                return _dateLoad;
            }
        }
        public static ObservableCollection<VHIData> _VHIData
        {
            get
            {
                return _vhiData;
            }
        }
        public static ObservableCollection<YearWeek> _YearWeek
        {
            get
            {
                return _yearWeek;
            }
        }


        public static void ReadFromFile(string FileName)
        {
            INotifyUser notify = new NotifyMessage();
            _vhiData = new ObservableCollection<VHIData>();
            string[] parsedData;
            string line;
            string outputPath = OutputDirectoryCreator.GetOutVHIPath(FileName);

            if (File.Exists(outputPath))
            {
                StreamReader sr = new StreamReader(outputPath);



                try
                {                 
                    _dateLoad = sr.ReadLine();                 
                    while ((line = sr.ReadLine()) != null)
                    {

                        parsedData = line.Split(new Char[] { ',', ' ' },
                                         StringSplitOptions.RemoveEmptyEntries);

                        VHIData data = new VHIData();                      
                        data.Year = int.Parse(parsedData[0]);
                        data.Week = int.Parse(parsedData[1]);
                        data.SMN =  double.Parse(parsedData[2], CultureInfo.InvariantCulture);
                        data.SMT =  double.Parse(parsedData[3], CultureInfo.InvariantCulture);
                        data.VCI =  double.Parse(parsedData[4], CultureInfo.InvariantCulture);
                        data.TCI =  double.Parse(parsedData[5], CultureInfo.InvariantCulture);
                        data.VHI =  double.Parse(parsedData[6], CultureInfo.InvariantCulture);
                        _vhiData.Add(data);
                    }


                }
                catch (Exception e) {

                }

                sr.Close();               
            }

            else { notify.Message("File -  " + FileName + ".dat is missing!"); }

        }
        public static double SelectByYearWeek(int year,int week, string FileName)
        {
            string outputPath = OutputDirectoryCreator.GetOutVHIPath(FileName);
            double result = 0;
            string[] parsedData;
            string line;
            if (File.Exists(outputPath))
            {
                StreamReader sr = null;
                try
                {
                    sr = new StreamReader(outputPath);
                    _dateLoad = sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {
                        parsedData = line.Split(new Char[] { ',', ' ' },
                                         StringSplitOptions.RemoveEmptyEntries);
                        int Year = int.Parse(parsedData[0]);
                        int Week = int.Parse(parsedData[1]);
                        if(year==Year&&week==Week)
                        {
                            sr.Close();
                            return double.Parse(parsedData[6], CultureInfo.InvariantCulture);
                            
                        }
                    }

                }

                catch (Exception) { return 0; }

                sr.Close();
            }
            return result;

        }
        public static ObservableCollection<YearWeek> GetYearWeekList()
        {
            _yearWeek = new ObservableCollection<YearWeek>();
            string FileName = "Weeks";
            string[] parsedData;
            string line;
            if (File.Exists(FileName + ".dat"))
            {
                StreamReader sr = new StreamReader(FileName + ".dat");
                try
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        parsedData = line.Split(new Char[] { ',', ' ' },
                                         StringSplitOptions.RemoveEmptyEntries);

                        YearWeek yearw = new YearWeek();
                        yearw.year = int.Parse(parsedData[0]);
                        yearw.week = int.Parse(parsedData[1]);

                        _yearWeek.Add(yearw);
                    }
                }

                catch (Exception) { }

                sr.Close();
              
            }
            return _yearWeek;
        }
    }
}
