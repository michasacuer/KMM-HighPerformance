using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMM_HighPerformance.Models
{
    class Measure //class for getting elapsed time for each insatnce of binarization, algorithm etc
    {
        public long timeElapsed;
        public long TimeElapsed() => timeElapsed;
    }
}
