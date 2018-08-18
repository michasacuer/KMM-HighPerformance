using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Models;
using Microsoft.Win32;

namespace KMM_HighPerformance.ViewModels
{
    class ViewModel
    {
        public string filepath { get; set; }
        Bitmap binarizeLPImage { get; set; }
        Bitmap binarizeHPImage { get; set; }
        BitmapImage kMMLP { get; set; }
        BitmapImage kMMHP { get; set; }
        long timeElapsedLP { get; set; }
        long timeElapsedHP { get; set; }

        public ViewModel()
        {

            OpenFileDialog openPicture = new OpenFileDialog();
            openPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            openPicture.FilterIndex = 1;

            if (openPicture.ShowDialog() == true) 
            {
                filepath = openPicture.FileName;

                Bitmap bmp = new Bitmap(filepath);
                binarizeLPImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(filepath));
                binarizeHPImage = new Bitmap(filepath);
                kMMLP = BitmapConversion.Bitmap2BitmapImage(new Bitmap(filepath));

                var lowPerformanceTasks = Task.Factory.StartNew(() => binarizeLPImage = Binarization.LowPerformance(new Bitmap(filepath), binarizeLPImage))
                                                      .ContinueWith((prevTask) => kMMLP = KMMLowPerformance.Init(new Bitmap(filepath), binarizeLPImage))
                                                      .ContinueWith((prevTask) => timeElapsedLP = Binarization.TimeElapsed() + KMMLowPerformance.TimeElapsed());

                lowPerformanceTasks.Wait();

                var highPerformanceTasks = Task.Factory.StartNew(() => binarizeHPImage = Binarization.HighPerformance(new Bitmap(filepath)))
                                                       .ContinueWith((prevTask) => timeElapsedHP = Binarization.TimeElapsed());

                highPerformanceTasks.Wait();
            }

        }

        public string DisplayedImage
        {
            get
            {
                return filepath;
            }
        }

        public BitmapImage DisplayedBinarizeLPImage
        {
            get
            {
                return BitmapConversion.Bitmap2BitmapImage(binarizeLPImage);
            }
        }

        public BitmapImage DisplayedBinarizeHPImage
        {
            get
            {
                return BitmapConversion.Bitmap2BitmapImage(binarizeHPImage);
            }
        }


        public BitmapImage DisplayedLowPerformanceImage
        {
            get
            {
                return kMMLP;
            }
        }

        public long DisplayedLPTime
        {
            get
            {
                return timeElapsedLP;
            }
        }

        public long DisplayedHPTime
        {
            get
            {
                return timeElapsedHP;
            }
        }

    }
}
