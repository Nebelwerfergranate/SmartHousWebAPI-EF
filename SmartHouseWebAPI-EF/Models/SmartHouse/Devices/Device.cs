namespace SmartHouse
{
    public abstract class Device
    {
        // Fields
        protected bool isOn;


        // Constructors
        protected Device(string name)
        {
            Name = name;
        }


        // Properties
        public string Name
        {get; set; }
        public bool IsOn
        {
            get { return isOn; }
        }
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
