namespace TddAcademy
{
    using System.Threading.Tasks;

    public class InstrumentProcessor : IInstrumentProcessor
    {
        private readonly ITaskDispatcher dispatcher;
        private readonly IInstrument instrument;
        private readonly IConsole console;

        public InstrumentProcessor(ITaskDispatcher dispatcher, IInstrument instrument, IConsole console)
        {
            this.dispatcher = dispatcher;
            this.instrument = instrument;
            this.console = console;
        }

        public async Task Process()
        {
            var task = await this.dispatcher.GetTask();
            await this.instrument.Execute(task).ContinueWith(async t =>
            {
                if (!t.IsFaulted)
                {
                    await this.dispatcher.FinishedTask(task);
                }
                else
                {
                    await this.console.Log("Error occurred");
                }
            });
        }
    }
}