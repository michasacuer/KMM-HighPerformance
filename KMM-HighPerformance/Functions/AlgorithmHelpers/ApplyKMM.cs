using KMM_HighPerformance.Functions.Algorithms;
using KMM_HighPerformance.Functions.Conversions;
using KMM_HighPerformance.Models;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace KMM_HighPerformance.Functions.AlgorithmsHelpers
{
    class ApplyKMM
    {

        public ApplyKMM(Bitmaps passedBitmaps)
        {
            Bitmaps = passedBitmaps;     
        }

        public Bitmaps Result()
        {
            Execute();
            return Bitmaps;
        }
        
        private void Execute()
        {
            async Task InitializeLP() //initialize methods that use Get/Set Pixel
            {
                MeasureTime measureLP = new MeasureTime();

                Bitmaps.BinarizeLPImage = BitmapConversion.CreateNonIndexedImage(new Bitmap(Bitmaps.Filepath));
                Bitmaps.BinarizeLPImageView = await Task.Run(() => Binarization.LowPerformance(new Bitmap(Bitmaps.Filepath), Bitmaps.BinarizeLPImage, measureLP));
                Bitmaps.KMMLP = await Task.Run(() => KMMLowPerformanceMain.Init(Bitmaps.BinarizeLPImage, measureLP));

                Bitmaps.TimeElapsedLP = measureLP.TimeElapsedMs;
            }

            async Task InitializeHP() //initialize methods with lockbits, marshall copy
            {
                MeasureTime measureHP = new MeasureTime();

                Bitmaps.BinarizeHPImage = await Task.Run(() => Binarization.HighPerformance(new Bitmap(Bitmaps.Filepath), measureHP));
                Bitmaps.BinarizeHPImageView = BitmapConversion.Bitmap2BitmapImage(Bitmaps.BinarizeHPImage);
                Bitmaps.KMMHP = await Task.Run(() => BitmapConversion.Bitmap2BitmapImage(KMMHighPerformanceMain.Init(Bitmaps.BinarizeHPImage, measureHP)));

                Bitmaps.TimeElapsedHP      = measureHP.TimeElapsedMs;
                Bitmaps.TimeElapsedHPTicks = measureHP.TimeElapsedTicks;
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

        private Bitmaps Bitmaps;
    } 
}
