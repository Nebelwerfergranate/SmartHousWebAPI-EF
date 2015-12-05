using System;

namespace SmartHouse
{
    public class Microwave : Device, IClock, ITimer, IOpenable, IBacklight, IVolume
    {
        // Fields
        private readonly Clock clock = new Clock("built-in_clock");

        private readonly Lamp backlight;

        private readonly double volume;

        private readonly System.Timers.Timer timer = new System.Timers.Timer();
        private bool isRunning;

        private bool isOpen;


        // Constructors
        public Microwave(string name, double volume, Lamp lamp)
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
            timer.AutoReset = false;
            timer.Elapsed += (sourse, eventArgs) =>
            {
                if (OperationDone != null && this.IsOn)
                {
                    OperationDone.Invoke(this);
                    isRunning = false;
                    lamp.TurnOff();
                    ResetTimer();
                }
            };
        }


        // Events
        public event OperationDoneDelegate OperationDone;


        // Properties
        public DateTime CurrentTime
        {
            get { return clock.CurrentTime; }
            set { clock.CurrentTime = value; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
        }

        public bool IsOpen
        {
            get { return isOpen; }
        }

        public bool IsHighlighted
        {
            get { return backlight.IsOn; }
        }
        public double LampPower
        {
            get { return backlight.Power; }
        }

        public double Volume
        {
            get { return volume; }
        }


        // Methods
        public override void TurnOn()
        {
            base.TurnOn();
            if (this.isOpen)
            {
                backlight.TurnOn();
            }
            clock.TurnOn();
        }
        public override void TurnOff()
        {
            base.TurnOff();
            this.Stop();
            backlight.TurnOff();
            clock.TurnOff();
        }

        public void SetTimer(TimeSpan time)
        {
            if (this.isOn)
            {
                int miliSeconds = time.Seconds * 1000 + time.Minutes * 60 * 1000;
                if (miliSeconds > 0)
                {
                    timer.Interval = miliSeconds;
                }
            }
        }
        public void Start()
        {
            if (this.isOn && !IsOpen && timer.Interval > 999)
            {
                timer.Start();
                isRunning = true;
                backlight.TurnOn();
            }
        }
        public void Stop()
        {
            timer.Stop();
            ResetTimer();
            isRunning = false;
            if (!this.IsOpen)
            {
                backlight.TurnOff();
            }
        }

        public void Open()
        {
            isOpen = true;
            if (this.IsOn)
            {
                backlight.TurnOn();
            }
            this.Stop();
        }
        public void Close()
        {
            isOpen = false;
            backlight.TurnOff();
        }

        private void ResetTimer()
        {
            // Сбросить значение таймера. Нуль установить нельзя, но метод Start() проверяет, 
            // что бы установленное значение было больше 999. 
            // Установка значение в интервал от 1 до 998 предотвратит запуск.
            timer.Interval = 1;
        }
    }
}
