using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMM_HighPerformance.Functions.AlgorithmHelpers
{
    class LowPerformance
    {
        //static public int FindPixelsValue(int[,] pixelArray, int compareSize, int x, int y)
        //{
        //    int yArray, xArray, maskY, maskX;
        //    int tempReturn = 1;
        //    int checkStick = 0;
        //    int checkClose = 0;
        //
        //    for (maskY = 0; maskY < compareSize; maskY++)
        //    {
        //        for (maskX = 0; maskX < compareSize; maskX++)
        //        {
        //            yArray = (y + maskY - 1);
        //            xArray = (x + maskX - 1);
        //
        //            if ((maskY == 0 && maskX == 0) || (maskY == 0 && maskX == 2) || (maskY == 2 && maskX == 0) || (maskY == 2 && maskX == 2))
        //            {
        //                if (pixelArray[yArray, xArray] == 0)
        //                {
        //                    checkStick = 1;
        //                }
        //            }
        //            else if (pixelArray[yArray, xArray] == 0)
        //                checkClose = 1;
        //        }
        //    }
        //
        //    if (checkStick == 1 && checkClose == 0)
        //        tempReturn = 3;
        //    else
        //        tempReturn = 2;
        //
        //    if (checkStick == 0 && checkClose == 0)
        //        tempReturn = 1;
        //
        //    return tempReturn;
        //}
        //
        //static public (int, int[,]) CheckNeighbourhood(Bitmap newImage, int[,] pixelArray, int compareSize)
        //{
        //    int deletion = 0;
        //    int yArray, xArray, maskY, maskX;
        //    int check = 0;
        //    int sum = 0;
        //    for (int y = 1; y < newImage.Height - 1; y++)
        //    {
        //        for (int x = 1; x < newImage.Width - 1; x++)
        //        {
        //            if (pixelArray[y, x] == 1)
        //            {
        //                for (maskY = 0; maskY < compareSize; maskY++)
        //                {
        //                    for (maskX = 0; maskX < compareSize; maskX++)
        //                    {
        //                        if (maskX == 1 && maskY == 1)
        //                            continue;
        //
        //                        yArray = (y + maskY - 1);
        //                        xArray = (x + maskX - 1);
        //
        //                        if (pixelArray[yArray, xArray] > 0)
        //                        {
        //                            check++; //counting neighbours for that pixel
        //                            sum += compareTable[maskY, maskX]; //summary according to compareTable
        //                        }
        //
        //                    }
        //                }
        //                if (check == 2 || check == 3 || check == 4)
        //                {
        //                    if (Lists.deleteList.Contains(sum))
        //                    {
        //                        deletion++;
        //                        pixelArray[y, x] = 0; // we are find "4" and setting it to "0" at the same time
        //                    }
        //                }
        //                else
        //                {
        //                    pixelArray[y, x] = 2; // if we not find any "4"
        //                }
        //            }
        //            sum = 0;
        //        }
        //    }
        //    return (deletion, pixelArray);
        //}
        //
        //static public (int, int[,]) CheckNeighbourhoodToDelete(Bitmap newImage, int[,] pixelArray, int compareSize)
        //{
        //    int deletion = 0;
        //    int yArray, xArray, maskY, maskX;
        //    int sum = 0;
        //    int N = 2;
        //    while (N <= 3)
        //    {
        //        for (int y = 1; y < newImage.Height - 1; y++)
        //        {
        //            for (int x = 1; x < newImage.Width - 1; x++)
        //            {
        //                if (pixelArray[y, x] == N)
        //                {
        //                    for (maskY = 0; maskY < compareSize; maskY++)
        //                    {
        //                        for (maskX = 0; maskX < compareSize; maskX++)
        //                        {
        //                            yArray = (y + maskY - 1);
        //                            xArray = (x + maskX - 1);
        //
        //                            if (pixelArray[yArray, xArray] != 0)
        //                            {
        //                                sum += compareTable[maskY, maskX]; //summary according to compareTable
        //                            }
        //                        }
        //                    }
        //
        //                }
        //
        //                if (Lists.deleteList.Contains(sum))
        //                {
        //                    deletion++;
        //                    pixelArray[y, x] = 0; //if we find pixel to delete, we are deleting it setting it to 0
        //                }
        //                else
        //                {
        //                    pixelArray[y, x] = 1;
        //                }
        //                sum = 0;
        //            }
        //        }
        //        N++;
        //    }
        //
        //    return (deletion, pixelArray);
        //}
        //
        //static int[,] compareTable = {
        //                        { 128, 1, 2 },
        //                        { 64, 0, 4 }, // 0 is a middle pixel, the rest are weights for the neighbourhood
        //                        { 32, 16, 8 } // of this pixel
        //                      };
    }
}
