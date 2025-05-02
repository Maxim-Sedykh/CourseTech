using CourseTech.WebApi;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<TestDataGenerator>();

builder.Services.ConfigureServices(builder);

var app = builder.Build();

app.MapGet("/generate-test-data", async (TestDataGenerator generator) =>
{
    return await generator.GenerateAllData();
});

app.ConfigureMiddlewares();

app.Run();

public class TestDataGenerator
{
    private readonly string _connectionString;
    private readonly Random _random = new();

    public TestDataGenerator(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("FilmDbConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<string> GenerateAllData()
    {
        var stopwatch = Stopwatch.StartNew();
        var result = new List<string>();

        try
        {
            // 1. Очистка всех таблиц
            //result.Add(await ClearAllTables());

            // 2. Генерация данных
            //result.Add(await GenerateHalls(100000));
            //result.Add(await GenerateFilms(100000));
            //result.Add(await GenerateHallRows(100000));
            //result.Add(await GenerateScreenings(100000));
            result.Add(await GenerateTickets(59000));

            stopwatch.Stop();
            result.Add($"Генерация данных завершена за {stopwatch.Elapsed.TotalSeconds} секунд");

            return string.Join(Environment.NewLine, result);
        }
        catch (Exception ex)
        {
            return $"Ошибка: {ex.Message}";
        }
    }

    private async Task<string> ClearAllTables()
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        // Отключаем проверку внешних ключей для быстрой очистки
        await connection.ExecuteAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
        await connection.ExecuteAsync("EXEC sp_MSforeachtable 'DELETE FROM ?'");
        await connection.ExecuteAsync("EXEC sp_MSforeachtable 'ALTER TABLE ? CHECK CONSTRAINT ALL'");

        return "Таблицы очищены";
    }

    private async Task<string> GenerateHalls(int count)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        for (int i = 1; i <= count; i++)
        {
            await connection.ExecuteAsync(
                "INSERT INTO Halls (Name) VALUES (@Name);",
                new { Name = $"Зал {i}" });
        }

        return $"Сгенерировано {count} залов";
    }

    private async Task<string> GenerateFilms(int count)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        for (int i = 1; i <= count; i++)
        {
            await connection.ExecuteAsync(
                "INSERT INTO Films (Name, Description) VALUES (@Name, @Description);",
                new
                {
                    Name = $"Фильм {i}",
                    Description = $"Описание фильма {i}"
                });
        }

        return $"Сгенерировано {count} фильмов";
    }

    private async Task<string> GenerateHallRows(int count)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        for (int i = 1; i <= count; i++)
        {
            await connection.ExecuteAsync(
                "INSERT INTO HallRows (HallId, Number, Capacity) VALUES (@HallId, @Number, @Capacity);",
                new
                {
                    HallId = _random.Next(1, 100000),
                    Number = _random.Next(1, 50),
                    Capacity = _random.Next(10, 30)
                });
        }

        return $"Сгенерировано {count} рядов";
    }

    private async Task<string> GenerateScreenings(int count)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        for (int i = 1; i <= count; i++)
        {
            await connection.ExecuteAsync(
                "INSERT INTO Screenings (HallId, FilmId, Time) VALUES (@HallId, @FilmId, @Time);",
                new
                {
                    HallId = _random.Next(1, 100000),
                    FilmId = _random.Next(1, 100000),
                    Time = DateTime.Now.AddDays(_random.Next(365))
                });
        }

        return $"Сгенерировано {count} сеансов";
    }

    private async Task<string> GenerateTickets(int count)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        for (int i = 1; i <= count; i++)
        {
            await connection.ExecuteAsync(
                "INSERT INTO Tickets ([Row], Seat, Cost, ScreeningId) VALUES (@Row, @Seat, @Cost, @ScreeningId);",
                new
                {
                    Row = _random.Next(1, 50),
                    Seat = _random.Next(1, 30),
                    Cost = _random.Next(200, 1000),
                    ScreeningId = _random.Next(1, 100000)
                });
        }

        return $"Сгенерировано {count} билетов";
    }
}