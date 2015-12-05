namespace SmartHouse
{
    public class Fridge : Device
    {
        // Fields
        private readonly Coldstore coldstore;
        private readonly Freezer freezer;


        // Constructors
        public Fridge(string name, Coldstore coldstore, Freezer freezer) : base(name)
        {
            this.coldstore = coldstore;
            this.freezer = freezer;
        }


        // Properties
        public bool ColdstoreIsOpen
        {
            get { return coldstore.IsOpen; }
        }
        public double ColdstoreTemperature
        {
            get { return coldstore.Temperature; }
            set { coldstore.Temperature = value; }
        }

        public double ColdstoreMinTemperature
        {
            get { return coldstore.MinTemperature; }
        }

        public double ColdstoreMaxTemperature
        {
            get { return coldstore.MaxTemperature; }
        }
        public bool ColdstoreIsHighlighted
        {
            get { return coldstore.IsHighlighted; }
        }
        public double ColdstoreLampPower
        {
            get { return coldstore.LampPower; }
        }
        public double ColdstoreVolume
        {
            get { return coldstore.Volume; }
        }

        public bool FreezerIsOpen
        {
            get { return freezer.IsOpen; }
        }
        public double FreezerTemperature
        {
            get { return freezer.Temperature; }
            set { freezer.Temperature = value; }
        }

        public double FreezerMinTemperature
        {
            get { return freezer.MinTemperature; }
        }

        public double FreezerMaxTemperature
        {
            get { return freezer.MaxTemperature; }
        }
        public double FreezerVolume
        {
            get { return freezer.Volume; }
        }


        // Methods
        public override void TurnOn()
        {
            base.TurnOn();
            coldstore.TurnOn();
            freezer.TurnOn();
        }
        public override void TurnOff()
        {
            base.TurnOff();
            coldstore.TurnOff();
            freezer.TurnOff();
        }

        public void OpenColdstore()
        {
            coldstore.Open();
        }
        public void CloseColdstore()
        {
            coldstore.Close();
        }
        public void OpenFreezer()
        {
            freezer.Open();
        }
        public void CloseFreezer()
        {
            freezer.Close();
        }
    }
}
