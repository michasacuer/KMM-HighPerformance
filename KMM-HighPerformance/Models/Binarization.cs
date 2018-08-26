using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;


namespace KMM_HighPerformance.Models
{
    class Binarization
    {
        static public Bitmap LowPerformance(Bitmap tempBmp, Bitmap newBmp, Measure measure)
        {
            var stopwatch = Stopwatch.StartNew();

            int threshold = OtsuValue(tempBmp);

            int[] pixelValue = new int[tempBmp.Width + 1];

            for (int y = 0; y < tempBmp.Height; y++)
            {
                for(int x = 0; x < tempBmp.Width; x++)
                {
                    Color color = tempBmp.GetPixel(x, y);
                    pixelValue[x] = (color.R + color.G + color.B) / 3;

                    if(pixelValue[x] < threshold)
                    {
                        pixelValue[x] = 0;
                    }

                    else
                    {
                        pixelValue[x] = 255;
                    }

                    Color newColor = Color.FromArgb(pixelValue[x], pixelValue[x], pixelValue[x]);
                    newBmp.SetPixel(x, y, newColor);
                }
            }

            measure.timeElapsed = stopwatch.ElapsedMilliseconds;
            return newBmp;
        }

        static public Bitmap HighPerformance(Bitmap tempBmp, Measure measure)
        {
            measure.timeElapsed = 0;
            var stopwatch = Stopwatch.StartNew();

            int threshold = OtsuValue(tempBmp);
            int pixelBPP = Image.GetPixelFormatSize(tempBmp.PixelFormat) / 8;

            unsafe
            {
                BitmapData bmpData = tempBmp.LockBits(new Rectangle(0, 0, tempBmp.Width, tempBmp.Height), ImageLockMode.ReadWrite, tempBmp.PixelFormat);

                byte* ptr = (byte*)bmpData.Scan0; //addres of first line

                int height = tempBmp.Height;
                int width = tempBmp.Width * pixelBPP;

                Parallel.For(0, height, y =>
                {
                    byte* offset = ptr + (y * bmpData.Stride); //set row
                    for(int x = 0; x < width; x = x + pixelBPP)
                    {

                        int value = (offset[x] + offset[x + 1] + offset[x + 2]) / 3 > threshold ? Byte.MaxValue : Byte.MinValue;
                        offset[x] = (byte)value;
                        offset[x + 1] = (byte)value;
                        offset[x + 2] = (byte)value;

                        if (pixelBPP == 4)
                        {
                            offset[x + 3] = 255;
                        }

                    }
                });

                tempBmp.UnlockBits(bmpData);
            }

            measure.timeElapsed = stopwatch.ElapsedMilliseconds;
            return tempBmp;
        }

        static private int OtsuValue(Bitmap tempBmp)
        {
            int x;
            int y;
            int[] histogram = new int[256];

            for(y = 0; y < tempBmp.Height; y++)
            {
                for(x = 0; x < tempBmp.Width; x++)
                {
                    Color color = tempBmp.GetPixel(x, y);
                    int pixelValue = (color.R + color.G + color.B) / 3;
                    histogram[pixelValue]++;
                }
            }

            int total = tempBmp.Height * tempBmp.Width;

            float summary = 0;

            for(int i = 0; i< 256; i++)
            {
                summary += i * histogram[i];
            }

            float summary2 = 0;
            x = 0;
            y = 0;

            float max = 0;
            int threshold = 0;

            for(int i = 0; i < 256; i++)
            {
                x += histogram[i];

                if (x == 0)
                {
                    continue;
                }

                y = total - x;

                if (y == 0)
                {
                    break;
                }

                summary2 += (i * histogram[i]);
                float o = summary2 / x;
                float p = (summary - summary2) / y;

                float between = x * y * (o - p) * (o - p);

                if(between > max)
                {
                    max = between;
                    threshold = i;
                }
  
            }

            return threshold;
        }

    }
}
