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
        static public BitmapImage Init(Bitmap resultBmp, MeasureTime measure)
        {
            var stopwatch = Stopwatch.StartNew();
            int[,] pixelArray = new int[resultBmp.Height, resultBmp.Width]; // one record on this array = one pixel

            pixelArray = LowPerformance.SetOneZero(resultBmp, pixelArray);

            deletion = 1;
            int deletionFirst, deletionSecond;

            while (deletion != 0)
            {
                deletion = 0;

                pixelArray                   = LowPerformance.SetOneTwoThree(resultBmp, pixelArray);
                (deletionFirst, pixelArray)  = LowPerformance.FindAndDeleteFour(resultBmp, pixelArray);
                (deletionSecond, pixelArray) = LowPerformance.DeletingTwoThree(resultBmp, pixelArray);

                deletion = deletionFirst > deletionSecond ? deletionFirst : deletionSecond;
            }

            resultBmp = LowPerformance.SetImageAfterKMM(resultBmp, pixelArray);

            measure.SumTimeElapsedMs(stopwatch.ElapsedMilliseconds);
            return BitmapConversion.Bitmap2BitmapImage(resultBmp);
        }

        private static int deletion = 1;       
    }
}
