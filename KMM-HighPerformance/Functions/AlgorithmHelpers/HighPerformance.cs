using KMM_HighPerformance.Functions.AlgorithmsHelpers;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading.Tasks;

namespace KMM_HighPerformance.Functions.AlgorithmHelpers
{
    class HighPerformance
    {
        public static byte[] SetTwoThree(byte[] pixels, BitmapData bmpData, int height, int width)
        {
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

                        pixels[positionOfPixel] = checkStick == true && checkClose == false ? three :
                                                  checkStick == false && checkClose == false ? one : two;
                    }
                }
            });

            return pixels;
        }

        public static (int, byte[]) FindAndDeleteFour(byte[] pixels, BitmapData bmpData, int height, int width, int deletion)
        {
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
                                pixels[positionOfPixel] = zero;
                                deletion++;
                            }
                        }
                    }
                }
            });

            return (deletion, pixels);
        }

        public static (int, byte[]) DeletingTwoThree(byte[] pixels, BitmapData bmpData, int height, int width, int deletion)
        {
            int N = 2;
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
                                pixels[positionOfPixel] = zero;
                                deletion++;
                            }

                            else
                            {
                                pixels[positionOfPixel] = one;
                            }
                        }
                    }
                });
                N++;
            }
            return (deletion, pixels);
        }

        private const byte zero = byte.MaxValue;
        private const byte one = byte.MinValue;
        private const byte two = 32;
        private const byte three = 64;

        private static bool CheckStickZeros(List<byte> list) => list.Contains(zero);
        private static bool CheckCloseZeros(List<byte> list) => list.Contains(zero);
    }
}
