namespace TddAcademy.Tests
{
    using System;
    using FluentAssertions;
    using Xunit;

    public class InstrumentProcessor_ManualFakesFacts
    {
        private readonly TaskDispatcherFake dispatcher;
        private readonly InstrumentFake instrument;
        private readonly InstrumentProcessor testee;
        private readonly ConsoleFake console;

        // TODO: Unsubscribes from instrument events after processing completed

        public InstrumentProcessor_ManualFakesFacts()
        {
            this.dispatcher = new TaskDispatcherFake();
            this.instrument = new InstrumentFake();
            this.console = new ConsoleFake();

            this.testee = new InstrumentProcessor(this.dispatcher, this.instrument, this.console);
        }

        [Fact]
        public void ExecutesTaskOnInstrument()
        {
            const string Task = "Task";
            this.dispatcher.SetNextTask(Task);

            this.testee.Process();

            this.instrument.Task.Should().Be(Task);
        }

        [Fact]
        public void ExceptionOnInstrumentIsPropagatedToCaller()
        {
            const string InvalidTask = null;
            this.dispatcher.SetNextTask(InvalidTask);

            Action act = () => this.testee.Process();

            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void NotifesFinishedTaskAfterInstrumentFinished()
        {
            const string Task = "Task";
            this.dispatcher.SetNextTask(Task);

            this.testee.Process();
            this.instrument.RaiseFinished();

            this.dispatcher.Task.Should().Be(Task);
        }

        [Fact]
        public void WritesInstrumentErrorToConsole()
        {
            this.dispatcher.SetNextTask("Task");

            this.testee.Process();
            this.instrument.RaiseError();

            this.console.Message.Should().Be("Error occurred");
        }

        private class InstrumentFake : IInstrument
        {
            public void Execute(string task)
            {
                if (task == null)
                {
                    throw new ArgumentNullException();
                }

                this.Task = task;
            }

            public event EventHandler Finished = delegate {  };
            public event EventHandler Error = delegate {  };
            public string Task { get; private set; }

            public void RaiseFinished()
            {
                this.Finished(null, EventArgs.Empty);
            }

            public void RaiseError()
            {
                this.Error(null, EventArgs.Empty);
            }
        }

        private class TaskDispatcherFake : ITaskDispatcher
        {
            private string task;

            public string GetTask()
            {
                var task = this.task;
                this.task = null;

                return task;
            }

            public void FinishedTask(string task)
            {
                this.Task = task;
            }

            public string Task { get; private set; }

            public void SetNextTask(string task)
            {
                this.task = task;
            }
        }

        private class ConsoleFake : IConsole
        {
            public void Log(string message)
            {
                this.Message = message;
            }

            public string Message { get; private set; }
        }
    }
}