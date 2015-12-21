using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse
{
    public class Clock : Device, IClock
    {
        // Fields
        private long delta = -DateTime.Now.TimeOfDay.Ticks;


        // Constructors
        public Clock() { }
        public Clock(string name)
            : base(name) { }
        public Clock(string name, DateTime time)
            : this(name)
        {
            CurrentTime = time;
        }


        // Properties
        [NotMapped] 
        public DateTime CurrentTime
        {
            get
            {
                return DateTime.Now + new TimeSpan(delta);
            }
            set
            {
                delta = (new TimeSpan(value.Hour, value.Minute, value.Second) - DateTime.Now.TimeOfDay).Ticks;
            }
        }

        public long Delta
        {
            get { return delta; }
            set { delta = value; }
        }
    }
}
