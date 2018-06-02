using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CropMonitoring.Helpers;
using CropMonitoring.UserNotify;

namespace CropMonitoring.Downloaders
{
    class VHIDataLoader : Loader
    {
        public override string Url { get { return "https://www.star.nesdis.noaa.gov/smcd/emb/vci/VH/get_provinceData.php?country=UKR&provinceID="; } }
        private const string partTypeData = "&year1=1981&year2=2018&type=Mean";
        protected override Stream Download(string ProvinceId)
        {
            INotifyUser notify = new NotifyMessage();
            Stream stream = null;

            try
            {
                WebClient client = new WebClient();
                stream = client.OpenRead(Url + ProvinceId + partTypeData);
                client.Dispose();
            }
            catch (Exception e)
            {
                notify.Message("Stream didn't load!");
            }
            return stream;
        }

        public override Task DownloadAndSaveData(string FileName, string ProvinceId)
        {
            INotifyUser notify = new NotifyMessage();
            int count = 0;
            int scale = 36;
            ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
            string outputPath = OutputDirectoryCreator.GetOutVHIPath(FileName);
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    

                    Stream stream = Download(ProvinceId);
                    if (stream == null)
                        return;

                    StreamWriter sw = new StreamWriter(outputPath);

                    StreamReader streamReader = new StreamReader(stream);
                    string line;

                    streamReader.ReadLine();
                    sw.WriteLine(DateTime.Now);
                    while ((line = streamReader.ReadLine()) != null)
                    {

                        CheckStatus(scale, args, ref count);
                        sw.WriteLine(line);
                    }

                    //MessageBox.Show("Дані провінції " + FileName + " завантажено!");

                    streamReader.Close();
                    sw.Close();

                }

                catch (Exception r)
                {
                    notify.Message(r.Message);
                }
            }
            );
        }
    }
}
