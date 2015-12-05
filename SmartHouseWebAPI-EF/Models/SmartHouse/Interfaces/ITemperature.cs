namespace SmartHouse
{
    public interface ITemperature
    {
        double Temperature { get; set; }
        double MinTemperature { get; }
        double MaxTemperature { get; }
    }
}
