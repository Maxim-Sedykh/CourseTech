using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Question
{
    public class PracticalQuestionUserAnswerValidator : AbstractValidator<PracticalQuestionUserAnswerDto>
    {
        public PracticalQuestionUserAnswerValidator()
        {
            RuleFor(x => x.UserCodeAnswer)
            .NotEmpty().WithMessage("Вы не ответили на вопрос");
        }
    }
}
