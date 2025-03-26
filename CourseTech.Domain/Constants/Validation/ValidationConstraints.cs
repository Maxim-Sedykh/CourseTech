namespace CourseTech.Domain.Constants.Validation;

/// <summary>
/// Класс для хранения констант для ограничений сущностей и моделей в валидаторах.
/// </summary>
public class ValidationConstraints
{
    public const byte LoginMaximumLength = 20;
    public const byte LoginMinimumLength = 4;

    public const int ReviewTextMaximumLength = 1000;

    public const int LessonNameMaximumLength = 500;
    public const byte LessonNameMinimumLength = 2;

    public const int LectureMarkupMaximumLength = 10000;

    public const byte PasswordMinimumLength = 8;

    public const byte UserNameMaximumLength = 50;

    public const byte SurnameMaximumLength = 50;

    public const byte RoleNameMaximumLength = 50;
    public const byte RoleNameMinimumLength = 2;

    public const byte KeywordWordMaximumLength = 50;

    public const byte QuestionTypeNameMaximumLength = 50;

    public const int OpenQuestionAnswerMaximumLength = 500;

    public const int QuestionDisplayQuestionMaximumLength = 500;

    public const int TestVariantContentMaximumLength = 300;

    public const byte QuestionNumberMinValue = 0;
    public const byte QuestionNumberMaxValue = 100;
}
