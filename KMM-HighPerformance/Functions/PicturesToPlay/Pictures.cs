using Microsoft.Win32;
using System;
using System.Windows;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using KMM_HighPerformance.Functions.Conversions;

namespace KMM_HighPerformance.Functions.PicturesToPlay
{
    static class Pictures
    {
        static public string GetNewImageFilepath()
        {
            OpenFileDialog openPicture = new OpenFileDialog()
            {
                Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif",
                FilterIndex = 1,
                //Debug Only
                InitialDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\ExampleImages"))
            };

            return openPicture.ShowDialog() == true ? openPicture.FileName : String.Empty;
        }

        static public void SaveImageToFile(BitmapImage image)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = "PNG Image|*.png",
            };

            if(saveFileDialog.ShowDialog() == true && saveFileDialog.FileName != String.Empty)
            {
                try
                {
                    Bitmap bmp = BitmapConversion.BitmapImage2Bitmap(image);
                    Image toSave = bmp;
                    toSave.Save(saveFileDialog.FileName, ImageFormat.Png);
                }

                catch(ArgumentNullException ex)
                {
                    MessageBox.Show("There is no final image to save");
                }
            }
        }

    }
}
