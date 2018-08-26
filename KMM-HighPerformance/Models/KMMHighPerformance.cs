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

            int checkStick = 0; //sticked zeros             0
            int checkClose = 0; //zeros looking like = 0 : pix : 0
                                //                          0

            var stopwatch = Stopwatch.StartNew();

            tempBmp = BitmapConversion.Create8bppGreyscaleImage(tempBmp);

            unsafe
            {
                BitmapData bmpData = tempBmp.LockBits(new Rectangle(0, 0, tempBmp.Width, tempBmp.Height), ImageLockMode.ReadWrite, tempBmp.PixelFormat);

                byte* ptr = (byte*)bmpData.Scan0; //addres of first line
                

                int bytes = bmpData.Stride * tempBmp.Height;
                byte[] pixels = new byte[bytes];
                byte[] pixelsCopy = new byte[bytes];

                Marshal.Copy((IntPtr)ptr, pixels, 0, bytes);
                Marshal.Copy((IntPtr)ptr, pixelsCopy, 0, bytes);

                int height = tempBmp.Height;
                int width = tempBmp.Width;

                Parallel.For(0, height, y => //seting 1 and 0
                {
                    int offset = y * bmpData.Stride; //row
                    for (int x = 0; x < width; x++)
                    {

                        int positionOfPixel = x + offset;
                        if(pixels[positionOfPixel] == one && x > 0 && x < width 
                                                          && y > 0 && y < height)
                        {
                            pixelsCopy[positionOfPixel] = pixels[positionOfPixel - 1]      == zero ? two : 
                            pixels[positionOfPixel + 1]                                    == zero ? two : 
                            pixels[positionOfPixel - bmpData.Stride]                       == zero ? two :
                            pixels[positionOfPixel + bmpData.Stride]                       == zero ? two :
                            pixels[positionOfPixel - bmpData.Stride + 1]                   == zero ? two :
                            pixels[positionOfPixel + bmpData.Stride - 1]                   == zero ? two :
                            pixels[positionOfPixel - bmpData.Stride - 1]                   == zero ? two :
                            pixels[positionOfPixel - bmpData.Stride + 1]                   == zero ? two : zero;
                        }

                    }
                });

                //Parallel.For(0, height, y => //setting 2s
                //{
                //    byte* offset = ptr + (y * bmpData.Stride); //set row
                //    for (int x = 0; x < width; x++)
                //    {
                //
                //        
                //
                //    }
                //});

                Marshal.Copy(pixelsCopy, 0, (IntPtr)ptr, bytes);
                tempBmp.UnlockBits(bmpData);
            }

            measure.timeElapsed = stopwatch.ElapsedMilliseconds;
            return tempBmp;
        }

        static int deletion = 1;
        static List<int> deleteTable = new List<int>(){

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

        static int[,] compareTable = {

                                { 128, 1, 2 },
                                { 64, 0, 4 }, // 0 is a middle pixel, the rest are weights for the neighbourhood
                                { 32, 16, 8 } // of this pixel

                              };

        static byte zero = byte.MaxValue;
        static byte one = byte.MinValue;
        static byte two = 32;
        static byte three = 64;
        static byte four = 128;

    }
}
