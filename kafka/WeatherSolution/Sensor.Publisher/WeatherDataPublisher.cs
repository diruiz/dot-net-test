using Confluent.Kafka;
using Newtonsoft.Json;

namespace Sensor.Publisher;
public class WeatherDataPublisher : IWeatherDataPublisher
{
    private readonly IProducer<Null, string> producer;
    public WeatherDataPublisher(IProducer<Null, string> producer)
    {
        this.producer = producer;
    }

    public async Task ProduceAsync(Weather weather)
    {
        await producer.ProduceAsync("weather-topic", new Message<Null, string> { Value = JsonConvert.SerializeObject(weather) });
    }
}

public record Weather(string state, int temperature);


