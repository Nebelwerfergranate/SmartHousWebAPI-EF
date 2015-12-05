using System;

namespace SmartHouse
{
    public interface IClock
    {
        DateTime CurrentTime
        {
            set;
            get;
        }
    }
}
