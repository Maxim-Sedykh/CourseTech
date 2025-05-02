using CourseTech.Domain.Interfaces.Databases;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CourseTech.DAL.DatabaseHelpers;

public class SqlQueryProvider : ISqlQueryProvider
{
    private readonly string _connectionString;

    public SqlQueryProvider(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("FilmDbConnection");
    }

    /// <inheritdoc/>
    public async Task<(List<dynamic> result, double elapsedSeconds)> ExecuteQueryAsync(string sqlQuery)
    {
        using var connection = new SqlConnection(_connectionString);

        await connection.OpenAsync();

        var regex = new Regex(@"'((?:''|[^'])*)'");
        sqlQuery = regex.Replace(sqlQuery, match => $"N'{match.Groups[1].Value}'");

        var stopwatch = new Stopwatch();

        lock(new object())
        {
            stopwatch.Restart();

            var result = connection.QueryAsync<dynamic>(sqlQuery).Result;

            stopwatch.Stop();

            return (result.ToList(), stopwatch.Elapsed.TotalSeconds);
        }
    }
}
