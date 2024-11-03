using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Constants.LearningProcess
{
    /// <summary>
    /// Константы для хранения количетсва баллов, за правильный ответ на задания разного типа.
    /// </summary>
    public static class QuestionGrades
    {
        /// <summary>
        /// Оценка пользователя, за правильный ответ на вопрос тестового типа.
        /// </summary>
        public const float TestQuestionGrade = 1;

        /// <summary>
        /// Оценка пользователя, за правильный ответ на вопрос открытого типа.
        /// </summary>
        public const float OpenQuestionGrade = 2;

        /// <summary>
        /// Оценка пользователя, за правильный ответ на вопрос практического типа. 
        /// Если результат запроса пользователя равен результату корректного запроса.
        /// Если результаты не равны, то в зависимости от правильности запроса пользователя,
        /// ему может назначаться определённая часть от этой оценки.
        /// </summary>
        public const float PracticalQuestionGrade = 5.75f;
    }
}
