using System.Drawing;
using System.Windows.Media.Imaging;

namespace KMM_HighPerformance.Models 
{
    static class Bitmaps
    {
        static public string Filepath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        static public Bitmap BinarizeLPImage
        {
            get { return binarizeLPImage; }
            set { binarizeLPImage = value; }
        }

        static public Bitmap BinarizeHPImage
        {
            get { return binarizeHPImage; }
            set { binarizeHPImage = value; }
        }

        static public BitmapImage BinarizeLPImageView
        {
            get { return binarizeLPImageView; }
            set { binarizeLPImageView = value; }
        }

        static public BitmapImage BinarizeHPImageView
        {
            get { return binarizeHPImageView; }
            set { binarizeHPImageView = value; }
        }

        static public BitmapImage KMMLP
        {
            get { return kMMLP; }
            set { kMMLP = value; }
        }

        static public BitmapImage KMMHP
        {
            get { return kMMHP; }
            set { kMMHP = value; }
        }

        static public long TimeElapsedLP
        {
            get { return timeElapsedLP; }
            set { timeElapsedLP = value; }
        }

        static public long TimeElapsedHP
        {
            get { return timeElapsedHP; }
            set { timeElapsedHP = value; }
        }

        static private string filepath { get; set; }

        static private Bitmap binarizeLPImage { get; set; }
        static private Bitmap binarizeHPImage { get; set; }
                
        static private BitmapImage binarizeLPImageView { get; set; }
        static private BitmapImage binarizeHPImageView { get; set; }
                
        static private BitmapImage kMMLP { get; set; }
        static private BitmapImage kMMHP { get; set; }
                
        static private long timeElapsedLP { get; set; }
        static private long timeElapsedHP { get; set; }
    }
}
