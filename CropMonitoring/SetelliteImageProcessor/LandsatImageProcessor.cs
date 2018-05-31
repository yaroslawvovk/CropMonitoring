using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropMonitoring.SetelliteImageProcessor
{
    public class ThresholdReachedEventArgs : EventArgs
    {
        public int Status { get; set; }
    }

    class LandsatImageProcessor
    {
        public event EventHandler<ThresholdReachedEventArgs> statusEvent;

        private Driver drv;

        private const int iOverview = 1;

        public LandsatImageProcessor()
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

        private void SaveBitmapBuffered(Dataset redDateset, Dataset nirDataset, string filename, int iOverview)
        {
            Band redBand = redDateset.GetRasterBand(1);


            if (redBand.GetRasterColorInterpretation() == ColorInterp.GCI_GrayIndex)
            {
                SaveBitmapGrayBuffered(redDateset, nirDataset, filename, iOverview);
                return;
            }
        }
        private void SaveBitmapGrayBuffered(Dataset ds, Dataset nirDataset, string filename, int iOverview)
        {
            ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
            Band band = ds.GetRasterBand(1);
            Band nirBand = nirDataset.GetRasterBand(1);
            if (iOverview >= 0 && band.GetOverviewCount() > iOverview)
                band = band.GetOverview(iOverview);


            int width = band.XSize;
            int height = band.YSize;


            Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);


            byte[] r = new byte[width * height];
            byte[] nir = new byte[width * height];
            byte[] r1 = new byte[width * height];
            byte[] b1 = new byte[width * height];
            byte[] g1 = new byte[width * height];
            band.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
            nirBand.ReadRaster(0, 0, width, height, nir, width, height, 0, 0);

            int i, j;
            double ndvi = 0;     
            int progressScale = width / 100;
            int progressCounter = 0;
            try
            {
                for (i = 0; i < width; i++)
                {
                    CheckStatus(progressScale, args, ref progressCounter);
                    for (j = 0; j < height; j++)
                    {
                        double low = nir[i + j * width] + r[i + j * width];
                        double up = nir[i + j * width] - r[i + j * width];

                        if (low == 0)
                            ndvi = 0;
                        else
                        {
                            ndvi = up / low;
                        }
                        Color color = SetColor(ndvi);
                        r1[i + j * width] = color.R;
                        b1[i + j * width] = color.B;
                        g1[i + j + width] = color.G;
                        //bitmap.SetPixel(i, j, SetColor(ndvi));
                        //bitmap.SetPixel(i, j, SetColor2(ndvi));
                    }
                }

                Dataset outRaster = Gdal.GetDriverByName("GTiff").Create(filename, width, height, 3, DataType.GDT_Byte, null);
                double[] geoTransformerData = new double[6];
                ds.GetGeoTransform(geoTransformerData);
                outRaster.SetGeoTransform(geoTransformerData);
                outRaster.SetProjection(ds.GetProjection());

                outRaster.GetRasterBand(1).WriteRaster(0, 0, width, height, r1, width, height, 0, 0);
                outRaster.GetRasterBand(2).WriteRaster(0, 0, width, height, b1, width, height, 0, 0);
                outRaster.GetRasterBand(3).WriteRaster(0, 0, width, height, g1, width, height, 0, 0);
                outRaster.FlushCache();
            }
            catch (Exception e)
            {

            }           
            bitmap.Save(filename);
        }

        public static Color SetColor(double ndvi)
        {
            if (ndvi <= -0.2)
            {
                return Color.DarkCyan;
            }
            else if (ndvi == 0)
            {
                return Color.Black;
            }
            else if (ndvi > -0.2 && ndvi <= -0.1)
            {
                return Color.White;
            }
            else if (ndvi > -0.1 && ndvi <= 0)
            {
                return Color.Gray;
            }
            else if (ndvi > 0 && ndvi <= 0.1)
            {
                return Color.DarkBlue;
            }
            else if (ndvi > 0.1 && ndvi <= 0.2)
            {
                return Color.Blue;
            }
            else if (ndvi > 0.2 && ndvi <= 0.3)
            {
                return Color.DarkGreen;
            }
            else if (ndvi > 0.3 && ndvi <= 0.4)
            {
                return Color.Green;
            }
            else if (ndvi > 0.4 && ndvi <= 0.5)
            {
                return Color.LightGreen;
            }
            else if (ndvi > 0.5 && ndvi <= 0.6)
            {
                return Color.Yellow;
            }
            else if (ndvi > 0.6 && ndvi <= 0.7)
            {
                return Color.LightYellow;
            }
            else if (ndvi > 0.7 && ndvi <= 0.8)
            {
                return Color.Red;
            }
            else if (ndvi > 0.8 && ndvi <= 0.9)
            {
                return Color.OrangeRed;
            }
            else if (ndvi > 0.9 && ndvi <= 1)
            {
                return Color.OrangeRed;
            }
            return Color.Black;
        }
        public static Color SetColor2(double ndvi)
        {
            if (ndvi <= -0.2)
            {
                return Color.DarkCyan;
            }
            else if (ndvi == 0)
            {
                return Color.Black;
            }
            else if (ndvi > -0.2 && ndvi <= -0.1)
            {
                return Color.White;
            }
            else if (ndvi > -0.1 && ndvi <= 0)
            {
                return Color.Gray;
            }
            else if (ndvi > 0 && ndvi <= 0.1)
            {
                return Color.DarkRed;
            }
            else if (ndvi > 0.1 && ndvi <= 0.2)
            {
                return Color.LightYellow;
            }
            else if (ndvi > 0.2 && ndvi <= 0.3)
            {
                return Color.Yellow;
            }
            else if (ndvi > 0.3 && ndvi <= 0.4)
            {
                return Color.LightGoldenrodYellow;
            }
            else if (ndvi > 0.4 && ndvi <= 0.5)
            {
                return Color.Cyan;
            }
            else if (ndvi > 0.5 && ndvi <= 0.6)
            {
                return Color.Blue;
            }
            else if (ndvi > 0.6 && ndvi <= 0.7)
            {
                return Color.DarkBlue;
            }
            else if (ndvi > 0.7 && ndvi <= 0.8)
            {
                return Color.LightGreen;
            }
            else if (ndvi > 0.8 && ndvi <= 0.9)
            {
                return Color.Green;
            }
            else if (ndvi > 0.9 && ndvi <= 1)
            {
                return Color.DarkGreen;
            }
            return Color.Green;
        }
    }
}
