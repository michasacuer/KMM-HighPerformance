using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Functions.AlgorithmHelpers;
using KMM_HighPerformance.Functions.Conversions;
using KMM_HighPerformance.Models;

namespace KMM_HighPerformance.Functions.Algorithms
{
    static class KMMLowPerformanceMain
    {
        static public BitmapImage Init(Bitmap newImage, MeasureTime measure)
        {
            var stopwatch = Stopwatch.StartNew();
            int[,] pixelArray = new int[newImage.Height, newImage.Width]; // one record on this array = one pixel

            pixelArray = LowPerformance.SetOneZero(newImage, pixelArray);

            deletion = 1;
            int deletionFirst, deletionSecond;

            while (deletion != 0)
            {
                deletion = 0;

                pixelArray                   = LowPerformance.SetOneTwoThree(newImage, pixelArray);
                (deletionFirst, pixelArray)  = LowPerformance.FindAndDeleteFour(newImage, pixelArray);
                (deletionSecond, pixelArray) = LowPerformance.DeletingTwoThree(newImage, pixelArray);

                deletion = deletionFirst > deletionSecond ? deletionFirst : deletionSecond;
            }

            newImage = LowPerformance.SetImageAfterKMM(newImage, pixelArray);

            measure.SumTimeElapsedMs(stopwatch.ElapsedMilliseconds);
            return BitmapConversion.Bitmap2BitmapImage(newImage);
        }

        static private int deletion = 1;       
    }
}
