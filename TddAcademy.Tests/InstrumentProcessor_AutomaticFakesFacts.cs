namespace TddAcademy.Tests
{
    using System;
    using FakeItEasy;
    using FluentAssertions;
    using Xunit;

    public class InstrumentProcessor_AutomaticFakesFacts
    {
        private readonly ITaskDispatcher dispatcher;
        private readonly IInstrument instrument;
        private readonly InstrumentProcessor testee;
        private readonly IConsole console;

        // TODO: Unsubscribes from instrument events after processing completed

        public InstrumentProcessor_AutomaticFakesFacts()
        {
            this.dispatcher = A.Fake<ITaskDispatcher>();
            this.instrument = A.Fake<IInstrument>();
            this.console = A.Fake<IConsole>();

            this.testee = new InstrumentProcessor(this.dispatcher, this.instrument, this.console);
        }

        [Fact]
        public void ExecutesTaskOnInstrument()
        {
            const string Task = "Task";
            A.CallTo(() => this.dispatcher.GetTask()).Returns(Task);

            this.testee.Process();

            A.CallTo(() => this.instrument.Execute(Task)).MustHaveHappened();
        }

        [Fact]
        public void ExceptionOnInstrumentIsPropagatedToCaller()
        {
            const string InvalidTask = null;
            A.CallTo(() => this.dispatcher.GetTask()).Returns(InvalidTask);
            A.CallTo(() => this.instrument.Execute(InvalidTask)).Throws<ArgumentNullException>();

            Action act = () => this.testee.Process();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void NotifesFinishedTaskAfterInstrumentFinished()
        {
            A.CallTo(() => this.dispatcher.GetTask()).Returns("Task");

            this.testee.Process();
            this.instrument.Finished += Raise.WithEmpty();

            A.CallTo(() => this.dispatcher.FinishedTask("Task")).MustHaveHappened();
        }

        [Fact]
        public void WritesInstrumentErrorToConsole()
        {
            this.testee.Process();
            this.instrument.Error += Raise.WithEmpty();

            A.CallTo(() => this.console.Log("Error occurred")).MustHaveHappened();
        }
    }
}