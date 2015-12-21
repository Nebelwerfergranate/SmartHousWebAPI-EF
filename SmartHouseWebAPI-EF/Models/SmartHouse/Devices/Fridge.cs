using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHouse
{
    public class Fridge : Device
    {
        // Fields

        // Constructors
        public Fridge()
        {
            Coldstore = new Coldstore();
            Freezer = new Freezer();
        }
        public Fridge(string name, Coldstore coldstore, Freezer freezer)
            : base(name)
        {
            this.Coldstore = coldstore;
            this.Freezer = freezer;
        }


        // Properties
        public override bool IsOn
        {
            get { return isOn; }
            set
            {
                isOn = value;
                if (value)
                {
                    Coldstore.TurnOn();
                    Freezer.TurnOn();
                }
            }
        }

        [NotMapped]
        public Coldstore Coldstore { get; set; }

        [NotMapped]
        public Freezer Freezer { get; set; }


        public bool ColdstoreIsOpen
        {
            get { return Coldstore.IsOpen; }
            set { Coldstore.IsOpen = value; }
        }

        public double ColdstoreTemperature
        {
            get { return Coldstore.Temperature; }
            set { Coldstore.Temperature = value; }
        }
        [NotMapped]
        public double ColdstoreMinTemperature
        {
            get { return Coldstore.MinTemperature; }
        }
        [NotMapped]
        public double ColdstoreMaxTemperature
        {
            get { return Coldstore.MaxTemperature; }
        }

        public bool ColdstoreIsHighlighted
        {
            get { return Coldstore.IsHighlighted; }
            set { Coldstore.IsHighlighted = value; }
        }

        public double ColdstoreLampPower
        {
            get { return Coldstore.LampPower; }
            set { Coldstore.LampPower = value; }
        }

        public double ColdstoreVolume
        {
            get { return Coldstore.Volume; }
            set { Coldstore.Volume = value; }
        }

        public bool FreezerIsOpen
        {
            get { return Freezer.IsOpen; }
            set { Freezer.IsOpen = value; }
        }

        public double FreezerTemperature
        {
            get { return Freezer.Temperature; }
            set { Freezer.Temperature = value; }
        }
        [NotMapped]
        public double FreezerMinTemperature
        {
            get { return Freezer.MinTemperature; }
        }
        [NotMapped]
        public double FreezerMaxTemperature
        {
            get { return Freezer.MaxTemperature; }
        }

        public double FreezerVolume
        {
            get { return Freezer.Volume; }
            set { Freezer.Volume = value; }
        }


        // Methods
        public override void TurnOn()
        {
            base.TurnOn();
            Coldstore.TurnOn();
            Freezer.TurnOn();
        }
        public override void TurnOff()
        {
            base.TurnOff();
            Coldstore.TurnOff();
            Freezer.TurnOff();
        }

        public void OpenColdstore()
        {
            Coldstore.Open();
        }
        public void CloseColdstore()
        {
            Coldstore.Close();
        }
        public void OpenFreezer()
        {
            Freezer.Open();
        }
        public void CloseFreezer()
        {
            Freezer.Close();
        }
    }
}
