using SimpleWebApplicationForKafka.Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/app/", () => "Hello World!");

app.MapGet("/app/PlaceOrder", () =>
{
    var orderGenerator = new RandomOrderGenerator();
    Task.Run(orderGenerator.GenerateDataAsync);
    return $"Successfully generated an order at {DateTime.Now}";
});

app.Run();