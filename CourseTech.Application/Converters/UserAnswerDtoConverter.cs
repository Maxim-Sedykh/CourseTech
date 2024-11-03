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
            var rootElement = document.RootElement;

            string type = rootElement.GetProperty("Type").GetString();

            return type switch
            {
                nameof(OpenQuestionUserAnswerDto) => JsonSerializer.Deserialize<OpenQuestionUserAnswerDto>(rootElement.GetRawText(), options),
                nameof(PracticalQuestionUserAnswerDto) => JsonSerializer.Deserialize<PracticalQuestionUserAnswerDto>(rootElement.GetRawText(), options),
                nameof(TestQuestionUserAnswerDto) => JsonSerializer.Deserialize<TestQuestionUserAnswerDto>(rootElement.GetRawText(), options),
                _ => throw new Exception("Неизвестный тип объекта")
            };
        }

        public override void Write(Utf8JsonWriter writer, IUserAnswerDto value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Type", value.GetType().Name);
            JsonSerializer.Serialize(writer, value, options);
            writer.WriteEndObject();
        }
    }
}
