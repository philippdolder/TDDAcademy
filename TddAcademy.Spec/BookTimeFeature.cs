namespace TddAcademy.Spec
{
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Xbehave;

    public class BookTimeFeature
    {
        [Scenario]
        public void BookTime(AwsomeTimeTracker awsomeTimeTracker, string userName, TimeBlock timeBlock)
        {
            "Given an awesome time tracker".x(() => { awsomeTimeTracker = new AwsomeTimeTracker(); });
            
            "Given a user".x(() => { userName = "Awesome User"; });

            "Given a time block".x(() =>
            {
                timeBlock = TimeBlock.CreateFromHours(10);
            });

            "When user books a time block".x(() =>
            {
                awsomeTimeTracker.BookTimeForUser(userName, timeBlock);
            });

            "Then working hours for the user are equal to the booked time".x(() =>
            {
                awsomeTimeTracker.GetWorkingHours(userName).Should().Be(10.Hours());
            });
        }
    }
}