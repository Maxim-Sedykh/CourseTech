using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Lesson.LessonInfo;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Lesson
{
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

            RuleFor(x => x.LessonMarkup).NotEmpty().WithMessage(ValidationErrorMessages.LessonMarkupNotEmpty);
        }
    }
}
