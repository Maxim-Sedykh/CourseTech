using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseTech.Application.Converters;

/// <summary>
/// Конветор JSON для реализации полиморфизма для моделей данных для отображения вопросов.
/// </summary>
public class QuestionDtoConverter : JsonConverter<QuestionDtoBase>
{
    public override QuestionDtoBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, QuestionDtoBase value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        writer.WriteNumber("id", value.Id);
        writer.WriteNumber("number", value.Number);
        writer.WriteString("displayQuestion", value.DisplayQuestion);
        writer.WriteString("questionType", value.QuestionType);

        if (value is TestQuestionDto testQuestion)
        {
            writer.WritePropertyName("testVariants");
            JsonSerializer.Serialize(writer, testQuestion.TestVariants, options);
        }

        writer.WriteEndObject();
    }
}
