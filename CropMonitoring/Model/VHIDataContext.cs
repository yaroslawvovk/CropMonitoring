using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CropMonitoring.Model
{
    static class VHIDataContext
    {
        private static string _dateLoad;
        private static ObservableCollection<VHIData> _vhiData;

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

        
        public static void ReadFromFile(string FileName)
        {
            _vhiData = new ObservableCollection<VHIData>();
            string[] parsedData;
            string line;
            if (File.Exists(FileName + ".txt"))
            {
                StreamReader sr = new StreamReader(FileName + ".txt");

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

                catch (Exception) { }

                sr.Close();
                MessageBox.Show("Дані провінції " + FileName + " записано в таблицю!");
            }

            else { MessageBox.Show("Файл з іменем " + FileName + ".txt відсутній!"); }

        }
        public static double SelectByYearWeek(int year,int week, string FileName)
        {
            double result = 0;
            string[] parsedData;
            string line;
            if (File.Exists(FileName + ".txt"))
            {
                StreamReader sr = new StreamReader(FileName + ".txt");

                try
                {
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

                catch (Exception) { }

                sr.Close();
            }
            return result;

        }
    }
}
