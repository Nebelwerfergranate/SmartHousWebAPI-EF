using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse
{
    public class Microwave : Device, IClock, ITimer, IOpenable, IBacklight, IVolume
    {
        // Fields

        private readonly Lamp backlight = new Lamp();

        private double volume;

        private bool isRunning;
        private TimeSpan remainTime;

        private bool isOpen;



        // Constructors
        public Microwave()
        {
            Clock = new Clock();
        }
        public Microwave(string name, double volume, Lamp lamp)
            : base(name)
        {
            Clock = new Clock();
            this.backlight = lamp;
            if (volume < 10)
            {
                this.volume = 10;
            }
            else
            {
                this.volume = volume;
            }
            ElapsedTime = DateTime.Now;
        }

        [NotMapped]
        public Clock Clock { get; set; }
        
        [NotMapped]
        public DateTime CurrentTime
        {
            get { return Clock.CurrentTime; }
            set { Clock.CurrentTime = value; }
        }

        public long Delta
        {
            get { return Clock.Delta; }
            set { Clock.Delta = value; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
        }

        public DateTime ElapsedTime { get; set; }

        public TimeSpan RemainTime
        {
            get
            {
                if (IsRunning)
                {
                    return ElapsedTime - DateTime.Now;
                }
                else
                {
                    return remainTime;
                }
            }
            set { remainTime = value; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
            set { isOpen = value; }
        }

        public bool IsHighlighted
        {
            get { return backlight.IsOn; }
            set { backlight.IsOn = value; }
        }
        public double LampPower
        {
            get { return backlight.Power; }
            set { backlight.Power = value; }
        }


        public double Volume
        {
            get { return volume; }
            set { volume = value; }
        }


        // Methods
        public override void TurnOn()
        {
            base.TurnOn();
            if (this.isOpen)
            {
                backlight.TurnOn();
            }
            Clock.TurnOn();
        }
        public override void TurnOff()
        {
            base.TurnOff();
            this.Stop();
            backlight.TurnOff();
            Clock.TurnOff();
        }

        public void CheckIsReady()
        {
            if (ElapsedTime <= DateTime.Now)
            {
                if (IsRunning)
                {
                    Stop();
                }
            }
        }
        public void SetTimer(TimeSpan time)
        {
            if (this.isOn)
            {
                RemainTime = time;
                if (IsRunning)
                {
                    ElapsedTime = DateTime.Now + RemainTime;
                }
            }
        }
        public void Start()
        {
            if (this.isOn && !IsOpen && RemainTime.TotalSeconds > 0)
            {
                ElapsedTime = DateTime.Now + RemainTime;
                isRunning = true;
                backlight.TurnOn();
            }
        }

        public void Pause()
        {
            if (!IsRunning)
            {
                return;
            }
            RemainTime = ElapsedTime - DateTime.Now;
            ElapsedTime = DateTime.Now;
            isRunning = false;
            if (!this.IsOpen)
            {
                backlight.TurnOff();
            }
        }

        public void Stop()
        {
            if (!IsRunning)
            {
                return;
            }
            Pause();
            RemainTime = new TimeSpan();
        }

        public void Open()
        {
            isOpen = true;
            if (this.IsOn)
            {
                backlight.TurnOn();
            }
            this.Pause();
        }
        public void Close()
        {
            isOpen = false;
            backlight.TurnOff();
        }
    }
}
