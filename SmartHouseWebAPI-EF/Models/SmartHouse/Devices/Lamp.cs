namespace SmartHouse
{
    public class Lamp : SmartHouse.Device
    {
        // Fields
        // Лампочки с нулевой мощностью не смогут подсвечивать.
        //private double power = 1;


        // Constructors
        public Lamp() { }
        public Lamp(double power)
            : base("lamp")
        {
            if (power > 1)
            {
                this.Power = power;
            }
        }


        // Properties
        //public double Power
        //{
        //    get { return power; }
        //    set { power = value; }
        //}
        public double Power { get; set; }
    }
}
