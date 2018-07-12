namespace TddAcademy.Tests
{
    using FluentAssertions;
    using FluentAssertions.Extensions;
    using Xunit;

    public class AwsomeTimeTrackerTest
    {
        [Fact]
        public void ReturnsAggregatedWorkingHours()
        {
            const string UserName = "user name";
            var testee = new AwsomeTimeTracker();
            
            testee.BookTimeForUser(UserName,TimeBlock.CreateFromHours(6));
            testee.BookTimeForUser(UserName,TimeBlock.CreateFromHours(1));
            testee.BookTimeForUser(UserName,TimeBlock.CreateFromHours(8));
            var workingHours = testee.GetWorkingHours(UserName);
            
            workingHours.Should().Be((6 + 1 + 8).Hours());
        }

        [Fact]
        public void ReturnsZeroWorkingHoursForUser_WhenUserHasNoBookings()
        {
            const string UserName = "lazy user";
            var testee = new AwsomeTimeTracker();

            var workingHours = testee.GetWorkingHours(UserName);

            workingHours.Should().Be(0.Hours());
        }

        [Fact]
        public void ReturnsWorkingHoursForUser_WhenMultipleUsersBooked()
        {
            const string UserName = "userName";
            const string AnotherUserName = "anotherUser";            
            var testee = new AwsomeTimeTracker();
            const int BookedHours = 6;
            
            testee.BookTimeForUser(UserName,TimeBlock.CreateFromHours(BookedHours));
            testee.BookTimeForUser(AnotherUserName,TimeBlock.CreateFromHours(1));
            var workingHours = testee.GetWorkingHours(UserName);
            
            workingHours.Should().Be(BookedHours.Hours());
        }
    }
}