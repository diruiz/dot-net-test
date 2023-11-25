namespace Sensor.Publisher
{
    public interface IWeatherDataPublisher
    {
        Task ProduceAsync(Weather weather);
    }
}