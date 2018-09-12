using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using KMM_HighPerformance.Conversions;
using KMM_HighPerformance.MeasureTime;
using KMM_HighPerformance.Functions.Algorithms;

namespace KMM_HighPerformance.Algorithms
{
    static class KMMHighPerformance
    {
        static public Bitmap Init(Bitmap tempBmp, Measure measure)
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
                byte[] pixelsCopy = new byte[bytes];

                Marshal.Copy(bmpData.Scan0, pixels, 0, bytes);
                Marshal.Copy(bmpData.Scan0, pixelsCopy, 0, bytes);

                int height = tempBmp.Height;
                int width = tempBmp.Width;
                int deletion = 1;

                while (deletion != 0)
                {
                    int N = 2;
                    deletion = 0;

                    Parallel.For(0, height - 1, y => //seting 2s and 3s
                    {
                        int offset = y * bmpData.Stride; //row
                        for (int x = 0; x < width - 1; x++)
                        {
                            int positionOfPixel = x + offset;
                            if (pixels[positionOfPixel] == one && x > 0 && x < width
                                                               && y > 0 && y < height)
                            {
                                bool checkStick;                                  //Stick =    zeros to corners          0
                                bool checkClose;                                  //Close=     zeros looking like = 0 : pix : 0
                                                                                  //                                     0
                                List<byte> stickPixels = Lists.StickToPixel(pixels, positionOfPixel, bmpData.Stride);
                                List<byte> closePixels = Lists.CloseToPixel(pixels, positionOfPixel, bmpData.Stride);

                                checkClose = CheckCloseZeros(closePixels);
                                checkStick = CheckStickZeros(stickPixels);

                                pixelsCopy[positionOfPixel] = checkStick == true  && checkClose == false ? three :
                                                              checkStick == false && checkClose == false ? one : two;
                            }
                        }
                    });

                    pixels = pixelsCopy;

                    Parallel.For(0, height, y => //looking for 4s and deleting all
                    {
                        int offset = y * bmpData.Stride; //set row
                        for (int x = 0; x < width; x++)
                        {
                            int positionOfPixel = x + offset;
                            if (pixels[positionOfPixel] == two && x > 0 && x < width
                                                               && y > 0 && y < height)
                            {
                                int counter = 0;
                                int summary = 0;

                                List<byte> stickPixels = Lists.PixelsAround(pixels, positionOfPixel, bmpData.Stride);

                                for (int i = 0; i < stickPixels.Count; i++)
                                {
                                    if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                                    {
                                        counter++;
                                        summary += Lists.compareList[i];
                                    }
                                }

                                if (counter == 2 || counter == 3 || counter == 4)
                                {
                                    if (Lists.deleteList.Contains(summary))
                                    {
                                        pixelsCopy[positionOfPixel] = zero;
                                        deletion++;
                                    }
                                }
                            }
                        }
                    });

                    pixels = pixelsCopy;

                    while (N <= 3) //deleting 2 and 3s
                    {
                        var value = N == 2 ? two : three;
                      
                        Parallel.For(0, height, y => 
                        {
                            int offset = y * bmpData.Stride; //set row
                            for (int x = 0; x < width; x++)
                            {
                                int positionOfPixel = x + offset;
                                if (pixels[positionOfPixel] == value)
                                {
                                    int summary = 0;

                                    List<byte> stickPixels = Lists.PixelsAround(pixels, positionOfPixel, bmpData.Stride);

                                    for (int i = 0; i < stickPixels.Count; i++)
                                    {
                                        if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                                        {
                                            summary += Lists.compareList[i];
                                        }
                                    }

                                    if (Lists.deleteList.Contains(summary))
                                    {
                                         pixelsCopy[positionOfPixel] = zero;
                                         deletion++;
                                    }

                                    else
                                    {
                                        pixelsCopy[positionOfPixel] = one;
                                    }
                                }
                            }   
                        });
                        N++;
                    }
                    pixels = pixelsCopy;
                }

                Marshal.Copy(pixels, 0, bmpData.Scan0, bytes);
                tempBmp.UnlockBits(bmpData);
            }
            measure.SumTimeElapsed(stopwatch.ElapsedMilliseconds);
            return tempBmp;
        }

        private const byte zero = byte.MaxValue;
        private const byte one = byte.MinValue;
        private const byte two = 32;
        private const byte three = 64;

        private static bool CheckStickZeros(List<byte> list) => list.Contains(zero);
        private static bool CheckCloseZeros(List<byte> list) => list.Contains(zero);
    }
}
