using System.Diagnostics;
using System.Drawing;
using System.Windows.Media.Imaging;
using KMM_HighPerformance.Conversions;
using KMM_HighPerformance.Functions.AlgorithmsHelpers;
using KMM_HighPerformance.Models;

namespace KMM_HighPerformance.Algorithms
{
    static class KMMLowPerformanceMain
    {
        static public BitmapImage Init(Bitmap newImage, MeasureTime measure)
        {
            var stopwatch = Stopwatch.StartNew();
            int compareSize = 3; //size of compare table
            int x, y;
            Color tempPixel;
            int[,] pixelArray = new int[newImage.Height, newImage.Width]; // one record on this array = one pixel
            int N = 2;

            for (y = 1; y < newImage.Height; y++)
                for (x = 1; x < newImage.Width; x++)
                {
                    tempPixel = newImage.GetPixel(x, y);
                    if (tempPixel.R < 100) //if color of pixel is black = 1
                        pixelArray[y, x] = 1;
                    else
                        pixelArray[y, x] = 0; //if color of pixel is white = 0
                }

            deletion = 1;
            while (deletion != 0)
            {
                deletion = 0;

                for (y = 1; y < newImage.Height - 1; y++)
                {
                    for (x = 1; x < newImage.Width - 1; x++)
                        if (pixelArray[y, x] == 1)
                            pixelArray[y, x] = FindPixelsValue(pixelArray, compareSize, x, y); //we are looking edges of image here
                }

                for (y = 1; y < newImage.Height - 1; y++)
                {
                    for (x = 1; x < newImage.Width - 1; x++)
                        if (pixelArray[y, x] == 2)
                            pixelArray[y, x] = CheckNeighbourhood(pixelArray, compareSize, x, y); //we are looking "4" here
                }

                while (N <= 3)
                {
                    for (y = 0; y < newImage.Height - 1; y++)
                    {
                        for (x = 0; x < newImage.Width - 1; x++)
                            if (pixelArray[y, x] == N)
                                pixelArray[y, x] = CheckNeighbourhoodToDelete(pixelArray, compareSize, x, y);
                        //deleting all "2" and "3" with neighbourhood compare to deleteTable
                    }
                    N++;
                }  
                N = 2;
            }

            for (y = 0; y < newImage.Height - 1; y++)
            {
                for (x = 0; x < newImage.Width - 1; x++)
                {
                    if (pixelArray[y, x] == 1)
                        newImage.SetPixel(x, y, Color.Black); //printing new bitmap
                    else
                        newImage.SetPixel(x, y, Color.White);
                }
            }

            measure.SumTimeElapsed(stopwatch.ElapsedMilliseconds);
            return BitmapConversion.Bitmap2BitmapImage(newImage);
        }

        static private int FindPixelsValue(int[,] pixelArray, int compareSize, int x, int y)
        {
            int yArray, xArray, maskY, maskX;
            int tempReturn = 1;
            int checkStick = 0;
            int checkClose = 0;

            for (maskY = 0; maskY < compareSize; maskY++)
            {
                for (maskX = 0; maskX < compareSize; maskX++)
                {
                    yArray = (y + maskY - 1);
                    xArray = (x + maskX - 1);

                    if ((maskY == 0 && maskX == 0) || (maskY == 0 && maskX == 2) || (maskY == 2 && maskX == 0) || (maskY == 2 && maskX == 2))
                    {
                        if (pixelArray[yArray, xArray] == 0)
                        {
                            checkStick = 1;
                        }
                    }
                    else if (pixelArray[yArray, xArray] == 0)
                        checkClose = 1;
                }
            }

            if (checkStick == 1 && checkClose == 0)
                tempReturn = 3;
            else
                tempReturn = 2;

            if (checkStick == 0 && checkClose == 0)
                tempReturn = 1;

            return tempReturn;
        }

        static private int CheckNeighbourhood(int[,] pixelArray, int compareSize, int x, int y)
        {
            int yArray, xArray, maskY, maskX;
            int check = 0;
            int sum = 0;

            for (maskY = 0; maskY < compareSize; maskY++)
            {
                for (maskX = 0; maskX < compareSize; maskX++)
                {
                    if (maskX == 1 && maskY == 1)
                        continue;

                    yArray = (y + maskY - 1);
                    xArray = (x + maskX - 1);

                    if (pixelArray[yArray, xArray] > 0)
                    {
                        check++; //counting neighbours for that pixel
                        sum += compareTable[maskY, maskX]; //summary according to compareTable
                    }
                }
            }

            if (check == 2 || check == 3 || check == 4)
            {
                if (Lists.deleteList.Contains(sum))
                {
                    deletion++;
                    return 0; // we are find "4" and setting it to "0" at the same time
                }
            }
            return 2; // if we not find any "4"
        }

        static private int CheckNeighbourhoodToDelete(int[,] pixelArray, int compareSize, int x, int y)
        {
            int yArray, xArray, maskY, maskX;
            int sum = 0;
            for (maskY = 0; maskY < compareSize; maskY++)
            {
                for (maskX = 0; maskX < compareSize; maskX++)
                {
                    yArray = (y + maskY - 1);
                    xArray = (x + maskX - 1);

                    if (pixelArray[yArray, xArray] != 0)
                    {
                        sum += compareTable[maskY, maskX]; //summary according to compareTable
                    }
                }
            }

            if (Lists.deleteList.Contains(sum))
            {
                deletion++;
                return 0; //if we find pixel to delete, we are deleting it setting it to 0
            }
            return 1;
        }

        static int deletion = 1;

        static int[,] compareTable = {
                                { 128, 1, 2 },
                                { 64, 0, 4 }, // 0 is a middle pixel, the rest are weights for the neighbourhood
                                { 32, 16, 8 } // of this pixel
                              };
    }
}
