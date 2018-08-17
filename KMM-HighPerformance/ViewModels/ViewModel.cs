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
        public BitmapImage lowPerformance { get; set; }
        Bitmap newImage;

        public ViewModel()
        {

            OpenFileDialog openPicture = new OpenFileDialog();
            openPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            openPicture.FilterIndex = 1;

            if (openPicture.ShowDialog() == true) 
            {
                filepath = openPicture.FileName;
                newImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(filepath));

            }

        }

        public string DisplayedImage
        {
            get { return filepath; }
        }

        public BitmapImage DisplayedBinarizeLPImage
        {
            get { return BitmapConversion.Bitmap2BitmapImage(Binarization.LowPerformance(new Bitmap(filepath), newImage)); }
        }

        public BitmapImage DisplayedBinarizeHPImage
        {
            get { return BitmapConversion.Bitmap2BitmapImage(Binarization.HighPerformance(new Bitmap(filepath))); }
        }


        public BitmapImage DisplayedLowPerformanceImage
        {
            get { return KMMLowPerformance.Init(new Bitmap(filepath), newImage); }
        }

    }
}
