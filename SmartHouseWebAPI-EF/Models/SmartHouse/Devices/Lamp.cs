namespace SmartHouse
{
    public class Lamp : SmartHouse.Device
    {
        // Fields
        // Лампочки с нулевой мощностью не смогут подсвечивать.
        private readonly double power = 1;


        // Constructors
        public Lamp(double power)
            : base("lamp")
        {
            if (power > 1)
            {
                this.power = power;
            }
        }


        // Properties
        public double Power
        {
            get { return power; }
        }
    }
}
