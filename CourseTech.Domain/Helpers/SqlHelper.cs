using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Text.RegularExpressions;

namespace CourseTech.Domain.Helpers
{
    public class SqlHelper
    {
        private static readonly IConfiguration configuration;

        static SqlHelper()
        {
        }

        public static DataTable ExecuteQuery(string sqlQuery)
        {
            var connectionString = "ConnectionStringToAnotherDatabase";
            DataTable table = new DataTable();
            List<string> prohibitedWords = new List<string> { "lessonrecords", "users", "lessons",
                "reviews", "questions", "testvariants", "userprofiles" };
            sqlQuery = Regex.Replace(sqlQuery, @"\s+", " ").ToLower();

            foreach (var word in prohibitedWords)
            {
                if (sqlQuery.Contains(word))
                {
                    return table;
                }
            }
            if (!sqlQuery.Contains("select"))
            {
                return table;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlQuery.ToLower(), connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

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

            return table;
        }
    }
}
