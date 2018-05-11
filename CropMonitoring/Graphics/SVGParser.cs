using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Svg;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace CropMonitoring.Graphics
{
    class SVGParser
    {

        public static Size MaximumSize { get; set; }

        public static Bitmap GetBitmapFromSVG(string filePath)
        {
            SvgDocument document = GetSvgDocument(filePath);

            Bitmap bmp = document.Draw();
            return bmp;
        }

        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public static SvgDocument GetSvgDocument(string filePath)
        {
            MaximumSize = new Size(1000, 1000);
            SvgDocument document = SvgDocument.Open(filePath);
            return AdjustSize(document);
        }


        private static SvgDocument AdjustSize(SvgDocument document)
        {
            if (document.Height > MaximumSize.Height)
            {
                document.Width = (int)((document.Width / (double)document.Height) * MaximumSize.Height);
                document.Height = MaximumSize.Height;
            }
            return document;
        }

    }
}
