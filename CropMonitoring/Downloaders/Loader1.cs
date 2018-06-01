using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.Downloaders
{
    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Status { get; set; }
    }
    abstract class Loader1
    {
        public event EventHandler<ThresholdReachedEventArgs> statusEvent;       
        protected virtual void OnProgressStatus(ThresholdReachedEventArgs e)
        {
            statusEvent?.Invoke(this, e);
        }
        protected virtual void CheckStatus(int progressScale, ThresholdReachedEventArgs args, ref int progressCounter)
        {
            if (progressCounter == progressScale)
            {
                args.Status++;
                progressCounter = 0;
                OnProgressStatus(args);

            }
            progressCounter++;

        }
        public abstract string Url { get; }
        public abstract Stream Download(string ProvinceId);
        public abstract Task DownloadAndSaveData(string FileName, string ProvinceId);
    }
}
