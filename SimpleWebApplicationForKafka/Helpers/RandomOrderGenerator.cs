using Npgsql;
using NpgsqlTypes;
using RandomNameGeneratorLibrary;

namespace SimpleWebApplicationForKafka.Helpers;

public class RandomOrderGenerator
{
    private readonly string[] _items = { "Desktop", "Monitor", "Laptop", "Phone", "TV", "Soundbar"};
    private readonly double[] _unitPrices = { 1000.00f, 600.00f, 1500.00f, 1200.00f, 2000.00f, 800.00f };
    private readonly PersonNameGenerator _personNameGenerator = new();
    
    public async Task GenerateDataAsync()
    {
        var connString = $"Host={Environment.GetEnvironmentVariable("PG_HOST")};" +
                         $"Username={Environment.GetEnvironmentVariable("PG_USER")};" +
                         $"Password={Environment.GetEnvironmentVariable("PG_PASSWORD")};" +
                         $"Database=orders";

        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(
            @"INSERT INTO orders.customer_orders 
                                (customer_name, item, quantity, unit_price)
                                values
                                (@name, @item, @quantity, @unit_price)", conn);
        var rnd = new Random();

        var i = rnd.Next(5);
        var q = rnd.Next(1, 4);

        cmd.Parameters.AddWithValue("name", 
            _personNameGenerator.GenerateRandomFirstAndLastName());
        cmd.Parameters.AddWithValue("item", _items[i]);
        cmd.Parameters.AddWithValue("quantity", NpgsqlDbType.Integer, rnd.Next(1, 4));
        cmd.Parameters.AddWithValue("unit_price", NpgsqlDbType.Double , _unitPrices[i]);
        await cmd.ExecuteNonQueryAsync();
    }
}