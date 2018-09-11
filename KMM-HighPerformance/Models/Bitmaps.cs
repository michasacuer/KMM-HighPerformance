using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace KMM_HighPerformance.Models
{
    static class Bitmaps
    {
        static public string filepath { get; set; }

        static public Bitmap binarizeLPImage { get; set; }
        static public Bitmap binarizeHPImage { get; set; }

        static public BitmapImage binarizeLPImageView { get; set; }
        static public BitmapImage binarizeHPImageView { get; set; }

        static public BitmapImage kMMLP { get; set; }
        static public BitmapImage kMMHP { get; set; }

        static public long timeElapsedLP { get; set; }
        static public long timeElapsedHP { get; set; }
    }
}
