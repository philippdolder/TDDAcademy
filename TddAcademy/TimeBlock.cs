namespace TddAcademy
{
    using System;

    public class TimeBlock
    {
        private TimeBlock(TimeSpan timespan)
        {
            this.TimeSpan = timespan;
        }

        public TimeSpan TimeSpan { get; }

        public static TimeBlock CreateFromHours(int hours)
        {
            return new TimeBlock(TimeSpan.FromHours(hours));
        }
    }
}