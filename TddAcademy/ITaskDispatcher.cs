namespace TddAcademy
{
    using System.Threading.Tasks;

    public interface ITaskDispatcher
    {
        Task<string> GetTask();

        Task FinishedTask(string task);
    }
}