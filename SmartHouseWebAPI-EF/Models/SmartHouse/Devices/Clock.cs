using System;

namespace SmartHouse
{
    public class Clock : Device, IClock
    {
        // Fields
        private TimeSpan delta;


        // Constructors
        public Clock(string name) : base(name)
        {
            delta = -DateTime.Now.TimeOfDay;
        }
        public Clock(string name, DateTime time)
            : this(name)
        {
            CurrentTime = time;
        }


        // Properties
        public DateTime CurrentTime
        {
            get
            {
                    return DateTime.Now + delta;
            }
            set
            {
                delta = new TimeSpan(value.Hour, value.Minute, value.Second) - DateTime.Now.TimeOfDay;
            }
        }
    }
}
