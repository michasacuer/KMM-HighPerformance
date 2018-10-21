using System.Drawing;
using System.Windows.Media.Imaging;

namespace KMM_HighPerformance.Models 
{
    public class Bitmaps
    {
        public string Filepath { get; set; }
        
        public Bitmap BinarizeLPImage { get; set; }
        public Bitmap BinarizeHPImage { get; set; }
        
        public BitmapImage BinarizeLPImageView { get; set; }
        public BitmapImage BinarizeHPImageView { get; set; }
        
        public BitmapImage KMMLP { get; set; }
        public BitmapImage KMMHP { get; set; }
        
        public long TimeElapsedLP { get; set; }
        public long TimeElapsedHP { get; set; }
        public long TimeElapsedHPTicks { get; set; }
    }
}
