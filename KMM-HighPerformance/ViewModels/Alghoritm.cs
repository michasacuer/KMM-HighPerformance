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

        private Bitmap BinarizationLowPerformance(Bitmap tempBmp,  Bitmap newImage)
        {

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
                    newImage.SetPixel(x, y, newColor);
                }
            }

            return newImage;
        }


    }
}
