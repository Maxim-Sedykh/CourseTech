using CourseTech.Domain.Constants.Validation;
using FluentValidation;

namespace CourseTech.Domain.Extensions
{
    /// <summary>
    /// Расширение для IRuleBuilderInitial, в нём вынесены часто повторяющиеся правила валидации.
    /// </summary>
    public static class RuleBuilderInitialExtension
    {
        public static IRuleBuilderOptions<T, string> ValidateLogin<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationErrorMessages.LoginNotEmptyMessage)
                .MinimumLength(ValidationConstraints.LoginMinimumLength).WithMessage(ValidationErrorMessages.GetLoginMinimumLengthMessage())
                .MaximumLength(ValidationConstraints.LoginMaximumLength).WithMessage(ValidationErrorMessages.GetLoginMaximumLengthMessage());
        }

        public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationErrorMessages.PasswordNotEmptyMessage)
                .MinimumLength(ValidationConstraints.PasswordMinimumLength).WithMessage(ValidationErrorMessages.GetPasswordMinimumLengthMessage());
        }

        public static IRuleBuilderOptions<T, string> ValidateUserName<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationErrorMessages.UserNameNotEmptyMessage)
                .MaximumLength(ValidationConstraints.UserNameMaximumLength).WithMessage(ValidationErrorMessages.GetUserNameMaximumLengthMessage());
        }

        public static IRuleBuilderOptions<T, string> ValidateUserSurname<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationErrorMessages.SurnameNotEmptyMessage)
                .MaximumLength(ValidationConstraints.SurnameMaximumLength).WithMessage(ValidationErrorMessages.GetUserNameMaximumLengthMessage());
        }

        public static IRuleBuilderOptions<T, string> ValidateRoleName<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage(ValidationErrorMessages.RoleNameNotEmptyMessage)
                .MinimumLength(ValidationConstraints.RoleNameMinimumLength).WithMessage(ValidationErrorMessages.GetRoleNameMinimumLengthMessage())
                .MaximumLength(ValidationConstraints.RoleNameMaximumLength).WithMessage(ValidationErrorMessages.GetRoleNameMaximumLengthMessage());
        }
    }
}
