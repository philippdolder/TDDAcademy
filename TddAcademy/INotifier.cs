namespace TddAcademy
{
    using System;

    public interface INotifier
    {
        void NotifyException(Exception exception);
    }
}