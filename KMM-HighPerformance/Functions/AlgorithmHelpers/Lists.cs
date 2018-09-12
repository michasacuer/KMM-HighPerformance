using System.Collections.Generic;

namespace KMM_HighPerformance.Functions.Algorithms
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

        public static List<int> deleteList = new List<int>(){

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

        public static List<int> compareList = new List<int>(){

                                128, 1,  2,
                                64,  4,     // i deleted middle 0, we dont need this
                                32,  16, 8

                              };
    }
}
