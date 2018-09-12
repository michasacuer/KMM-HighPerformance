using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using KMM_HighPerformance.Conversions;
using KMM_HighPerformance.Models;
using KMM_HighPerformance.Functions.AlgorithmHelpers;

namespace KMM_HighPerformance.Algorithms
{
    class KMMHighPerformanceMain
    {
        static public Bitmap Init(Bitmap tempBmp, MeasureTime measure)
        {
            var stopwatch = Stopwatch.StartNew();

            tempBmp = BitmapConversion.Create8bppGreyscaleImage(tempBmp);

            unsafe
            {
                BitmapData bmpData = tempBmp.LockBits(new Rectangle(0, 0, 
                                                      tempBmp.Width, 
                                                      tempBmp.Height), 
                                                      ImageLockMode.ReadWrite, 
                                                      tempBmp.PixelFormat
                                                      );
                
                int bytes = bmpData.Stride * tempBmp.Height;
                byte[] pixels = new byte[bytes];

                Marshal.Copy(bmpData.Scan0, pixels, 0, bytes);

                int height = tempBmp.Height;
                int width = tempBmp.Width;              

                int deletion = 1;
                while (deletion != 0)
                {
                    deletion = 0;
                    pixels = HighPerformance.SetTwoThree(pixels, 
                                                         bmpData, 
                                                         height, 
                                                         width);

                    (deletion, pixels) = HighPerformance.FindAndDeleteFour(pixels, 
                                                                           bmpData, 
                                                                           height, 
                                                                           width, 
                                                                           deletion);

                    (deletion, pixels) = HighPerformance.DeletingTwoThree(pixels, 
                                                                          bmpData, 
                                                                          height, 
                                                                          width, 
                                                                          deletion);
                }

                Marshal.Copy(pixels, 0, bmpData.Scan0, bytes);
                tempBmp.UnlockBits(bmpData);
            }
            measure.SumTimeElapsed(stopwatch.ElapsedMilliseconds);
            return tempBmp;
        }
    }

}

