﻿using CourseTech.Domain.Dto.Question.Get;
using CourseTech.Domain.Interfaces.Dtos.Question;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CourseTech.Application.Converters
{
    /// <summary>
    /// Конветор JSON для реализации полиморфизма для моделей данных для отображения вопросов.
    /// </summary>
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

            if (value is TestQuestionDto testQuestion)
            {
                writer.WritePropertyName(nameof(TestQuestionDto.TestVariants));
                JsonSerializer.Serialize(writer, testQuestion.TestVariants, options);
            }

            writer.WriteEndObject();
        }
    }
}
