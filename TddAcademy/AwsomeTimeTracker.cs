namespace TddAcademy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AwsomeTimeTracker
    {
        private readonly Dictionary<string, List<TimeBlock>> bookings = new Dictionary<string, List<TimeBlock>>();

        public void BookTimeForUser(string userName, TimeBlock timeBlock)
        {
            if (this.IsUserMissing(userName))
            {
                this.bookings.Add(userName, new List<TimeBlock> ());                
            }
            this.bookings[userName].Add(timeBlock);
        }

        public TimeSpan GetWorkingHours(string userName)
        {
            if (this.IsUserMissing(userName))
            {
                return TimeSpan.Zero;
            }
            return this.AggregateBookedTimeBlocks(userName);
        }

        private bool IsUserMissing(string userName)
        {
            return !this.bookings.ContainsKey(userName);
        }

        private TimeSpan AggregateBookedTimeBlocks(string userName)
        {
            return this.bookings[userName].Aggregate(TimeSpan.Zero, (result, item) => result + item.TimeSpan);
        }
    }
}