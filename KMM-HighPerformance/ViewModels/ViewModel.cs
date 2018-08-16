using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMM_HighPerformance.Models;

namespace KMM_HighPerformance.ViewModels
{
    class ViewModel
    {
        public string filepath { get; set; }



        public string DisplayedImage
        {
            get { return filepath; }

            
        }

    }
}
