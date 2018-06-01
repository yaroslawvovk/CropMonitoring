using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CropMonitoring.Downloaders
{
    class VHIDataLoader1 : Loader1
    {
        public override string Url { get { return "https://www.star.nesdis.noaa.gov/smcd/emb/vci/VH/get_provinceData.php?country=UKR&provinceID="; } }
        private const string partTypeData = "&year1=1981&year2=2018&type=Mean";
        public override Stream Download(string ProvinceId)
        {
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(Url + ProvinceId + partTypeData);
            client.Dispose();
            return stream;
        }

        public override Task DownloadAndSaveData(string FileName, string ProvinceId)
        {
            int count = 0;
            int scale = 36;
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    StreamWriter sw = new StreamWriter(FileName + ".dat");

                    Stream stream = Download(ProvinceId);


                    StreamReader streamReader = new StreamReader(stream);
                    string line;
                   
                    streamReader.ReadLine();
                    sw.WriteLine(DateTime.Now);
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        CheckStatus(scale)
                        sw.WriteLine(line);
                    }

                    //MessageBox.Show("Дані провінції " + FileName + " завантажено!");

                    streamReader.Close();
                    sw.Close();

                }

                catch (Exception r)
                {
                    MessageBox.Show(r.Message);
                }
            }
            );
        }
    }
}
