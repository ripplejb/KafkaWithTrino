using Npgsql;
using NpgsqlTypes;
using RandomNameGeneratorLibrary;

namespace SimpleWebApplicationForKafka.Helpers;

public class RandomOrderGenerator
{
    private readonly string[] _items = { "Desktop", "Monitor", "Laptop", "Phone", "TV", "Soundbar"};
    private readonly Decimal[] _unitPrices = { 1000.00M, 600.00M, 1500.00M, 1200.00M, 2000.00M, 800.00M };
    private readonly PersonNameGenerator _personNameGenerator = new();

    private NpgsqlConnection GetConnection()
    {
        var connString = $"Host={Environment.GetEnvironmentVariable("PG_HOST")};" +
                         $"Username={Environment.GetEnvironmentVariable("PG_USER")};" +
                         $"Password={Environment.GetEnvironmentVariable("PG_PASSWORD")};" +
                         $"Database=postgres";
        return new NpgsqlConnection(connString);
    }
    
    public async Task CreateDatabase()
    {
        const string sql = @"
                    create schema if not exists orders;

                    create table if not exists orders.customer_orders
                    (
                        id            integer generated by default as identity,
                        customer_name varchar(100) not null,
                        item          varchar(100) not null,
                        quantity      integer      not null,
                        unit_price    money        not null
                    );

                    alter table orders.customer_orders
                        owner to postgres;
                    ";
        await using var conn = GetConnection();
        await conn.OpenAsync();

        await using var cmd = new NpgsqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();

    }
    
    public async Task GenerateDataAsync()
    {

        await using var conn = GetConnection();
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
        cmd.Parameters.AddWithValue("unit_price", NpgsqlDbType.Money , _unitPrices[i]);
        await cmd.ExecuteNonQueryAsync();
    }
    
}