namespace TddAcademy
{
    using System;
    using System.Threading.Tasks;

    public interface IInstrument
    {
        Task Execute(string task);
    }
}