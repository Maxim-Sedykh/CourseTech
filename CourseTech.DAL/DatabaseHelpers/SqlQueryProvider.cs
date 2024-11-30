using CourseTech.Domain.Interfaces.Databases;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CourseTech.DAL.DatabaseHelpers;

public class SqlQueryProvider : ISqlQueryProvider
{
    private readonly string _connectionString;

    public SqlQueryProvider(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("FilmDbConnection");
    }

    /// <inheritdoc/>
    public async Task<List<dynamic>> ExecuteQueryAsync(string sqlQuery)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<dynamic>(sqlQuery);
            return result.ToList();
        }
    }
}
