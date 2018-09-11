using Microsoft.Win32;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

namespace KMM_HighPerformance.Models
{
    class Pictures
    {
        static public string GetNewImageFilepath()
        {
            string filepath = "";
            OpenFileDialog openPicture = new OpenFileDialog();
            openPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif";
            openPicture.FilterIndex = 1;
            openPicture.InitialDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\ExampleImages")); //note, that i can only use it in development!

            if (openPicture.ShowDialog() == true)
            {
                filepath = openPicture.FileName;
            }

            return filepath;
        }

        static public void SaveImageToFile(BitmapImage image)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PNG Image|*.png";
            saveFileDialog1.ShowDialog();

            if(saveFileDialog1.FileName != "")
            {
                try
                {
                    Bitmap bmp = BitmapConversion.BitmapImage2Bitmap(image);
                    Image toSave = bmp;
                    toSave.Save(saveFileDialog1.FileName, ImageFormat.Png);
                }

                catch(ArgumentNullException ex)
                {
                    System.Windows.MessageBox.Show("There is no final image to save");
                }
            }
        }

    }
}
