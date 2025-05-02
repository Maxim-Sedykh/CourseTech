using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json.Serialization;
using System.Text.Json;
using CourseTech.Domain.Dto.Question.CorrectAnswer;

namespace CourseTech.Application.Converters;

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
        ArgumentNullException.ThrowIfNull(value);

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

    private static void WritePracticalQuestionProperties(Utf8JsonWriter writer, PracticalQuestionCorrectAnswerDto correctAnswer, JsonSerializerOptions options)
    {
        writer.WritePropertyName("questionUserGrade");
        JsonSerializer.Serialize(writer, correctAnswer.QuestionUserGrade, options);

        writer.WritePropertyName("chatGptAnalysis");
        JsonSerializer.Serialize(writer, correctAnswer.ChatGptAnalysis, options);

        writer.WritePropertyName("correctQueryTime");
        JsonSerializer.Serialize(writer, correctAnswer.CorrectQueryTime, options);

        writer.WritePropertyName("userQueryTime");
        JsonSerializer.Serialize(writer, correctAnswer.UserQueryTime, options);
    }
}
