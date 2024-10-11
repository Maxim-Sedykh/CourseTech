using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using CourseTech.Domain.Dto.Question.Pass;
using System.Data;

namespace CourseTech.Application.Converters
{
    // To Do https://stackoverflow.com/questions/74827249/json-polymorphic-serialization-in-net7-web-api
    public class CorrectAnswerDtoConverter : JsonConverter<ICorrectAnswerDto>
    {
        public override ICorrectAnswerDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ICorrectAnswerDto value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteNumber("Id", value.Id);
            writer.WriteString("CorrectAnswer", value.CorrectAnswer);
            writer.WriteBoolean("AnswerCorrectness", value.AnswerCorrectness);

            if (value is PracticalQuestionCorrectAnswerDto correctAnswer)
            {
                writer.WritePropertyName("QuestionUserGrade");
                JsonSerializer.Serialize(writer, correctAnswer.QuestionUserGrade, options);

                writer.WritePropertyName("Remarks");
                JsonSerializer.Serialize(writer, correctAnswer.Remarks, options);

                var dataTable = correctAnswer.QueryResult;

                writer.WritePropertyName("QueryResult");

                if (dataTable != null)
                {
                    var dataTableJsonFormat = dataTable
                    .AsEnumerable()
                    .Select(row => dataTable.Columns.Cast<DataColumn>()
                    .ToDictionary(col => col.ColumnName, col => row[col]))
                    .ToList();

                    JsonSerializer.Serialize(writer, dataTableJsonFormat, options);
                }
                else
                {
                    JsonSerializer.Serialize(writer, new List<Dictionary<string, object>>(), options);
                }
            }

            writer.WriteEndObject();
        }
    }
}
