using Microsoft.Extensions.Configuration;

namespace Infraestructure.Kafka;
using Confluent.Kafka;
public class KafkaService
{
    private readonly IProducer<Null, string> _producer;
    private readonly string _topic;

    public KafkaService(IConfiguration config)
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = config["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        _topic = config["Kafka:Topic"];
    }

    public async Task ProduceAsync(string message)
    {
        await _producer.ProduceAsync(_topic, new Message<Null, string> { Value = message });
    }
}