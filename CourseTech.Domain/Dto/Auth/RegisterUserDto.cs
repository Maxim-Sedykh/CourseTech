﻿namespace CourseTech.Domain.Dto.Auth
{
    /// <summary>
    /// DTO для регистрации пользователя
    /// </summary>
    public class RegisterUserDto
    {
        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}