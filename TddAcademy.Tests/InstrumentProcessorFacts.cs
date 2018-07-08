namespace TddAcademy.Tests
{
    using System;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Xunit;

    public class InstrumentProcessorFacts
    {
        private readonly InstrumentProcessor testee;
        private readonly TaskDispatcherFake dispatcher;
        private readonly InstrumentFake instrument;
        private readonly ConsoleFake console;

        public InstrumentProcessorFacts()
        {
            this.dispatcher = new TaskDispatcherFake();
            this.instrument = new InstrumentFake();
            this.console = new ConsoleFake();
            this.testee = new InstrumentProcessor(this.dispatcher, this.instrument, this.console);
        }

        [Fact]
        public async Task ExecutesTaskOnInstrument()
        {
            const string TaskName = "Task";
            this.dispatcher.SetNextTask(TaskName);

            await this.testee.Process();

            this.instrument.TaskName.Should().Be(TaskName);
        }

        [Fact]
        public void ExceptionOnInstrumentIsPropagatedToCaller()
        {
            Func<Task> act = async () => await this.testee.Process();
            
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task NotifiesFinishedTaskAfterInstrumentFinished()
        {
            const string TaskName = "Task";
            this.dispatcher.SetNextTask(TaskName);

            await this.testee.Process();

            this.dispatcher.TaskName.Should().Be(TaskName);
        }

        [Fact]
        public async Task WritesInstrumentErrorToConsole()
        {
            this.instrument.SetupInstrumentError();

            await this.testee.Process();

            this.console.Message.Should().Be("Error occurred");
        }

        private class TaskDispatcherFake : ITaskDispatcher
        {
            private string task;
            public string TaskName { get; private set; }

            public Task<string> GetTask()
            {
                return Task.FromResult(this.task);
            }

            public Task FinishedTask(string task)
            {
                this.TaskName = task;

                return Task.CompletedTask;
            }

            public void SetNextTask(string task)
            {
                this.task = task;
            }
        }

        private class InstrumentFake : IInstrument
        {
            private bool instrumentHasError;

            public Task Execute(string task)
            {
                if (this.instrumentHasError)
                {
                    return Task.FromException(new Exception());
                }

                if (task == null)
                {
                    throw new ArgumentNullException(nameof(task));
                }

                this.TaskName = task;

                return Task.CompletedTask;
            }

            public string TaskName { get; private set; }

            public void SetupInstrumentError()
            {
                this.instrumentHasError = true;
            }
        }

        private class ConsoleFake : IConsole
        {

            public Task Log(string message)
            {
                this.Message = message;
                return Task.CompletedTask;
            }

            public string Message { get; private set; }
        }
    }
}