using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Question
{
    public class OpenQuestionUserAnswerValidator : AbstractValidator<OpenQuestionUserAnswerDto>
    {
        public OpenQuestionUserAnswerValidator()
        {
            RuleFor(x => x.UserAnswer)
            .NotEmpty().WithMessage("Вы не ответили на вопрос");
        }
    }
}
