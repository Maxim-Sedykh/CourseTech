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
                .NotEmpty().WithMessage("Введите название урока")
                .MinimumLength(4).WithMessage("Название урока должен быть не менее 2 символов")
                .MaximumLength(20).WithMessage("Название урока должен быть не более 50 символов");

            RuleFor(x => x.LessonMarkup).NotEmpty().WithMessage("Не указана разметка лекции");
        }
    }
}
