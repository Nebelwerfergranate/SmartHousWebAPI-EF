using System;

namespace SmartHouse
{
    public interface ITimer
    {
        event SmartHouse.OperationDoneDelegate OperationDone;
    
        bool IsRunning
        {
            get;
        }

        void SetTimer(TimeSpan time);

        void Start();

        void Stop();
    }
}
