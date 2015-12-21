using System;

namespace SmartHouse
{
    public class Oven : Device, ITemperature, IOpenable, IBacklight, ITimer, IVolume
    {
        // Fields
        private readonly Lamp backlight = new Lamp();

        private double volume;

        private bool isRunning;
        private TimeSpan remainTime;

        private double temperature = 110;
        private readonly double minTemperature = 110;
        private readonly double maxTemperature = 250;

        private bool isOpen;


        // Constructors
        public Oven() { }
        public Oven(string name, double volume, Lamp lamp)
            : base(name)
        {
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


        // Properties
        public double Temperature
        {
            get { return temperature; }
            set
            {
                if (value >= MinTemperature && value <= MaxTemperature)
                {
                    temperature = value;
                }
            }
        }

        public double MinTemperature
        {
            get { return minTemperature; }
        }

        public double MaxTemperature
        {
            get { return maxTemperature; }
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

        public DateTime ElapsedTime { get; set; }

        public bool IsRunning
        {
            get { return isRunning; }
            set { isRunning = value; }
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
        }

        public override void TurnOff()
        {
            base.TurnOff();
            this.Stop();
            backlight.TurnOff();
        }

        public void Close()
        {
            isOpen = false;
            backlight.TurnOff();
        }
        public void Open()
        {
            isOpen = true;
            if (this.IsOn)
            {
                backlight.TurnOn();
            }
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
            if (this.isOn && RemainTime.TotalSeconds > 0)
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
    }
}
