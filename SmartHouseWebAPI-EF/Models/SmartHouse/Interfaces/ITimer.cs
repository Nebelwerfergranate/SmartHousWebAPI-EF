using System;

namespace SmartHouse
{
    public interface ITimer
    { 
        bool IsRunning
        {
            get;
        }

        DateTime ElapsedTime { get; }
        TimeSpan RemainTime { get; }
        
        void CheckIsReady();

        void SetTimer(TimeSpan time);
        
        void Start();
        
        void Pause();

        void Stop();
    }
}
