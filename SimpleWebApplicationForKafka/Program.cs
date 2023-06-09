using SimpleWebApplicationForKafka.Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/app/", () => "Hello World!");

app.MapGet("/app/Produce", () =>
{
    var kafkaProducer = new KafkaProducer();
    var dr = kafkaProducer.Produce().Result;
    return dr.Value;
});

app.Run();