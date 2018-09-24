namespace KMM_HighPerformance.Models
{
    class MeasureTime //class for getting elapsed time for each insatnce of binarization, algorithm etc
    {
        //time elapsed in miliseconds
        public long timeElapsedMs;
        public long SumTimeElapsedMs(long ms) => timeElapsedMs = ms + timeElapsedMs;
        public long TimeElapsedMs() => timeElapsedMs;

        //time elapsed in cpu ticks
        public long timeElapsedTicks;
        public long SumTimeElapsedTicks(long ticks) => timeElapsedTicks = ticks + timeElapsedTicks;
        public long TimeElapsedTicks() => timeElapsedTicks;
    }
}
