using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Models;
using Microsoft.Win32;

namespace KMM_HighPerformance.ViewModels
{
    class ViewModel
    {
        public string filepath { get; set; }
        public string filepathLowPerformance { get; set; }
        public string filepathHighPerformance { get; set; }
        public BitmapImage lowPerformance { get; set; }

        public ViewModel()
        {

            OpenFileDialog openPicture = new OpenFileDialog();
            openPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            openPicture.FilterIndex = 1;

            if (openPicture.ShowDialog() == true) 
            {
                filepath = openPicture.FileName;

                filepathLowPerformance = filepath;

                //lowPerformance = KMMLowPerformance.Init(new Bitmap(filepathLowPerformance));
                //highPerformanceBitmap = new Bitmap(filepath);
            }

        }

        public string DisplayedImage
        {
            get { return filepath; }
        }

        public BitmapImage DisplayedLowPerformanceImage
        {
            get { return KMMLowPerformance.Init(new Bitmap(filepath)); }
        }

    }
}
