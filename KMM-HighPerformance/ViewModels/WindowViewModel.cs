using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
                return newImageCommand ?? (newImageCommand = new Commands.CommandHandler(() => GetImageAndInitKMM(), canExecute));
            }
        }

        public ICommand SaveImageCommand //command for button click
        {
            get
            {
                return saveImageCommand ?? (saveImageCommand = new Commands.CommandHandler(() => SaveImageToFile(), canExecute));
            }
        }

        public void GetImageAndInitKMM()
        {
            DisplayedImage = Pictures.GetNewImageFilepath(); //image given as input to app

            async Task InitializeLP() //initialize methods that use Get/Set Pixel
            {
                Measure measureLP = new Measure();

                binarizeLPImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(filepath));
                DisplayedBinarizeLPImage = await Task.Run(() => Binarization.LowPerformance(new Bitmap(filepath), binarizeLPImage, measureLP)); 
                DisplayedLowPerformanceImage = await Task.Run(() => KMMLowPerformance.Init(binarizeLPImage, measureLP));

                DisplayedLPTime = measureLP.TimeElapsed();
            }

            async Task InitializeHP() //initialize methods with lockbits, marshall copy
            {
                Measure measureHP = new Measure();

                binarizeHPImage = await Task.Run(() => Binarization.HighPerformance(new Bitmap(filepath), measureHP));
                DisplayedBinarizeHPImage = BitmapConversion.Bitmap2BitmapImage(binarizeHPImage);
                DisplayedHighPerformanceImage = await Task.Run(() => BitmapConversion.Bitmap2BitmapImage(KMMHighPerformance.Init(binarizeHPImage, measureHP)));

                DisplayedHPTime = measureHP.TimeElapsed();
            }

            var task1 = Task.Run(() => InitializeLP());
            var task2 = Task.Run(() => InitializeHP());
            Task.WaitAll(task1, task2);
        }

        public void SaveImageToFile() => Pictures.SaveImageToFile(kMMHP);


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string DisplayedImage //displaying image given as input
        {
            get { return filepath; }
            set { filepath = value; NotifyPropertyChanged(nameof(DisplayedImage)); }
        }

        public string CpuName
        {
            get { return GetHardwareInfo.GetCPUName(); }
        }

        public BitmapImage DisplayedBinarizeLPImage //displaying image from filepath after binarization with Get/Set pixel
        {
            get { return binarizeLPImageView; }

            set { binarizeLPImageView = value; NotifyPropertyChanged(nameof(DisplayedBinarizeLPImage)); }

        }

        public BitmapImage DisplayedBinarizeHPImage //displaying image from filepath after binarization with lockbits
        {
            get { return binarizeHPImageView; }

            set { binarizeHPImageView = value; NotifyPropertyChanged(nameof(DisplayedBinarizeHPImage)); }
        }

        public BitmapImage DisplayedLowPerformanceImage //displaying image from filepath after kmm with Get/Set pixel
        {
            get { return kMMLP; }

            set { kMMLP = value; NotifyPropertyChanged(nameof(DisplayedLowPerformanceImage)); }
        }

        public BitmapImage DisplayedHighPerformanceImage //displaying image from filepath after kmm with lockbits
        {
            get { return kMMHP; }

            set { kMMHP = value; NotifyPropertyChanged(nameof(DisplayedHighPerformanceImage)); }
        }

        public long DisplayedLPTime //displaying elapsed time of `LowPerformance` methods
        {
            get { return timeElapsedLP; }

            set { timeElapsedLP = value; NotifyPropertyChanged(nameof(DisplayedLPTime)); }
        }

        public long DisplayedHPTime //displaying elapsed time of `HighPerformance` methods
        {
            get { return timeElapsedHP; }

            set { timeElapsedHP = value; NotifyPropertyChanged(nameof(DisplayedHPTime)); }
        }

        public string filepath { get; set; }

        private Bitmap binarizeLPImage { get; set; }
        private Bitmap binarizeHPImage { get; set; }

        private BitmapImage binarizeLPImageView { get; set; }
        private BitmapImage binarizeHPImageView { get; set; }

        private BitmapImage kMMLP { get; set; }
        private BitmapImage kMMHP { get; set; }

        private long timeElapsedLP { get; set; }
        private long timeElapsedHP { get; set; }

        private bool canExecute;
        private ICommand newImageCommand;
        private ICommand saveImageCommand;

    }
}
