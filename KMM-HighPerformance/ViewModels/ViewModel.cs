﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    class ViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

                //var lowPerformanceTasks = Task.Factory.StartNew(() => binarizeLPImage = Binarization.LowPerformance(new Bitmap(filepath), binarizeLPImage))
                //                                      .ContinueWith((prevTask) => kMMLP = await KMMLowPerformance.Init(new Bitmap(filepath), binarizeLPImage))
                //                                      .ContinueWith((prevTask) => timeElapsedLP = Binarization.TimeElapsed() + KMMLowPerformance.TimeElapsed());
                //
                //lowPerformanceTasks.Wait();
                //
                //var highPerformanceTasks = Task.Factory.StartNew(() => binarizeHPImage = Binarization.HighPerformance(new Bitmap(filepath)))
                //                                       .ContinueWith((prevTask) => timeElapsedHP = Binarization.TimeElapsed());
                //
                //highPerformanceTasks.Wait();



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

                timeElapsedHP = measureHP.TimeElapsed();

            }

            var task1 = Task.Run(() => InitializeLP());
            task1.Wait();

            var task2 = Task.Run(() => InitializeHP());
            task2.Wait();
        }

        public string DisplayedImage
        {
            get { return filepath; }
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

    }
}
