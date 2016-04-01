﻿namespace TddAcademy
{
    using System;

    public interface IInstrument
    {
        void Execute(string task);

        event EventHandler Finished;
        event EventHandler Error;
    }
}