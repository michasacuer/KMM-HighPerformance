namespace KMM_HighPerformance.Models
{
    class Measure //class for getting elapsed time for each insatnce of binarization, algorithm etc
    {
        public long timeElapsed;
        public long SumTimeElapsed(long time) => time + timeElapsed;
        public long TimeElapsed() => timeElapsed;
    }
}
