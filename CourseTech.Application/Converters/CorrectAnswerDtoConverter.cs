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
    public class CorrectAnswerDtoConverter : JsonConverter<ICorrectAnswerDto>
    {
        public override ICorrectAnswerDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ICorrectAnswerDto value, JsonSerializerOptions options)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            writer.WriteStartObject();

            writer.WriteNumber(nameof(value.Id), value.Id);
            writer.WriteString(nameof(value.CorrectAnswer), value.CorrectAnswer);
            writer.WriteBoolean(nameof(value.AnswerCorrectness), value.AnswerCorrectness);

            if (value is PracticalQuestionCorrectAnswerDto correctAnswer)
            {
                WritePracticalQuestionProperties(writer, correctAnswer, options);
            }

            writer.WriteEndObject();
        }

        private void WritePracticalQuestionProperties(Utf8JsonWriter writer, PracticalQuestionCorrectAnswerDto correctAnswer, JsonSerializerOptions options)
        {
            writer.WritePropertyName(nameof(correctAnswer.QuestionUserGrade));
            JsonSerializer.Serialize(writer, correctAnswer.QuestionUserGrade, options);

            writer.WritePropertyName(nameof(correctAnswer.Remarks));
            JsonSerializer.Serialize(writer, correctAnswer.Remarks, options);

            writer.WritePropertyName(nameof(correctAnswer.QueryResult));
            SerializeQueryResult(writer, correctAnswer.QueryResult, options);
        }

        private void SerializeQueryResult(Utf8JsonWriter writer, DataTable dataTable, JsonSerializerOptions options)
        {
            if (dataTable != null)
            {
                var dataTableJsonFormat = dataTable.AsEnumerable()
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
    }
}
