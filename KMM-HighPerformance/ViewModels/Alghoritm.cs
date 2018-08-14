using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace KMM_HighPerformance.ViewModels
{
    class Alghoritm
    {

        private Bitmap CreateNonIndexedImage(Bitmap bmp)
        {
            Bitmap newBmp = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(newBmp))
            {
                graphics.DrawImage(bmp, 0, 0);
            }

            return newBmp;
        }

        private int OtsuValue(Bitmap tempBmp)
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

        private Bitmap BinarizationLowPerformance(Bitmap tempBmp)
        {
            Bitmap newBmp = CreateNonIndexedImage(tempBmp);

            int threshold = OtsuValue(tempBmp);

            int pixelValue;

            for (int y = 0; y < tempBmp.Height; y++)
            {
                for(int x = 0; x < tempBmp.Width; x++)
                {
                    Color color = tempBmp.GetPixel(x, y);
                    pixelValue = (color.R + color.G + color.B) / 3;

                    if(pixelValue < threshold)
                    {
                        pixelValue = 0;
                    }

                    else
                    {
                        pixelValue = 255;
                    }

                    Color newColor = Color.FromArgb(pixelValue);
                    newBmp.SetPixel(x, y, newColor);
                }
            }

            return newBmp;
        }

        private Bitmap BinarizationHighPerformance(Bitmap tempBmp)
        {

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
                        offset[x] = offset[x] > threshold ? Byte.MinValue : Byte.MaxValue;
                        offset[x + 1] = offset[x + 1] > threshold ? Byte.MinValue : Byte.MaxValue;
                        offset[x + 2] = offset[x + 2] > threshold ? Byte.MinValue : Byte.MaxValue;

                        if (pixelBPP == 4)
                        {
                            offset[x + 3] = 255;
                        }
                    }

                });

                tempBmp.UnlockBits(bmpData);
            }

            return tempBmp;
        }


    }
}
