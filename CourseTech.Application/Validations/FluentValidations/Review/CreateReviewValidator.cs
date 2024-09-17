using CourseTech.Domain.Dto.Auth;
using CourseTech.Domain.Dto.Review;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Application.Validations.FluentValidations.Review
{
    public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
    {
        public CreateReviewValidator()
        {
            RuleFor(review => review.ReviewText)
            .NotEmpty().WithMessage("Введите отзыв")
            .MaximumLength(1000).WithMessage("Длина отзыва должна быть меньше 1000 символов");
        }
    }
}
