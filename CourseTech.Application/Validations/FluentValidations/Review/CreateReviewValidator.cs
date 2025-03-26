using CourseTech.Domain.Constants.Validation;
using CourseTech.Domain.Dto.Review;
using FluentValidation;

namespace CourseTech.Application.Validations.FluentValidations.Review;

/// <summary>
/// Валидация создания отзыва пользователем.
/// </summary>
public class CreateReviewValidator : AbstractValidator<CreateReviewDto>
{
    public CreateReviewValidator()
    {
        RuleFor(review => review.ReviewText)
            .NotEmpty().WithMessage("Введите отзыв")
            .MaximumLength(ValidationConstraints.ReviewTextMaximumLength).WithMessage("Длина отзыва должна быть меньше 1000 символов");
    }
}
