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

        static public int[,] SetOneZero(Bitmap newImage, int[,] pixelArray)
        {
            for (int y = 1; y < newImage.Height; y++)
            {
                for (int x = 1; x < newImage.Width; x++)
                {
                    Color tempPixel = newImage.GetPixel(x, y);
                    if (tempPixel.R < 100) //if color of pixel is black = 1
                        pixelArray[y, x] = 1;
                    else
                        pixelArray[y, x] = 0; //if color of pixel is white = 0
                }
            }

            return pixelArray;
        }
        static public int[,] SetOneTwoThree(Bitmap newImage, int[,] pixelArray)
        {
            int yArray, xArray, maskY, maskX;
            int checkStick = 0;
            int checkClose = 0;

            for (int y = 1; y < newImage.Height - 1; y++)
            {
                for (int x = 1; x < newImage.Width - 1; x++)
                {
                    if (pixelArray[y, x] == 1)
                    {
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
                            pixelArray[y, x] = 3;
                        else
                            pixelArray[y, x] = 2;

                        if (checkStick == 0 && checkClose == 0)
                            pixelArray[y, x] = 1;

                        checkClose = 0;
                        checkStick = 0;
                    }
                }
            }
            return pixelArray;
        }
        
        static public (int, int[,]) FindAndDeleteFour(Bitmap newImage, int[,] pixelArray)
        {
            int deletion = 0;
            int yArray, xArray, maskY, maskX;
            int check = 0;
            int sum = 0;
            for (int y = 1; y < newImage.Height - 1; y++)
            {
                for (int x = 1; x < newImage.Width - 1; x++)
                {
                    if (pixelArray[y, x] == 2)
                    {
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
                                    sum += Lists.compareTable[maskY, maskX]; //summary according to compareTable
                                }
        
                            }
                        }
                        if (check == 2 || check == 3 || check == 4)
                        {
                            if (Lists.deleteList.Contains(sum))
                            {
                                deletion++;
                                pixelArray[y, x] = 0; // we are find "4" and setting it to "0" at the same time
                            }
                        }
                        else
                        {
                            pixelArray[y, x] = 2; // if we not find any "4"
                        }
                    }
                    check = 0;
                    sum = 0;
                }
            }
            return (deletion, pixelArray);
        }
        
        static public (int, int[,]) DeletingTwoThree(Bitmap newImage, int[,] pixelArray)
        {
            int deletion = 0;
            int yArray, xArray, maskY, maskX;
            int sum = 0;
            int N = 2;
            while (N <= 3)
            {
                for (int y = 1; y < newImage.Height - 1; y++)
                {
                    for (int x = 1; x < newImage.Width - 1; x++)
                    {
                        if (pixelArray[y, x] == N)
                        {
                            for (maskY = 0; maskY < compareSize; maskY++)
                            {
                                for (maskX = 0; maskX < compareSize; maskX++)
                                {
                                    yArray = (y + maskY - 1);
                                    xArray = (x + maskX - 1);
        
                                    if (pixelArray[yArray, xArray] != 0)
                                    {
                                        sum += Lists.compareTable[maskY, maskX]; //summary according to compareTable
                                    }
                                }
                            }
        
                            if (Lists.deleteList.Contains(sum))
                            {
                                deletion++;
                                pixelArray[y, x] = 0; //if we find pixel to delete, we are deleting it setting it to 0
                            }
                            else
                            {
                                pixelArray[y, x] = 1;
                            }
                        }
                        sum = 0;
                    }
                }
                N++;
            }
            return (deletion, pixelArray);
        }

        static public Bitmap SetImageAfterKMM(Bitmap newImage, int[,] pixelArray)
        {
            for (int y = 0; y < newImage.Height - 1; y++)
            {
                for (int x = 0; x < newImage.Width - 1; x++)
                {
                    if (pixelArray[y, x] == 1)
                        newImage.SetPixel(x, y, Color.Black); //printing new bitmap
                    else
                        newImage.SetPixel(x, y, Color.White);
                }
            }

            return newImage;
        }

        static private int compareSize = 3;
    }
}
