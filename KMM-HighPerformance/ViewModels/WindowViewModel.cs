using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Algorithms;
using KMM_HighPerformance.Conversions;
using KMM_HighPerformance.MeasureTime;
using KMM_HighPerformance.HardwareInformation;
using KMM_HighPerformance.PicturesToPlay;
using System;
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
            get
            {
                return newImageCommand ?? (newImageCommand = new Commands.CommandHandler(() => GetImageFilepath(), canExecute));
            }
        }

        public ICommand SaveImageCommand //command for button click
        {
            get
            {
                return saveImageCommand ?? (saveImageCommand = new Commands.CommandHandler(() => SaveImageToFile(), canExecute));
            }
        }

        public ICommand ApplyKMMCommand //command for button click
        {
            get
            {
                return applyKMMCommand ?? (applyKMMCommand = new Commands.CommandHandler(() => ApplyKMMToNewImage(), canExecute));
            }
        }

        public void SaveImageToFile() => Pictures.SaveImageToFile(Bitmaps.kMMHP);
        public void GetImageFilepath() => DisplayedImage = Pictures.GetNewImageFilepath();

        public void ApplyKMMToNewImage()
        {

            async Task InitializeLP() //initialize methods that use Get/Set Pixel
            {
                Measure measureLP = new Measure();

                Bitmaps.binarizeLPImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(Bitmaps.filepath));
                DisplayedBinarizeLPImage = await Task.Run(() => Binarization.LowPerformance(new Bitmap(Bitmaps.filepath), Bitmaps.binarizeLPImage, measureLP)); 
                DisplayedLowPerformanceImage = await Task.Run(() => KMMLowPerformance.Init(Bitmaps.binarizeLPImage, measureLP));

                DisplayedLPTime = measureLP.TimeElapsed();
            }

            async Task InitializeHP() //initialize methods with lockbits, marshall copy
            {
                Measure measureHP = new Measure();

                Bitmaps.binarizeHPImage = await Task.Run(() => Binarization.HighPerformance(new Bitmap(Bitmaps.filepath), measureHP));
                DisplayedBinarizeHPImage = BitmapConversion.Bitmap2BitmapImage(Bitmaps.binarizeHPImage);
                DisplayedHighPerformanceImage = await Task.Run(() => BitmapConversion.Bitmap2BitmapImage(KMMHighPerformance.Init(Bitmaps.binarizeHPImage, measureHP)));

                DisplayedHPTime = measureHP.TimeElapsed();
            }

            try
            {
                var task1 = Task.Run(() => InitializeLP());
                var task2 = Task.Run(() => InitializeHP());
                Task.WaitAll(task1, task2);
            }

            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is AggregateException)
                {
                    System.Windows.MessageBox.Show("There is no image to Apply KMM");
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
            get { return Bitmaps.filepath; }
            set { Bitmaps.filepath = value; NotifyPropertyChanged(nameof(DisplayedImage)); }
        }

        public string CpuName
        {
            get { return GetHardwareInfo.GetCPUName(); }
        }

        public BitmapImage DisplayedBinarizeLPImage //displaying image from filepath after binarization with Get/Set pixel
        {
            get { return Bitmaps.binarizeLPImageView; }

            set { Bitmaps.binarizeLPImageView = value; NotifyPropertyChanged(nameof(DisplayedBinarizeLPImage)); }
        }

        public BitmapImage DisplayedBinarizeHPImage //displaying image from filepath after binarization with lockbits
        {
            get { return Bitmaps.binarizeHPImageView; }

            set { Bitmaps.binarizeHPImageView = value; NotifyPropertyChanged(nameof(DisplayedBinarizeHPImage)); }
        }

        public BitmapImage DisplayedLowPerformanceImage //displaying image from filepath after kmm with Get/Set pixel
        {
            get { return Bitmaps.kMMLP; }

            set { Bitmaps.kMMLP = value; NotifyPropertyChanged(nameof(DisplayedLowPerformanceImage)); }
        }

        public BitmapImage DisplayedHighPerformanceImage //displaying image from filepath after kmm with lockbits
        {
            get { return Bitmaps.kMMHP; }

            set { Bitmaps.kMMHP = value; NotifyPropertyChanged(nameof(DisplayedHighPerformanceImage)); }
        }

        public long DisplayedLPTime //displaying elapsed time of `LowPerformance` methods
        {
            get { return Bitmaps.timeElapsedLP; }

            set { Bitmaps.timeElapsedLP = value; NotifyPropertyChanged(nameof(DisplayedLPTime)); }
        }

        public long DisplayedHPTime //displaying elapsed time of `HighPerformance` methods
        {
            get { return Bitmaps.timeElapsedHP; }

            set { Bitmaps.timeElapsedHP = value; NotifyPropertyChanged(nameof(DisplayedHPTime)); }
        }

        private bool canExecute;
        private ICommand newImageCommand;
        private ICommand saveImageCommand;
        private ICommand applyKMMCommand;

    }
}
