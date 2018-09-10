using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Accord.Imaging.Filters;

namespace KMM_HighPerformance.Models
{
    class BitmapConversion
    {
        static public BitmapImage Bitmap2BitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                return bitmapImage;
            }
        }
 
        static public Bitmap CreateNonIndexedImage(Bitmap bitmap)
        {
            Bitmap newBmp = new Bitmap(bitmap.Width, bitmap.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(newBmp))
            {
                graphics.DrawImage(bitmap, 0, 0);
            }

            return newBmp;
        }

        static public Bitmap Create8bppGreyscaleImage(Bitmap bitmap) => Grayscale.CommonAlgorithms.BT709.Apply(bitmap);


    }
}
