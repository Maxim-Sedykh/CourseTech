using CourseTech.Domain.Dto.Question.CorrectAnswer;
using CourseTech.Domain.Dto.Question.Pass;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseTech.Application.Converters;

/// <summary>
/// Конветор JSON для реализации полиморфизма для моделей правильных ответов.
/// </summary>
public class CorrectAnswerDtoConverter : JsonConverter<CorrectAnswerDtoBase>
{
    public override CorrectAnswerDtoBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, CorrectAnswerDtoBase value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(value);

        writer.WriteStartObject();

        writer.WriteNumber("id", value.Id);
        writer.WriteString("correctAnswer", value.CorrectAnswer);
        if (value is OpenQuestionCorrectAnswerDto openQuestionCorrectAnswerDto)
        {
            writer.WriteBoolean("answerCorrectness", openQuestionCorrectAnswerDto.AnswerCorrectness);
        }
        if (value is TestQuestionCorrectAnswerDto testQuestionCorrectAnswerDto)
        {
            writer.WriteBoolean("answerCorrectness", testQuestionCorrectAnswerDto.AnswerCorrectness);
        }
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
    }
}
