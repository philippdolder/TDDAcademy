namespace TddAcademy
{
    using System;

    public class InstrumentProcessor : IInstrumentProcessor
    {
        private readonly ITaskDispatcher dispatcher;
        private readonly IInstrument instrument;
        private readonly IConsole console;
        private string currentTask;

        public InstrumentProcessor(ITaskDispatcher dispatcher, IInstrument instrument, IConsole console)
        {
            this.dispatcher = dispatcher;
            this.instrument = instrument;
            this.console = console;
        }

        public void Process()
        {
            this.SubscribeToInstrumentEvents();

            this.currentTask = this.dispatcher.GetTask();
            this.instrument.Execute(this.currentTask);
        }

        private void SubscribeToInstrumentEvents()
        {
            this.instrument.Finished += this.HandleInstrumentFinished;
            this.instrument.Error += this.HandleInstrumentError;
        }

        private void HandleInstrumentError(object sender, EventArgs e)
        {
            this.console.Log("Error occurred");
        }

        private void HandleInstrumentFinished(object sender, EventArgs e)
        {
            this.dispatcher.FinishedTask(this.currentTask);
        }
    }
}