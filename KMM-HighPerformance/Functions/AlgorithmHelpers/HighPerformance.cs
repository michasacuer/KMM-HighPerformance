using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace KMM_HighPerformance.Functions.AlgorithmHelpers
{

    class HighPerformance
    {
        
        public static byte[] Execute(byte[] pixels, int stride, int height, int width)
        {
            int deletion = -1;
            while(deletion != 0)
            {
                deletion = 0;
                Parallel.For(0, height - 1, y => //seting 2s and 3s
                {
                    int offset = y * stride; //row
                    for (int x = 0; x < width - 1; x++)
                    {
                        int positionOfPixel = x + offset;
                        if (pixels[positionOfPixel] == one)
                        {
                            if (pixels[positionOfPixel + 1] == zero ||
                                pixels[positionOfPixel - 1] == zero ||
                                pixels[positionOfPixel + stride] == zero ||
                                pixels[positionOfPixel - stride] == zero)
                            {
                                pixels[positionOfPixel] = two;
                            }
                            else if (pixels[positionOfPixel - stride + 1] == zero ||
                                     pixels[positionOfPixel - stride - 1] == zero ||
                                     pixels[positionOfPixel + stride - 1] == zero ||
                                     pixels[positionOfPixel + stride - 1] == zero)
                            {
                                pixels[positionOfPixel] = three;
                            }
                        }
                    }
                });

                Parallel.For(0, height, y => //looking for 4s and deleting all
                {
                    int offset = y * stride; //set row
                    for (int x = 0; x < width; x++)
                    {
                        int positionOfPixel = x + offset;
                        if (pixels[positionOfPixel] == two)
                        {
                            int counter = 0;
                            int summary = 0;

                            var stickPixels = new int[8]
                            {
                                pixels[positionOfPixel - stride - 1],
                                pixels[positionOfPixel - stride],
                                pixels[positionOfPixel - stride + 1],
                                pixels[positionOfPixel - 1],
                                pixels[positionOfPixel + 1],
                                pixels[positionOfPixel + stride - 1],
                                pixels[positionOfPixel + stride],
                                pixels[positionOfPixel + stride + 1],
                            };

                            for (int i = 0; i < stickPixels.Length; i++)
                            {
                                if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                                {
                                    counter++;
                                    summary += Lists.CompareList[i];
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

                int N = 2;
                while (N <= 3) //deleting 2 and 3s
                {
                    var value = N == 2 ? two : three;

                    Parallel.For(0, height, y =>
                    {
                        int offset = y * stride; //set row
                        for (int x = 0; x < width; x++)
                        {
                            int positionOfPixel = x + offset;
                            if (pixels[positionOfPixel] == value)
                            {
                                int summary = 0;

                                var stickPixels = new int[8]
                                {
                                    pixels[positionOfPixel - stride - 1],
                                    pixels[positionOfPixel - stride],
                                    pixels[positionOfPixel - stride + 1],
                                    pixels[positionOfPixel - 1],
                                    pixels[positionOfPixel + 1],
                                    pixels[positionOfPixel + stride - 1],
                                    pixels[positionOfPixel + stride],
                                    pixels[positionOfPixel + stride + 1],
                                };

                                for (int i = 0; i < stickPixels.Length; i++)
                                {
                                    if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                                    {
                                        summary += Lists.CompareList[i];
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


            }
            return pixels;

        }

        public static byte[] SetOneTwoThree(byte[] pixels, int stride, int height, int width)
        {
            Parallel.For(0, height - 1, y => //seting 2s and 3s
            {
                int offset = y * stride; //row
                for (int x = 0; x < width - 1; x++)
                {
                    int positionOfPixel = x + offset;
                    if (pixels[positionOfPixel] == one)
                    {
                        if (pixels[positionOfPixel + 1] == zero ||
                            pixels[positionOfPixel - 1] == zero ||
                            pixels[positionOfPixel + stride] == zero ||
                            pixels[positionOfPixel - stride] == zero)
                        {
                            pixels[positionOfPixel] = two;
                        }
                        else if (pixels[positionOfPixel - stride + 1] == zero ||
                                 pixels[positionOfPixel - stride - 1] == zero ||
                                 pixels[positionOfPixel + stride - 1] == zero ||
                                 pixels[positionOfPixel + stride - 1] == zero)
                        {
                            pixels[positionOfPixel] = three;
                        }
                    }
                }
            });

            return pixels;
        }

        public static (int, byte[]) FindAndDeleteFour(byte[] pixels, int stride, int height, int width, int deletion)
        {
            Parallel.For(0, height, y => //looking for 4s and deleting all
            {
                int offset = y * stride; //set row
                for (int x = 0; x < width; x++)
                {
                    int positionOfPixel = x + offset;
                    if (pixels[positionOfPixel] == two)
                    {
                        int counter = 0;
                        int summary = 0;

                        var stickPixels = new int[8]
                        {
                            pixels[positionOfPixel - stride - 1],
                            pixels[positionOfPixel - stride],
                            pixels[positionOfPixel - stride + 1],
                            pixels[positionOfPixel - 1],
                            pixels[positionOfPixel + 1],
                            pixels[positionOfPixel + stride - 1],
                            pixels[positionOfPixel + stride],
                            pixels[positionOfPixel + stride + 1],
                        };

                        for (int i = 0; i < stickPixels.Length; i++)
                        {
                            if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                            {
                                counter++;
                                summary += Lists.CompareList[i];
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

        public static (int, byte[]) DeletingTwoThree(byte[] pixels, int stride, int height, int width, int deletion)
        {
            int N = 2;
            while (N <= 3) //deleting 2 and 3s
            {
                var value = N == 2 ? two : three;

                Parallel.For(0, height, y =>
                {
                    int offset = y * stride; //set row
                    for (int x = 0; x < width; x++)
                    {
                        int positionOfPixel = x + offset;
                        if (pixels[positionOfPixel] == value)
                        {
                            int summary = 0;

                            var stickPixels = new int[8]
                            {
                                pixels[positionOfPixel - stride - 1],
                                pixels[positionOfPixel - stride],
                                pixels[positionOfPixel - stride + 1],
                                pixels[positionOfPixel - 1],
                                pixels[positionOfPixel + 1],
                                pixels[positionOfPixel + stride - 1],
                                pixels[positionOfPixel + stride],
                                pixels[positionOfPixel + stride + 1],
                            };

                            for (int i = 0; i < stickPixels.Length; i++)
                            {
                                if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                                {
                                    summary += Lists.CompareList[i];
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

        private const byte zero  = byte.MaxValue;
        private const byte one   = byte.MinValue;
        private const byte two   = 32;
        private const byte three = 64;

        private static bool CheckStickZeros(List<byte> list) => list.Contains(zero);
        private static bool CheckCloseZeros(List<byte> list) => list.Contains(zero);
    }
}
