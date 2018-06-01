using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.SetelliteImageProcessor
{
    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Status { get; set; }
    }
    abstract class LandsatImageProcessor1
    {
        public event EventHandler<ThresholdReachedEventArgs> statusEvent;
        protected Driver drv;
        protected const int iOverview = 1;
        protected LandsatImageProcessor1()
        {
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();
            Gdal.AllRegister();
        }

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
        public void CalculateNDVI(string redSetPath, string nirSetPath, string outImage)
        {
            try
            {
                Dataset redDateset = Gdal.Open(redSetPath, Access.GA_ReadOnly);
                Dataset nirDataset = Gdal.Open(nirSetPath, Access.GA_ReadOnly);

                if (redDateset == null)
                {
                    System.Environment.Exit(-1);
                }


                drv = redDateset.GetDriver();

                if (drv == null)
                {
                    System.Environment.Exit(-1);
                }


                for (int iBand = 1; iBand <= redDateset.RasterCount; iBand++)
                {
                    Band band = redDateset.GetRasterBand(iBand);
                    Band nirBand = nirDataset.GetRasterBand(iBand);
                    for (int iOver = 0; iOver < band.GetOverviewCount(); iOver++)
                    {
                        Band over = band.GetOverview(iOver);
                        Band over1 = nirBand.GetOverview(iOver);
                    }
                }

                SaveBitmapBuffered(redDateset, nirDataset, outImage, iOverview);
            }
            catch (Exception e)
            {

            }

        }
        protected void SaveBitmapBuffered(Dataset redDateset, Dataset nirDataset, string filename, int iOverview)
        {
            Band redBand = redDateset.GetRasterBand(1);


            if (redBand.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
            {
                SaveBitmapGrayBuffered(redDateset, nirDataset, filename, iOverview);
                return;
            }
        }
        protected abstract void SaveBitmapGrayBuffered(Dataset ds, Dataset nirDataset, string filename, int iOverview);
        protected abstract Color SetColor(double ndvi);
        protected abstract string  GetOutImageName(string filename);

    }
}
