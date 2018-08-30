using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Models;
using Microsoft.Win32;

namespace KMM_HighPerformance.ViewModels 
{
    class WindowViewModel : INotifyPropertyChanged
    {
        public WindowViewModel()
        {
            OpenFileDialog openPicture = new OpenFileDialog();
            openPicture.Filter = "Image files|*.bmp;*.jpg;*.gif;*.png;*.tif|All files|*.*";
            openPicture.FilterIndex = 1;

            if (openPicture.ShowDialog() == true) 
            {
                filepath = openPicture.FileName;

                Bitmap bmp = new Bitmap(filepath);
                binarizeLPImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(filepath));
                kMMLP = BitmapConversion.Bitmap2BitmapImage(new Bitmap(filepath));
            }

            async Task InitializeLP()
            {
                Measure measureLP = new Measure();

                binarizeLPImage = await Task.Run(() => Binarization.LowPerformance(new Bitmap(filepath), binarizeLPImage, measureLP));
                binarizeLPImageView = BitmapConversion.Bitmap2BitmapImage(binarizeLPImage);

                kMMLP = await Task.Run(() => KMMLowPerformance.Init(new Bitmap(filepath), binarizeLPImage, measureLP));

                timeElapsedLP = measureLP.TimeElapsed() + measureLP.TimeElapsed();                
            }

            async Task InitializeHP()
            {
                Measure measureHP = new Measure();

                binarizeHPImage = await Task.Run(() => Binarization.HighPerformance(new Bitmap(filepath), measureHP));
                binarizeHPImageView = BitmapConversion.Bitmap2BitmapImage(binarizeHPImage);

                kMMHP = await Task.Run(() => BitmapConversion.Bitmap2BitmapImage(KMMHighPerformance.Init(binarizeHPImage, measureHP)));

                timeElapsedHP = measureHP.TimeElapsed();
            }

            var task1 = Task.Run(() => InitializeLP());
            //task1.Wait();

            var task2 = Task.Run(() => InitializeHP());
            //task2.Wait();

            Task.WaitAll(task1, task2);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string DisplayedImage
        {
            get { return filepath; }
        }

        public string CpuName
        {
            get { return GetHardwareInfo.GetCPUName(); }
        }

        public BitmapImage DisplayedBinarizeLPImage
        {
            get { return binarizeLPImageView; }

            set { binarizeLPImageView = value; NotifyPropertyChanged(nameof(binarizeLPImageView)); }

        }

        public BitmapImage DisplayedBinarizeHPImage
        {
            get { return binarizeHPImageView; }

            set { binarizeHPImageView = value; NotifyPropertyChanged(nameof(binarizeHPImageView)); }
        }


        public BitmapImage DisplayedLowPerformanceImage
        {
            get { return kMMLP; }

            set { kMMLP = value; NotifyPropertyChanged(nameof(kMMLP)); }
        }

        public BitmapImage DisplayedHighPerformanceImage
        {
            get { return kMMHP; }

            set { kMMHP = value; NotifyPropertyChanged(nameof(kMMHP)); }
        }

        public long DisplayedLPTime
        {
            get { return timeElapsedLP; }

            set { timeElapsedLP = value; NotifyPropertyChanged(nameof(timeElapsedLP)); }
        }

        public long DisplayedHPTime
        {
            get { return timeElapsedHP; }

            set { timeElapsedHP = value; NotifyPropertyChanged(nameof(timeElapsedHP)); }
        }

        public string filepath { get; set; }
        
        Bitmap binarizeLPImage { get; set; }
        Bitmap binarizeHPImage { get; set; }

        BitmapImage binarizeLPImageView { get; set; }
        BitmapImage binarizeHPImageView { get; set; }

        BitmapImage kMMLP { get; set; }
        BitmapImage kMMHP { get; set; }

        long timeElapsedLP { get; set; }
        long timeElapsedHP { get; set; }



    }
}
