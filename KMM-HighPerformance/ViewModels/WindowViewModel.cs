using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Models;

namespace KMM_HighPerformance.ViewModels 
{
    class WindowViewModel : INotifyPropertyChanged
    {
        public WindowViewModel()
        {

            filepath = Pictures.GetNewImage();

            async Task InitializeLP()
            {
                Measure measureLP = new Measure();

                binarizeLPImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(filepath));
                binarizeLPImage = await Task.Run(() => Binarization.LowPerformance(new Bitmap(filepath), binarizeLPImage, measureLP));
                binarizeLPImageView = BitmapConversion.Bitmap2BitmapImage(binarizeLPImage);

                kMMLP = BitmapConversion.Bitmap2BitmapImage(new Bitmap(filepath));
                kMMLP = await Task.Run(() => KMMLowPerformance.Init(new Bitmap(filepath), binarizeLPImage, measureLP));

                timeElapsedLP = measureLP.TimeElapsed();               
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
            var task2 = Task.Run(() => InitializeHP());

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
