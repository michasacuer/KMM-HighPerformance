using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace KMM_HighPerformance.Models
{
    class KMMHighPerformance
    {
        static public Bitmap Init(Bitmap tempBmp, Measure measure)
        {
            var stopwatch = Stopwatch.StartNew();

            tempBmp = BitmapConversion.Create8bppGreyscaleImage(tempBmp);

            unsafe
            {
                BitmapData bmpData = tempBmp.LockBits(new Rectangle(0, 0, tempBmp.Width, tempBmp.Height), ImageLockMode.ReadWrite, tempBmp.PixelFormat);
                
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
                                List<byte> stickPixels = new List<byte>(4)
                                {
                                    pixels[positionOfPixel - bmpData.Stride + 1],
                                    pixels[positionOfPixel - bmpData.Stride - 1],
                                    pixels[positionOfPixel + bmpData.Stride - 1],
                                    pixels[positionOfPixel + bmpData.Stride + 1]
                                };

                                List<byte> closePixels = new List<byte>(4)
                                {
                                    pixels[positionOfPixel + 1],
                                    pixels[positionOfPixel - 1],
                                    pixels[positionOfPixel + bmpData.Stride],
                                    pixels[positionOfPixel - bmpData.Stride]
                                };

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

                                List<byte> stickPixels = new List<byte>(8)
                                {
                                    pixels[positionOfPixel - bmpData.Stride - 1],
                                    pixels[positionOfPixel - bmpData.Stride],
                                    pixels[positionOfPixel - bmpData.Stride + 1],
                                    pixels[positionOfPixel - 1],
                                    pixels[positionOfPixel + 1],
                                    pixels[positionOfPixel + bmpData.Stride - 1],
                                    pixels[positionOfPixel + bmpData.Stride],
                                    pixels[positionOfPixel + bmpData.Stride + 1]
                                };

                                for (int i = 0; i < stickPixels.Count; i++)
                                {
                                    if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                                    {
                                        counter++;
                                        summary += compareList[i];
                                    }
                                }

                                if (counter == 2 || counter == 3 || counter == 4)
                                {
                                    if (deleteList.Contains(summary))
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

                                    List<byte> stickPixels = new List<byte>(8)
                                    {
                                        pixels[positionOfPixel - bmpData.Stride - 1],
                                        pixels[positionOfPixel - bmpData.Stride],
                                        pixels[positionOfPixel - bmpData.Stride + 1],
                                        pixels[positionOfPixel - 1],
                                        pixels[positionOfPixel + 1],
                                        pixels[positionOfPixel + bmpData.Stride - 1],
                                        pixels[positionOfPixel + bmpData.Stride],
                                        pixels[positionOfPixel + bmpData.Stride + 1]
                                    };

                                    for (int i = 0; i < stickPixels.Count; i++)
                                    {
                                        if (stickPixels[i] == one || stickPixels[i] == two || stickPixels[i] == three)
                                        {
                                            summary += compareList[i];
                                        }
                                    }

                                    if (deleteList.Contains(summary))
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

        readonly static List<int> deleteList = new List<int>(){

                                            3, 5, 7, 12, 13, 14, 15, 20,
                                            21, 22, 23, 28, 29, 30, 31, 48,
                                            52, 53, 54, 55, 56, 60, 61, 62,
                                            63, 65, 67, 69, 71, 77, 79, 80,
                                            81, 83, 84, 85, 86, 87, 88, 89,
                                            91, 92, 93, 94, 95, 97, 99, 101,
                                            103, 109, 111, 112, 113, 115, 116, 117,
                                            118, 119, 120, 121, 123, 124, 125, 126,
                                            127, 131, 133, 135, 141, 143, 149, 151,
                                            157, 159, 181, 183, 189, 191, 192, 193,            //deleteTable that we are looking pixels to delete
                                            195, 197, 199, 205, 207, 208, 209, 211,
                                            212, 213, 214, 215, 216, 217, 219, 220,
                                            221, 222, 223, 224, 225, 227, 229, 231,
                                            237, 239, 240, 241, 243, 244, 245, 246,
                                            247, 248, 249, 251, 252, 253, 254, 255

                                            };

        readonly static List<int> compareList= new List<int>(){

                                128, 1,  2,
                                64,  4,     // i deleted middle 0, we dont need this
                                32,  16, 8  

                              };

        const byte zero = byte.MaxValue;
        const byte one = byte.MinValue;
        const byte two = 32;
        const byte three = 64;

        static bool CheckStickZeros(List<byte> list) => list.Contains(zero);
        static bool CheckCloseZeros(List<byte> list) => list.Contains(zero);
    }
}
