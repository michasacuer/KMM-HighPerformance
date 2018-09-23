namespace KMM_HighPerformance.Models
{
    class MeasureTime //class for getting elapsed time for each insatnce of binarization, algorithm etc
    {
        //time elapsed in miliseconds
        public long timeElapsedMs;
        public long SumTimeElapsedMs(long ms) => ms + timeElapsedMs;
        public long TimeElapsedMs() => timeElapsedMs;

        //time elapsed in cpu ticks
        public long timeElapsedTicks;
        public long SumTimeElapsedTicks(long ticks) => ticks + timeElapsedTicks;
        public long TimeElapsedTicks() => timeElapsedTicks;
    }
}
