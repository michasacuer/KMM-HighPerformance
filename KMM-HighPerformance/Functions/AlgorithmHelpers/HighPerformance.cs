using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace KMM_HighPerformance.Functions.AlgorithmHelpers
{

    class HighPerformance
    {

        public static byte[] K3M(byte[] pixels, int stride, int height, int width)
        {
            bool deletion = true;

            var temp = new byte[pixels.Length];
            var result = new byte[pixels.Length];
            Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length); //copying pixels to temp

            while (deletion)
            {
                deletion = false;

                Parallel.For(0, height - 1, y => //seting 2s and 3s
                {
                    int offset = y * stride; //row

                    for (int x = 0; x < width - 1; x++)
                    {
                        int positionOfPixel = x + offset;
                        if (pixels[positionOfPixel] == one)
                        {
                            int summary = 0;

                            var stickPixels = new int[8]
                            {
                                temp[positionOfPixel - stride - 1],
                                temp[positionOfPixel - stride],
                                temp[positionOfPixel - stride + 1],
                                temp[positionOfPixel - 1],
                                temp[positionOfPixel + 1],
                                temp[positionOfPixel + stride - 1],
                                temp[positionOfPixel + stride],
                                temp[positionOfPixel + stride + 1],
                            };

                            for (int i = 0; i < stickPixels.Length; i++)
                            {
                                if (stickPixels[i] == one)
                                {
                                    summary += Lists.CompareList[i];
                                }
                            }

                            if (Lists.A0.Contains(summary))
                            {
                                pixels[positionOfPixel] = two;
                            }
                        }
                    }
                });

                Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length); //copying pixels to temp

                for (int i = 1; i < Lists.A.Length; i++)
                {
                    Parallel.For(0, height - 1, y => //seting 2s and 3s
                    {
                        int offset = y * stride; //row

                        for (int x = 0; x < width - 1; x++)
                        {
                            int positionOfPixel = x + offset;
                            if (pixels[positionOfPixel] == two)
                            {
                                int summary = 0;

                                var stickPixels = new int[8]
                                {
                                    temp[positionOfPixel - stride - 1],
                                    temp[positionOfPixel - stride],
                                    temp[positionOfPixel - stride + 1],
                                    temp[positionOfPixel - 1],
                                    temp[positionOfPixel + 1],
                                    temp[positionOfPixel + stride - 1],
                                    temp[positionOfPixel + stride],
                                    temp[positionOfPixel + stride + 1],
                                };

                                for (int j = 0; j < stickPixels.Length; j++)
                                {
                                    if (stickPixels[j] != zero)
                                    {
                                        summary += Lists.CompareList[j];
                                    }
                                }

                                if (Lists.A[i].Contains(summary))
                                {
                                    pixels[positionOfPixel] = zero;
                                    deletion = true;
                                }
                            }
                        }
                    });

                    Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length); //copying pixels to temp
                }

                for(int y = 1; y < height - 1; y++) //seting 2s and 3s
                {
                    int offset = y * stride; //row

                    for (int x = 0; x < width - 1; x++)
                    {
                        int positionOfPixel = x + offset;
                        if (pixels[positionOfPixel] != zero)
                        {
                            int summary = 0;

                            var stickPixels = new int[8]
                            {
                                        temp[positionOfPixel - stride - 1],
                                        temp[positionOfPixel - stride],
                                        temp[positionOfPixel - stride + 1],
                                        temp[positionOfPixel - 1],
                                        temp[positionOfPixel + 1],
                                        temp[positionOfPixel + stride - 1],
                                        temp[positionOfPixel + stride],
                                        temp[positionOfPixel + stride + 1],
                            };

                            for (int i = 0; i < stickPixels.Length; i++)
                            {
                                if (stickPixels[i] != zero)
                                {
                                    summary += Lists.CompareList[i];
                                }
                            }

                            if (Lists.A0pix.Contains(summary))
                            {
                                pixels[positionOfPixel] = zero;
                                deletion = true;
                            }
                        }
                    }
                }
            }


            Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length); //copying pixels to temp

            return pixels;
        }
        
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
                                if (stickPixels[i] != zero)
                                {
                                    counter++;
                                    summary += Lists.CompareList[i];
                                }
                            }
                            if (Lists.Fours.Contains(summary))
                            {
                                pixels[positionOfPixel] = zero;
                                deletion++;
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

        public static byte[] ZhangSuen(byte[] pixels, int stride, int height, int width)
        {
            bool deletion = true;

            var temp = new byte[pixels.Length];
            var result = new byte[pixels.Length];
            Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length); //copying pixels to temp

            while (deletion)
            {

                deletion = false;

                //step 1

                Parallel.For(0, height - 1, y =>
                {
                    int offset = y * stride; //row
                    for (int x = 0; x < width - 1; x++)
                    {

                        int positionOfPixel = x + offset;

                        if (pixels[positionOfPixel] == one)
                        {
                            int p2 = temp[positionOfPixel - stride];
                            int p3 = temp[positionOfPixel - stride + 1];
                            int p4 = temp[positionOfPixel + 1];        
                            int p5 = temp[positionOfPixel + stride + 1];
                            int p6 = temp[positionOfPixel + stride];   
                            int p7 = temp[positionOfPixel + stride - 1];
                            int p8 = temp[positionOfPixel - 1];        
                            int p9 = temp[positionOfPixel - stride - 1];

                            int neighbours = p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;

                            if (neighbours >= 510 && neighbours <= 1530)
                            {
                                int transitionsToBlack 
                                    = (p2 & ~p3) + (p3 & ~p4) + (p4 & ~p5) + (p5 & ~p6) + (p6 & ~p7) + (p7 & ~p8) + (p8 & ~p9) + (p9 & ~p2);

                                if (transitionsToBlack == 255) 
                                {
                                    if ((~p2 & ~p4 & ~p8) != -1 && (~p2 & ~p6 & ~p8) != -1) 
                                    {
                                        pixels[positionOfPixel] = zero;
                                        deletion = true;
                                    }
                                }
                            }
                        }
                    }
                });

                if (!deletion)
                {
                    break;
                }

                Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length); //copying pixels to temp

                //step 2

                Parallel.For(0, height - 1, y =>
                {
                    int offset = y * stride; //row
                    for (int x = 0; x < width - 1; x++)
                    {

                        int positionOfPixel = x + offset;

                        if (pixels[positionOfPixel] == one)
                        {

                            int p2 = temp[positionOfPixel - stride];
                            int p3 = temp[positionOfPixel - stride + 1];
                            int p4 = temp[positionOfPixel + 1];        
                            int p5 = temp[positionOfPixel + stride + 1];
                            int p6 = temp[positionOfPixel + stride];   
                            int p7 = temp[positionOfPixel + stride - 1];
                            int p8 = temp[positionOfPixel - 1];        
                            int p9 = temp[positionOfPixel - stride - 1];

                            int neighbours = p2 + p3 + p4 + p5 + p6 + p7 + p8 + p9;

                            if (neighbours >= 510 && neighbours <= 1530)
                            {
                                int transitionsToBlack
                                    = (p2 & ~p3) + (p3 & ~p4) + (p4 & ~p5) + (p5 & ~p6) + (p6 & ~p7) + (p7 & ~p8) + (p8 & ~p9) + (p9 & ~p2);

                                if (transitionsToBlack == 255) //theres need do be exactly one transition from white to black
                                {
                                    if ((~p2 & ~p4 & ~p6) != -1 && (~p4 & ~p6 & ~p8) != -1) //last conditions from step 2
                                    {
                                        pixels[positionOfPixel] = zero;
                                        deletion = true;
                                    }
                                }
                            }
                        }
                    }
                });
                Buffer.BlockCopy(pixels, 0, temp, 0, pixels.Length); //copying pixels to temp
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
