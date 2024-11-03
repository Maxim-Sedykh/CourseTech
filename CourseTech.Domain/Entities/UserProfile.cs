using CourseTech.Domain.Interfaces.Entities;

namespace CourseTech.Domain.Entities;

/// <summary>
/// Профиль пользователя, для хранения различной информации о пользователе.
/// И его состояния прохождения курса.
/// </summary>
public class UserProfile : IEntityId<long>, IAuditable
{
    public long Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string Surname { get; set; }

    /// <summary>
    /// Возраст пользователя.
    /// Вычисляется автоматически исходя из даты рождения.
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Дата рождения пользователя.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Завершил ли курс пользователь.
    /// </summary>
    public bool IsExamCompleted { get; set; }

    /// <summary>
    /// Текущая оценка пользователя по курсу.
    /// </summary>
    public float CurrentGrade { get; set; }

    /// <summary>
    /// Количество пройденных уроков пользователем.
    /// </summary>
    public int LessonsCompleted { get; set; }

    /// <summary>
    /// Анализ прохождения курса пользователем.
    /// Если курс не пройден, то ставится значение по умолчанию - 
    /// Значение переменной NotReceivedYet константы AnalysParts.
    /// </summary>
    public string Analys { get; set; }

    /// <summary>
    /// Разрешается ли пользователю изменять свои данные.
    /// И разрешается ли ему оставлять отзывы.
    /// </summary>
    public bool IsEditAble { get; set; }

    /// <summary>
    /// Сколько отзывов сделал пользователь.
    /// </summary>
    public byte CountOfReviews { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Пользователь.
    /// </summary>
    public User User { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
