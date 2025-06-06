﻿namespace CourseTech.Domain.Enum;

/// <summary>
/// Типы вопросов (заданий), которые пользователь получает после лекционной части урока, в практической части.
/// </summary>
public enum QuestionTypes
{
    /// <summary>
    /// Тестовый тип вопроса, на который есть варианты ответа
    /// </summary>
    Test = 0,

    /// <summary>
    /// Открытый тип вопроса, на который пользователь отвечает сам, набирая ответ вручную
    /// </summary>
    Open = 1,

    /// <summary>
    /// Практический тип вопроса, для ответа на вопрос пользователь пишет запрос SQL, который посылается в учебную базу данных.
    /// И на основе результата запроса из БД, вычисляется оцена.
    /// (SQL-тренажёр для ученика).
    /// </summary>
    Practical = 2,
}
