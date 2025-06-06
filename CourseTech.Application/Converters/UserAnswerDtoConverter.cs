﻿using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseTech.Application.Converters;

/// <summary>
/// Конветор JSON для реализации полиморфизма для моделей данных для отображения ответов пользователей.
/// </summary>
public class UserAnswerDtoConverter : JsonConverter<IUserAnswerDto>
{
    public override IUserAnswerDto Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var document = JsonDocument.ParseValue(ref reader);
        var rootElement = document.RootElement;

        string type = rootElement.GetProperty("questionType").GetString();

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
        JsonSerializer.Serialize(writer, value, options);
        writer.WriteEndObject();
    }
}
