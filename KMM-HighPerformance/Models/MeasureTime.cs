namespace KMM_HighPerformance.Models
{
    class MeasureTime //class for getting elapsed time for each insatnce of binarization, algorithm etc
    {
        //time elapsed in miliseconds
        public long TimeElapsedMs { get; set; }
        public long SumTimeElapsedMs(long ms) => TimeElapsedMs = ms + TimeElapsedMs;

        //time elapsed in cpu ticks
        public long TimeElapsedTicks { get; set; }
        public long SumTimeElapsedTicks(long ticks) => TimeElapsedTicks = ticks + TimeElapsedTicks;
    }
}
