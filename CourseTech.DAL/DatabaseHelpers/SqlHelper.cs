using CourseTech.Domain.Interfaces.Databases;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CourseTech.Domain.Helpers
{
    public class SqlHelper(IConfiguration config) : ISqlHelper
    {
        // To Do сделать здесь всё ассинхронным
        public DataTable ExecuteQuery(string sqlQuery)
        {
            var connectionString = config.GetConnectionString("FilmDbConnection");
            DataTable table = new DataTable();

            if (!sqlQuery.Trim().StartsWith("select", StringComparison.OrdinalIgnoreCase))
            {
                return table;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery.ToLower(), connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        table.Columns.Add(reader.GetName(i));
                    }

                    while (reader.Read())
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

            return table;
        }
    }
}
