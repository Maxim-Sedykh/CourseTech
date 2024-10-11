using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseTech.Application.Converters
{
    public class UserAnswerDtoConverter : JsonConverter<IUserAnswerDto>
    {
        public override IUserAnswerDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);

            string type = document.RootElement.GetProperty("Type").GetString();
            switch (type)
            {
                case "OpenQuestionUserAnswerDto":
                    return JsonSerializer.Deserialize<OpenQuestionUserAnswerDto>(document.RootElement.GetRawText(), options);
                case "PracticalQuestionUserAnswerDto":
                    return JsonSerializer.Deserialize<PracticalQuestionUserAnswerDto>(document.RootElement.GetRawText(), options);
                case "TestQuestionUserAnswerDto":
                    return JsonSerializer.Deserialize<TestQuestionUserAnswerDto>(document.RootElement.GetRawText(), options);
                default:
                    throw new Exception("Неизвестный тип объекта");
            }
        }

        public override void Write(Utf8JsonWriter writer, IUserAnswerDto value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Type");
            writer.WriteStringValue(value.GetType().Name);

            JsonSerializer.Serialize(writer, value, options);

            writer.WriteEndObject();
        }
    }
}
