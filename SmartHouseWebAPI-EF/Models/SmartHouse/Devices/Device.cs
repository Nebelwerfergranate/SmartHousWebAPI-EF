namespace SmartHouse
{
    public abstract class Device
    {
        // Fields
        protected bool isOn;


        // Constructors
        protected Device()
        {
            Name = "";
        }
        protected Device(string name)
        {
            Name = name;
        }

        // Properties
        public virtual int Id { get; set; }
        public string Name {get; set; }
        public virtual bool IsOn
        {
            get { return isOn; }
            set { isOn = value; }
        }

        // Methods
        public virtual void TurnOn()
        {
            isOn = true;
        }
        public virtual void TurnOff()
        {
            isOn = false;
        }
    }
}
