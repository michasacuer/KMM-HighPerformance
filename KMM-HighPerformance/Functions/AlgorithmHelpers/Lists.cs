using System.Collections.Generic;

namespace KMM_HighPerformance.Functions.AlgorithmHelpers
{
    static class Lists
    {
        public static List<byte> PixelsAround(byte[] pixels, int positionOfPixel, int stride) => new List<byte>(8)
        {
            pixels[positionOfPixel - stride - 1],
            pixels[positionOfPixel - stride],
            pixels[positionOfPixel - stride + 1],
            pixels[positionOfPixel - 1],
            pixels[positionOfPixel + 1],
            pixels[positionOfPixel + stride - 1],
            pixels[positionOfPixel + stride],
            pixels[positionOfPixel + stride + 1]
        };

        public static List<byte> StickToPixel(byte[] pixels, int positionOfPixel, int stride) => new List<byte>(4)
        {
            pixels[positionOfPixel - stride + 1],
            pixels[positionOfPixel - stride - 1],
            pixels[positionOfPixel + stride - 1],
            pixels[positionOfPixel + stride + 1]
        };

        public static List<byte> CloseToPixel(byte[] pixels, int positionOfPixel, int stride) => new List<byte>(4)
        {
            pixels[positionOfPixel + 1],
            pixels[positionOfPixel - 1],
            pixels[positionOfPixel + stride],
            pixels[positionOfPixel - stride]
        };

        public static readonly HashSet<int> deleteList = new HashSet<int>(){

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

        public static readonly HashSet<int> compareList = new HashSet<int>(){

                         128, 1,  2,
                         64,  4,     // i deleted middle 0, we dont need this
                         32,  16, 8
                         
                                   };

        public static readonly List<int> CompareList = new List<int>(){

                         128, 1,  2,
                         64,  4,     // i deleted middle 0, we dont need this
                         32,  16, 8

                                   };

        public static readonly int[,] compareTable = {

                       { 128, 1, 2 },
                       { 64, 0, 4 }, // 0 is a middle pixel, the rest are weights for the neighbourhood
                       { 32, 16, 8 } // of this pixel
                       
                                   };

        public static readonly HashSet<int> Fours = new HashSet<int>(new int[] { 3, 6, 12, 24, 48, 96, 192, 129, 7, 14, 28, 56, 112, 224, 193, 131, 15, 30, 60, 120, 240, 225, 195, 135 });

        public static readonly HashSet<int>[] A = new HashSet<int>[]
        {
            new HashSet<int>() { 3, 6, 7, 12, 14, 15, 24, 28, 30, 31, 48, 56, 60, 62, 63, 96, 112, 120, 124, 126, 127, 129, 131, 135, 143, 159, 191, 192, 193, 195, 199, 207, 223, 224, 225, 227, 231, 239, 240, 241,243, 247, 248, 249, 251, 252, 253, 254, },
            new HashSet<int>() { 7, 14, 28, 56, 112, 131, 193, 224, },
            new HashSet<int>() { 7, 14, 15, 28, 30, 56, 60, 112, 120, 131, 135, 193, 195, 224, 225, 240, },
            new HashSet<int>() { 7, 14, 15, 28, 30, 31, 56, 60, 62, 112, 120, 124, 131, 135, 143, 193, 195, 199, 224, 225, 227, 240, 241, 248, },
            new HashSet<int>() { 7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120, 124, 126, 131, 135, 143, 159, 193, 195, 199, 207, 224, 225, 227, 231, 240, 241, 243, 248, 249, 252, },
            new HashSet<int>() { 7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120, 124, 126, 131, 135, 143, 159, 191, 193, 195, 199, 207, 224, 225, 227, 231, 239, 240, 241, 243, 248, 249, 251, 252, 254, }
        };
    

        public static readonly HashSet<int> A0 = new HashSet<int>()
        {
            3, 6, 7, 12, 14, 15, 24, 28, 30, 31, 48, 56,
            60, 62, 63, 96, 112, 120, 124, 126, 127, 129,
            131, 135, 143, 159, 191, 192, 193, 195, 199,
            207, 223, 224, 225, 227, 231, 239, 240, 241,
            243, 247, 248, 249, 251, 252, 253, 254,
        };

        public static readonly HashSet<int> A1 = new HashSet<int>()
        {
            7, 14, 28, 56, 112, 131, 193, 224,
        };

        public static readonly HashSet<int> A2 = new HashSet<int>()
        {
            7, 14, 15, 28, 30, 56, 60, 112, 120, 131, 135, 193, 195, 224, 225, 240,
        };

        public static readonly HashSet<int> A3 = new HashSet<int>()
        {
            7, 14, 15, 28, 30, 31, 56, 60, 62, 112, 120, 124, 131,
            135, 143, 193, 195, 199, 224, 225, 227, 240, 241, 248,
        };

        public static readonly HashSet<int> A4 = new HashSet<int>()
        {
            7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120, 124, 126, 131, 135, 143,
            159, 193, 195, 199, 207, 224, 225, 227, 231, 240, 241, 243, 248, 249, 252,
        };

        public static readonly HashSet<int> A5 = new HashSet<int>()
        {
            7, 14, 15, 28, 30, 31, 56, 60, 62, 63, 112, 120, 124,
            126, 131, 135, 143, 159, 191, 193, 195, 199, 207, 224,
            225, 227, 231, 239, 240, 241, 243, 248, 249, 251, 252, 254,
        };

        public static readonly HashSet<int> A0pix = new HashSet<int>()
        {
            3, 6, 7, 12, 14, 15, 24, 28, 30, 31, 48, 56, 60, 62, 63,
            96, 112, 120, 124, 126, 127, 129, 131, 135, 143, 159,
            191, 192, 193, 195, 199, 207, 223, 224, 225, 227, 231,
            239, 240, 241, 243, 247, 248, 249, 251, 252, 253, 254,
        };
    }
}
