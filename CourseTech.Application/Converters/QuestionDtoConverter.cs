using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseTech.Application.Converters
{
    public class QuestionDtoConverter : JsonConverter<IQuestionDto>
    {
        public override IQuestionDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Реализация десериализации (при необходимости)
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IQuestionDto value, JsonSerializerOptions options)
        {
            // Запись JSON
            writer.WriteStartObject();

            // Общие свойства
            writer.WriteNumber("Id", value.Id);
            writer.WriteNumber("Number", value.Number);
            writer.WriteString("DisplayQuestion", value.DisplayQuestion);

            // Специфичные свойства для TestQuestionDto
            if (value is TestQuestionDto testQuestion)
            {
                writer.WritePropertyName("TestVariants");
                JsonSerializer.Serialize(writer, testQuestion.TestVariants, options);
            }

            writer.WriteEndObject();
        }
    }
}
