using System;

namespace SmartHouse
{
    public class Oven : Device, ITemperature, IOpenable, IBacklight, ITimer, IVolume
    {
        // Fields
        private readonly Lamp backlight;

        private readonly System.Timers.Timer timer = new System.Timers.Timer();

        private readonly double volume;

        private bool isRunning;

        private double temperature = 110;
        private readonly double minTemperature = 110;
        private readonly double maxTemperature = 250;

        private bool isOpen;
        

        // Constructors
        public Oven(string name, double volume, Lamp lamp) : base(name)
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
                    if (!isOpen)
                    {
                        lamp.TurnOff();
                    }
                    ResetTimer();
                }
            };
        }


        // Events
        public event OperationDoneDelegate OperationDone;


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
        }

        public bool IsHighlighted
        {
            get { return backlight.IsOn; }
        }
        public double LampPower
        {
            get { return backlight.Power; }
        }

        public bool IsRunning
        {
            get { return isRunning; }
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


        public void SetTimer(TimeSpan time)
        {
            if (this.isOn)
            {
                int miliSeconds = time.Seconds * 1000 + time.Minutes * 60 * 1000 + time.Hours * 60 * 60 * 1000;
                if (miliSeconds > 0)
                {
                    timer.Interval = miliSeconds;
                }
            }
        }
        public void Start()
        {
            // В отличие от микроволновки духовку можно открывать.
            if (this.isOn && timer.Interval > 999)
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

        private void ResetTimer()
        {
            // Сбросить значение таймера. Нуль установить нельзя, но метод Start() проверяет, 
            // что бы установленное значение было больше 999. 
            // Установка значение в интервал от 1 до 998 предотвратит запуск.
            timer.Interval = 1;
        }
    }
}
