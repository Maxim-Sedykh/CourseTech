using CourseTech.Domain.Interfaces.Dtos.Validation;

namespace CourseTech.Domain.Dto.Auth
{
    /// <summary>
    /// Модель данных для авторизации пользователя.
    /// </summary>
    public class LoginUserDto : ILoginValidation, IPasswordValidation
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
