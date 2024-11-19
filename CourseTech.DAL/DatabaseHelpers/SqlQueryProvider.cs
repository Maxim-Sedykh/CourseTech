using CourseTech.Domain.Interfaces.Databases;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CourseTech.Domain.Helpers
{
    public class SqlQueryProvider(IConfiguration config) : ISqlQueryProvider
    {
        /// <inheritdoc/>
        public async Task<DataTable> ExecuteQueryAsync(string sqlQuery)
        {
            var connectionString = config.GetConnectionString("FilmDbConnection");
            DataTable table = new();

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sqlQuery, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        table.Columns.Add(reader.GetName(i));
                    }

                    while (await reader.ReadAsync())
                    {
                        var row = table.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            row[i] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                        }
                        table.Rows.Add(row);
                    }
                }
            }

            return new DataTable();
        }
    }
}
