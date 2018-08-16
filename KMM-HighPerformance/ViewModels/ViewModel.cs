using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMM_HighPerformance.Models;
using Microsoft.Win32;

namespace KMM_HighPerformance.ViewModels
{
    class ViewModel
    {
        public string filepath { get; set; }
        public Bitmap lowPerformanceBitmap { get; set; }
        public Bitmap highPerformanceBitmap { get; set; }

        public ViewModel()
        {

            OpenFileDialog openPicture = new OpenFileDialog();
            openPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            openPicture.FilterIndex = 1;

            if (openPicture.ShowDialog() == true) 
            {
                filepath = openPicture.FileName;
                lowPerformanceBitmap = new Bitmap(filepath);
                highPerformanceBitmap = new Bitmap(filepath);
            }



        }

        public string DisplayedImage
        {

            get { return filepath; }

        }


    }
}
