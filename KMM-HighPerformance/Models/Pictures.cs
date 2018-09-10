using Microsoft.Win32;
using System;
using System.IO;

namespace KMM_HighPerformance.Models
{
    class Pictures
    {
        static public string GetNewImage()
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

    }
}
