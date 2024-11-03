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
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, IQuestionDto value, JsonSerializerOptions options)
        {
            ArgumentNullException.ThrowIfNull(value);

            writer.WriteStartObject();

            writer.WriteNumber(nameof(value.Id), value.Id);
            writer.WriteNumber(nameof(value.Number), value.Number);
            writer.WriteString(nameof(value.DisplayQuestion), value.DisplayQuestion);

            switch (value)
            {
                case TestQuestionDto testQuestion:
                    writer.WritePropertyName(nameof(TestQuestionDto.TestVariants));
                    JsonSerializer.Serialize(writer, testQuestion.TestVariants, options);
                    break;
                default:
                    throw new NotSupportedException($"Тип вопроса {value.GetType().Name} не поддерживается.");
            }

            writer.WriteEndObject();
        }
    }
}
