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
    /// <summary>
    /// Конветор JSON для реализации полиморфизма для моделей правильных ответов.
    /// </summary>
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

            writer.WriteNumber("id", value.Id);
            writer.WriteString("correctAnswer", value.CorrectAnswer);
            writer.WriteBoolean("answerCorrectness", value.AnswerCorrectness);
            writer.WriteString("questionType", value.QuestionType);

            if (value is PracticalQuestionCorrectAnswerDto correctAnswer)
            {
                WritePracticalQuestionProperties(writer, correctAnswer, options);
            }

            writer.WriteEndObject();
        }

        private void WritePracticalQuestionProperties(Utf8JsonWriter writer, PracticalQuestionCorrectAnswerDto correctAnswer, JsonSerializerOptions options)
        {
            writer.WritePropertyName("questionUserGrade");
            JsonSerializer.Serialize(writer, correctAnswer.QuestionUserGrade, options);

            writer.WritePropertyName("userQueryAnalys");
            JsonSerializer.Serialize(writer, correctAnswer.UserQueryAnalys, options);

            writer.WritePropertyName("queryResult");
            SerializeQueryResult(writer, correctAnswer.QueryResult, options);
        }

        private void SerializeQueryResult(Utf8JsonWriter writer, List<dynamic> dynamicLists, JsonSerializerOptions options)
        {
            if (dynamicLists != null)
            {
                var dynamicListJsonFormat = JsonSerializer.Serialize(dynamicLists, options);

                JsonSerializer.Serialize(writer, dynamicListJsonFormat, options);
            }
            else
            {
                JsonSerializer.Serialize(writer, new List<Dictionary<string, object>>(), options);
            }
        }
    }
}
