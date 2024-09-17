namespace CourseTech.Domain.Dto.Auth
{
    /// <summary>
    /// DTO для авторизации пользователя
    /// </summary>
    public class LoginUserDto
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
