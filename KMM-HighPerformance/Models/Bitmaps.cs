using System.Drawing;
using System.Windows.Media.Imaging;

namespace KMM_HighPerformance.Models 
{
    static class Bitmaps
    {

        static public string Filepath { get; set; }
               
        static public Bitmap BinarizeLPImage { get; set; }
        static public Bitmap BinarizeHPImage { get; set; }
               
        static public BitmapImage BinarizeLPImageView { get; set; }
        static public BitmapImage BinarizeHPImageView { get; set; }
               
        static public BitmapImage KMMLP { get; set; }
        static public BitmapImage KMMHP { get; set; }
               
        static public long TimeElapsedLP { get; set; }
        static public long TimeElapsedHP { get; set; }
        static public long TimeElapsedHPTicks { get; set; }
    }
}
