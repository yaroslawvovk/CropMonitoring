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
    static class VHIDataPercetageContext
    {
        private static string _dateLoad;
        private static ObservableCollection<VHIDataPercentage> _vhiDataPercentage;

        public static string _DateLoad
        {
            get
            {
                return _dateLoad;
            }
        }
        public static ObservableCollection<VHIDataPercentage> _VHIDataPercentage
        {
            get
            {
                return _vhiDataPercentage;
            }
        }

        public static void ReadFromFile(string FileName)
        {
            _vhiDataPercentage = new ObservableCollection<VHIDataPercentage>();
            string[] parsedData;
            string line;
            if (File.Exists(FileName + "Percentage.dat"))
            {
                StreamReader sr = new StreamReader(FileName + "Percentage.dat");

                try
                {
                    _dateLoad = sr.ReadLine();
                    while ((line = sr.ReadLine()) != null)
                    {

                        parsedData = line.Split(new Char[] { ',', ' ' },
                                         StringSplitOptions.RemoveEmptyEntries);

                        VHIDataPercentage data = new VHIDataPercentage();
                        data.Year = int.Parse(parsedData[0]);
                        data.Week = int.Parse(parsedData[1]);
                        data._0 = double.Parse(parsedData[2], CultureInfo.InvariantCulture);
                        data._5 = double.Parse(parsedData[3], CultureInfo.InvariantCulture);
                        data._10 = double.Parse(parsedData[4], CultureInfo.InvariantCulture);
                        data._15 = double.Parse(parsedData[5], CultureInfo.InvariantCulture);
                        data._20 = double.Parse(parsedData[6], CultureInfo.InvariantCulture);
                        data._25 = double.Parse(parsedData[7], CultureInfo.InvariantCulture);
                        data._30 = double.Parse(parsedData[8], CultureInfo.InvariantCulture);
                        data._35 = double.Parse(parsedData[9], CultureInfo.InvariantCulture);
                        data._40 = double.Parse(parsedData[10], CultureInfo.InvariantCulture);
                        data._45 = double.Parse(parsedData[11], CultureInfo.InvariantCulture);
                        data._50 = double.Parse(parsedData[12], CultureInfo.InvariantCulture);
                        data._55 = double.Parse(parsedData[13], CultureInfo.InvariantCulture);
                        data._60 = double.Parse(parsedData[14], CultureInfo.InvariantCulture);
                        data._65 = double.Parse(parsedData[15], CultureInfo.InvariantCulture);
                        data._70 = double.Parse(parsedData[16], CultureInfo.InvariantCulture);
                        data._75 = double.Parse(parsedData[17], CultureInfo.InvariantCulture);
                        data._80 = double.Parse(parsedData[18], CultureInfo.InvariantCulture);
                        data._85 = double.Parse(parsedData[19], CultureInfo.InvariantCulture);
                        data._90 = double.Parse(parsedData[20], CultureInfo.InvariantCulture);
                        data._95 = double.Parse(parsedData[21], CultureInfo.InvariantCulture);
                        data._100 = double.Parse(parsedData[22], CultureInfo.InvariantCulture);
                        _vhiDataPercentage.Add(data);
                    }


                }

                catch (Exception) { }

                sr.Close();
                MessageBox.Show("Дані провінції " + FileName + " записано в таблицю!");
            }

            else { MessageBox.Show("Файл з іменем " + FileName + ".dat відсутній!"); }

        }
    }
}
