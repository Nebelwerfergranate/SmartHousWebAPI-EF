namespace SmartHouse
{
    public class Freezer : FridgeModule
    {
        // Fields
        private readonly double minTemperature = -30;
        private readonly double maxTemperature = -6;

        // Constructors
        public Freezer(uint volume)
            : base("freezer", volume)
        {
            Temperature = -10;
        }


        // Properties
        public override double Temperature
        {
            get { return temperature; }
            set
            {
                if (value <= MaxTemperature && value >= MinTemperature)
                {
                    temperature = value;
                }
            }
        }
        public override double MinTemperature
        {
            get { return minTemperature; }
        }

        public override double MaxTemperature
        {
            get { return maxTemperature; }
        }
    }
}
