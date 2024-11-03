using FluentValidation;

namespace CourseTech.Domain.Extensions
{
    /// <summary>
    /// Расширение для IRuleBuilderInitial, в нём вынесены часто повторяющиеся правила валидации.
    /// </summary>
    public static class RuleBuilderInitialExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidateLogin<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Введите логин")
                .MinimumLength(4).WithMessage("Логин должен быть не менее 4 символов")
                .MaximumLength(20).WithMessage("Логин должен быть не более 20 символов");
        }

        public static IRuleBuilderOptions<T, string> ValidatePassword<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Введите пароль")
                .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов");
        }

        public static IRuleBuilderOptions<T, string> ValidateUserName<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Введите имя")
                .Matches(@"^[А-Яа-я]+$").WithMessage("Имя должно содержать только русские буквы");
        }

        public static IRuleBuilderOptions<T, string> ValidateUserSurname<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Введите фамилию")
                .Matches(@"^[А-Яа-я]+$").WithMessage("Фамилия должна содержать только русские буквы");
        }

        public static IRuleBuilderOptions<T, string> ValidateRoleName<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Введите имя роли")
                .MinimumLength(2).WithMessage("Длина роли должна быть больше 2 символов")
                .MaximumLength(50).WithMessage("Длина роли должна быть меньше 50 символов");
        }
    }
}
