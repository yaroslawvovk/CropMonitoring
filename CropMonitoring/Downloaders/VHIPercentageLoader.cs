using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace CropMonitoring.Downloaders
{
    class VHIPercentageLoader: Loader
    {
        private ProgressBar pbar;
        private const string partTypeData = "&year1=1981&year2=2018&type=VHI_Parea";

        public VHIPercentageLoader(ProgressBar pbar)
        {
            this.pbar = pbar;
        }

        public override string Url { get { return "https://www.star.nesdis.noaa.gov/smcd/emb/vci/VH/get_provinceData.php?country=UKR&provinceID="; } }
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
            return Task.Factory.StartNew(() =>
            {
                StreamWriter sw = new StreamWriter(FileName + "Percentage.txt");

                Stream stream = Download(ProvinceId);

                try
                {
                    StreamReader streamReader = new StreamReader(stream);
                    string line;
                    streamReader.ReadLine();
                    sw.WriteLine(DateTime.Now);
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                        this.pbar.Dispatcher.Invoke(DispatcherPriority.Normal, (Action)delegate ()
                        {
                            count++;
                            if (count == 36)
                            {
                                if (pbar.Value < 100)
                                {
                                    pbar.Value++;
                                    count = 0;
                                }
                            }
                        });

                    }

                    MessageBox.Show("Data of region " + FileName + " has been downloaded!");

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
