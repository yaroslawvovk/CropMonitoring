using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CropMonitoring.Model
{
    class SetelliteImageAccess: IDisposable
    {
        private static List<string> files;
        private const string folderPath = "D:\\SetelliteImages";
        private const string folderOutPath = "D:\\OutputSetelliteImages";

        public  List<string> GetInputImageFiles()
        {
            if (Directory.Exists(folderPath))
            {
                files = new DirectoryInfo(folderPath).GetFiles().Select(x => x.Name).ToList<string>();
                if (files.Count == 0)
                    return null;
            }
            else
            {
                Directory.CreateDirectory(folderPath);
                files = new DirectoryInfo(folderPath).GetFiles().Select(x => x.Name).ToList<string>();
                if (files.Count == 0)
                    return null;
            }
            return files;
        }
        public  List<string> GetOutputImageFiles()
        {
            if (Directory.Exists(folderOutPath))
            {
                files = new DirectoryInfo(folderOutPath).GetFiles().Select(x => x.Name).ToList<string>();
                if (files.Count == 0)
                    return null;
            }
            else
            {
                Directory.CreateDirectory(folderOutPath);
                files = new DirectoryInfo(folderOutPath).GetFiles().Select(x => x.Name).ToList<string>();
                if (files.Count == 0)
                    return null;
            }
            return files;
        }

        public string GetImageFile(string fileName)
        {
            string concpath = folderPath + "\\" + fileName;
            if (File.Exists(concpath))
            {
                return concpath;
            }
            return null;
        }
        public BitmapImage GetBitmapImage(string fileName)
        {          
            string concpath = folderPath + "\\" + fileName;
            try
            {
                if (File.Exists(concpath))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(concpath);
                    image.EndInit();
                    return image;
                }
            }
            catch (Exception e)
            {

            }
            return null;
        }
        
        public string GetNDVIImage(string fileName)
        {
            string concpath = folderOutPath + "\\" + fileName;
            if (File.Exists(concpath))
            {
                return concpath;
            }
            return null;
        }

        public BitmapImage GetBitmapNDVIImage(string fileName)
        {
            string concpath = folderOutPath + "\\" + fileName;
            try
            {
                if (File.Exists(concpath))
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(concpath);
                    image.EndInit();
                    return image;
                }
            }
            catch (Exception e)
            {

            }
            
            return null;
        }


        public bool DeleteImage(string fileName)
        {
            string concpath = folderOutPath + "\\" + fileName;
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


        public (string band3, string band4, string outImg) GetBandsPath(string bandPath)
        {
            string[] parts = bandPath.Split('_');
            string band = parts[parts.Length - 1];
            string b3 = null;
            string b4 = null;
            string _out = null;
            if (parts.Length != 8)
                return (b3, b4, _out);

            if (band.ToUpper() == "B3.TIF")
            {
                b3 = GetImageFile(bandPath);
                b4 = GetImageFile(bandPath.Replace("B3.TIF", "B4.TIF"));

                if (!Directory.Exists(folderOutPath))
                    Directory.CreateDirectory(folderOutPath);
                _out = folderOutPath + "\\" + parts[3] + "NDVI.TIF";

                var tuple = (band3: b3, band4: b4, outImg: _out);
                return tuple;
            }
            else if (band.ToUpper() == "B4.TIF")
            {
                b4 = GetImageFile(bandPath);
                b3 = GetImageFile(bandPath.Replace("B4.TIF", "B3.TIF"));
                if (!Directory.Exists(folderOutPath))
                    Directory.CreateDirectory(folderOutPath);
                _out = folderOutPath + "\\" + parts[3] + "NDVI.TIF";

                var tuple = (band3: b3, band4: b4, outImg: _out);
                return tuple;
            }

            return (b3, b4, _out);
        }

        public void Dispose()
        {
            this.Dispose();
        }
    }
}
