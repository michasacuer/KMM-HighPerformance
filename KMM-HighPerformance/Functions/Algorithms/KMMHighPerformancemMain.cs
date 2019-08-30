using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using KMM_HighPerformance.Models;
using KMM_HighPerformance.Functions.AlgorithmHelpers;
using KMM_HighPerformance.Functions.Conversions;

namespace KMM_HighPerformance.Functions.Algorithms
{
    class KMMHighPerformanceMain
    {
        public static unsafe Bitmap Init(Bitmap resultBmp, MeasureTime measure)
        {
            var stopwatch = Stopwatch.StartNew();

            resultBmp = BitmapConversion.Create8bppGreyscaleImage(resultBmp);
            BitmapData bmpData = resultBmp.LockBits(new Rectangle(0, 0, resultBmp.Width, resultBmp.Height), 
                                                    ImageLockMode.ReadWrite, 
                                                    resultBmp.PixelFormat
                                                    );
            
            int bytes = bmpData.Stride * resultBmp.Height;
            byte[] pixels = new byte[bytes];

            Marshal.Copy(bmpData.Scan0, pixels, 0, bytes);
            int height = resultBmp.Height;
            int width  = resultBmp.Width;

            int deletion = 1;
            int deletionFirst, deletionSecond;

            pixels = HighPerformance.Execute(pixels, bmpData.Stride, height, width);
            

            Marshal.Copy(pixels, 0, bmpData.Scan0, bytes);
            resultBmp.UnlockBits(bmpData);

            stopwatch.Stop();
            measure.SumTimeElapsedMs(stopwatch.ElapsedMilliseconds);
            measure.SumTimeElapsedTicks(stopwatch.ElapsedTicks);
            return resultBmp;
        }
    }
}

