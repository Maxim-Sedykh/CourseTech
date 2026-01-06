using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Dto.Lesson.Info;
using FluentValidation;

namespace CourseTech.Application.Validations.FluentValidations.Lesson;

public class LessonLectureValidator : AbstractValidator<LessonLectureDto>
{
    /// <summary>
    /// Валидация обновления информации об уроке.
    /// </summary>
    public LessonLectureValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ValidationErrorMessages.LessonNameNotEmptyMessage)
            .MinimumLength(ValidationConstraints.LessonNameMinimumLength).WithMessage(ValidationErrorMessages.GetLessonNameMinimumLengthMessage())
            .MaximumLength(ValidationConstraints.LessonNameMaximumLength).WithMessage(ValidationErrorMessages.GetLessonNameMaximumLengthMessage());

        RuleFor(x => x.LectureMarkup).NotEmpty().WithMessage(ValidationErrorMessages.LessonMarkupNotEmpty);
    }
}
