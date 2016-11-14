namespace TddAcademy
{
    using System.Threading.Tasks;

    public interface IConsole
    {
        Task Log(string message);
    }
}