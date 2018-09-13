using System;
using System.Windows;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Algorithms;
using KMM_HighPerformance.Conversions;
using KMM_HighPerformance.HardwareInformation;
using KMM_HighPerformance.PicturesToPlay;
using KMM_HighPerformance.Models;

namespace KMM_HighPerformance.ViewModels 
{
    class WindowViewModel : INotifyPropertyChanged
    {
        public WindowViewModel()
        {
            canExecute = true;
        }

        public ICommand NewImageCommand //command for button click
        {
            get { return newImageCommand ?? (newImageCommand = new Commands.CommandHandler(() => GetImageFilepath(), canExecute)); }
        }

        public ICommand SaveImageCommand //command for button click
        {
            get { return saveImageCommand ?? (saveImageCommand = new Commands.CommandHandler(() => SaveImageToFile(), canExecute)); }
        }

        public ICommand ApplyKMMCommand //command for button click
        {
            get { return applyKMMCommand ?? (applyKMMCommand = new Commands.CommandHandler(() => ApplyKMMToNewImage(), canExecute)); }         
        }

        public void SaveImageToFile() => Pictures.SaveImageToFile(Bitmaps.KMMHP);
        public void GetImageFilepath() => DisplayedImage = Pictures.GetNewImageFilepath();

        public void ApplyKMMToNewImage()
        {
            //ApplyKMM.Init();

            async Task InitializeLP() //initialize methods that use Get/Set Pixel
            {
                MeasureTime measureLP = new MeasureTime();
                Bitmaps.BinarizeLPImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(Bitmaps.Filepath));
                DisplayedBinarizeLPImage = await Task.Run(() => Binarization.LowPerformance(new Bitmap(Bitmaps.Filepath),
                                                                                            Bitmaps.BinarizeLPImage, 
                                                                                            measureLP
                                                                                            )); 

                DisplayedLowPerformanceImage = await Task.Run(() => KMMLowPerformanceMain.Init(Bitmaps.BinarizeLPImage,
                                                                                               measureLP
                                                                                               ));      
                DisplayedLPTime = measureLP.TimeElapsed();
            }
            
            async Task InitializeHP() //initialize methods with lockbits, marshall copy
            {
                MeasureTime measureHP = new MeasureTime();
                Bitmaps.BinarizeHPImage = await Task.Run(() => Binarization.HighPerformance(new Bitmap(Bitmaps.Filepath),
                                                                                            measureHP)
                                                                                            );

                DisplayedBinarizeHPImage = BitmapConversion.Bitmap2BitmapImage(Bitmaps.BinarizeHPImage);
                DisplayedHighPerformanceImage = await Task.Run(() => BitmapConversion.Bitmap2BitmapImage(KMMHighPerformanceMain.Init(Bitmaps.BinarizeHPImage,
                                                                                                                                     measureHP)
                                                                                                                                     ));          
                DisplayedHPTime = measureHP.TimeElapsed();
            }
            
            try
            {
                Task task1 = Task.Run(() => InitializeLP());
                Task task2 = Task.Run(() => InitializeHP());
                Task.WaitAll(task1, task2);
            }
            
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is AggregateException)
                {
                    MessageBox.Show("There is no image to Apply KMM");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string DisplayedImage //displaying image given as input
        {
            get { return Bitmaps.Filepath; }
            set
            {
                Bitmaps.Filepath = value;
                NotifyPropertyChanged(nameof(DisplayedImage));
            }
        }

        public string CpuName
        {
            get { return GetHardwareInfo.GetCPUName(); }
        }

        public BitmapImage DisplayedBinarizeLPImage //displaying image from filepath after binarization with Get/Set pixel
        {
            get { return Bitmaps.BinarizeLPImageView; }

            set
            {
                Bitmaps.BinarizeLPImageView = value; 
                NotifyPropertyChanged(nameof(DisplayedBinarizeLPImage));
            }
        }

        public BitmapImage DisplayedBinarizeHPImage //displaying image from filepath after binarization with lockbits
        {
            get { return Bitmaps.BinarizeHPImageView; }

            set
            {
                Bitmaps.BinarizeHPImageView = value;
                NotifyPropertyChanged(nameof(DisplayedBinarizeHPImage));
            }           
        }

        public BitmapImage DisplayedLowPerformanceImage //displaying image from filepath after kmm with Get/Set pixel
        {
            get { return Bitmaps.KMMLP; }

            set
            {
                Bitmaps.KMMLP = value;
                NotifyPropertyChanged(nameof(DisplayedLowPerformanceImage));
            }
        }

        public BitmapImage DisplayedHighPerformanceImage //displaying image from filepath after kmm with lockbits
        {
            get { return Bitmaps.KMMHP; }

            set
            {
                Bitmaps.KMMHP = value;
                NotifyPropertyChanged(nameof(DisplayedHighPerformanceImage));
            }
        }

        public long DisplayedLPTime //displaying elapsed time of `LowPerformance` methods
        {
            get { return Bitmaps.TimeElapsedLP; }

            set
            {
                Bitmaps.TimeElapsedLP = value;
                NotifyPropertyChanged(nameof(DisplayedLPTime));
            }
        }

        public long DisplayedHPTime //displaying elapsed time of `HighPerformance` methods
        {
            get { return Bitmaps.TimeElapsedHP; }

            set
            {
                Bitmaps.TimeElapsedHP = value;
                NotifyPropertyChanged(nameof(DisplayedHPTime));
            }
        }

        private bool canExecute;
        private ICommand newImageCommand;
        private ICommand saveImageCommand;
        private ICommand applyKMMCommand;

    }
}
