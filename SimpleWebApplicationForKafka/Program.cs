using SimpleWebApplicationForKafka.Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var orderGenerator = new RandomOrderGenerator();
Console.WriteLine("Creating database.");
Task.Run(orderGenerator.CreateDatabase).Wait();
Console.WriteLine("Database created.");

app.MapGet("/app/", () => "Hello World!");

app.MapGet("/app/PlaceOrder", () =>
{
    Task.Run(orderGenerator.GenerateDataAsync).Wait();
    return $"Successfully generated an order at {DateTime.Now}";
});

app.Run();
