using Confluent.Kafka;
using RandomNameGeneratorLibrary;

namespace SimpleWebApplicationForKafka.Helpers;

public class KafkaProducer
{
    private readonly PersonNameGenerator _personNameGenerator = new();

    public async Task<DeliveryResult<string, string>> Produce()
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "my-cluster-kafka-bootstrap.kafka:9092"
        };
        using var producer = new ProducerBuilder<string, string>(config).Build();
        try
        {
            var name = _personNameGenerator.GenerateRandomFirstAndLastName();
            var dr = 
                await producer.ProduceAsync("my-topic", 
                    new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(), 
                        Value= $"{{ \"Name\": \"{name}\"}}"
                    }, new CancellationToken(false));
            return dr;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}