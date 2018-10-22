using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Functions.HardwareInformation;
using KMM_HighPerformance.Functions.PicturesToPlay;
using KMM_HighPerformance.Models;
using KMM_HighPerformance.Functions.AlgorithmsHelpers;

namespace KMM_HighPerformance.ViewModels 
{
    class WindowViewModel : INotifyPropertyChanged
    {
        public WindowViewModel()
        {
            canExecute = true;
            Bitmaps    = new Bitmaps();
            ApplyKMM   = new ApplyKMM(Bitmaps);
        }

        public ICommand NewImageCommand //command for button click
        {
            get => newImageCommand ?? (newImageCommand = new Commands.CommandHandler(() => GetImageFilepath(), canExecute)); 
        }

        public ICommand SaveImageCommand //command for button click
        {
            get => saveImageCommand ?? (saveImageCommand = new Commands.CommandHandler(() => SaveImageToFile(), canExecute)); 
        }

        public ICommand ApplyKMMCommand //command for button click
        {
            get => applyKMMCommand ?? (applyKMMCommand = new Commands.CommandHandler(() => ApplyKMMToNewImage(), canExecute));          
        }

        public void SaveImageToFile()  => Pictures.SaveImageToFile(Bitmaps.KMMHP);
        public void GetImageFilepath() => DisplayedImage = Pictures.GetNewImageFilepath();

        public void ApplyKMMToNewImage()
        {
            Bitmaps = ApplyKMM.Result();

            DisplayedBinarizeLPImage      = Bitmaps.BinarizeLPImageView;
            DisplayedLowPerformanceImage  = Bitmaps.KMMLP;
            DisplayedBinarizeHPImage      = Bitmaps.BinarizeHPImageView;
            DisplayedHighPerformanceImage = Bitmaps.KMMHP;
            DisplayedLPTime               = Bitmaps.TimeElapsedLP;
            DisplayedHPTime               = Bitmaps.TimeElapsedHP;
            DisplayedHPTimeInTicks        = Bitmaps.TimeElapsedHPTicks;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string CpuName
        {
            get => GetHardwareInfo.GetCPUName(); 
        }

        public string DisplayedImage //displaying image given as input
        {
            get => Bitmaps.Filepath; 
            set
            {
                Bitmaps.Filepath = value;
                NotifyPropertyChanged(nameof(DisplayedImage));
            }
        }

        public BitmapImage DisplayedBinarizeLPImage //displaying image from filepath after binarization with Get/Set pixel
        {
            get => Bitmaps.BinarizeLPImageView; 
            set
            {
                Bitmaps.BinarizeLPImageView = value; 
                NotifyPropertyChanged(nameof(DisplayedBinarizeLPImage));
            }
        }

        public BitmapImage DisplayedBinarizeHPImage //displaying image from filepath after binarization with lockbits
        {
            get => Bitmaps.BinarizeHPImageView; 
            set
            {
                Bitmaps.BinarizeHPImageView = value;
                NotifyPropertyChanged(nameof(DisplayedBinarizeHPImage));
            }           
        }

        public BitmapImage DisplayedLowPerformanceImage //displaying image from filepath after kmm with Get/Set pixel
        {
            get => Bitmaps.KMMLP; 
            set
            {
                Bitmaps.KMMLP = value; //funny comment
                NotifyPropertyChanged(nameof(DisplayedLowPerformanceImage));
            }
        }

        public BitmapImage DisplayedHighPerformanceImage //displaying image from filepath after kmm with lockbits
        {
            get => Bitmaps.KMMHP; 
            set
            {
                Bitmaps.KMMHP = value;
                NotifyPropertyChanged(nameof(DisplayedHighPerformanceImage));
            }
        }

        public long DisplayedLPTime //displaying elapsed time of `LowPerformance` methods
        {
            get => Bitmaps.TimeElapsedLP; 
            set
            {
                Bitmaps.TimeElapsedLP = value;
                NotifyPropertyChanged(nameof(DisplayedLPTime));
            }
        }

        public long DisplayedHPTime //displaying elapsed time of `HighPerformance` methods
        {
            get => Bitmaps.TimeElapsedHP; 
            set
            {
                Bitmaps.TimeElapsedHP = value;
                NotifyPropertyChanged(nameof(DisplayedHPTime));
            }
        }

        public long DisplayedHPTimeInTicks //displaying elapsed time of `HighPerformance` methods
        {
            get => Bitmaps.TimeElapsedHPTicks; 
            set
            {
                Bitmaps.TimeElapsedHPTicks = value;
                NotifyPropertyChanged(nameof(DisplayedHPTimeInTicks));
            }
        }

        private Bitmaps  Bitmaps;
        private ApplyKMM ApplyKMM;
        private bool     canExecute;
        private ICommand newImageCommand;
        private ICommand saveImageCommand;
        private ICommand applyKMMCommand;
    }
}
