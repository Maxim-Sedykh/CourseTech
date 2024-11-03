using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Enum;

/// <summary>
/// Перечисление типов уроков.
/// </summary>
public enum LessonTypes
{
    /// <summary>
    /// Стандартный урок, включающий тестовые задания и открытые вопросы.
    /// </summary>
    Common = 0,

    /// <summary>
    /// Урок, содержащий тестовые задания, открытые вопросы и практические задания по SQL для студентов.
    /// </summary>
    WithPractical = 1,

    /// <summary>
    /// Экзамен, представляющий собой урок без лекционной части, включающий все типы заданий в увеличенном объеме.
    /// </summary>
    Exam = 2
}