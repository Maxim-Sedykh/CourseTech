using CourseTech.Domain.Dto.Question.QuestionUserAnswer;
using CourseTech.Domain.Dto.Review;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Question
{
    public class TestQuestionUserAnswerValidator : AbstractValidator<TestQuestionUserAnswerDto>
    {
        public TestQuestionUserAnswerValidator()
        {
            RuleFor(x => x.UserAnswerNumberOfVariant)
            .NotEmpty().WithMessage("Вы не ответили на вопрос");
        }
    }
}
