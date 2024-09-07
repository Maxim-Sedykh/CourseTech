using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с вопросами после урока, практической частью
    /// </summary>
    public interface IQuestionService
    {
        /// <summary>
        /// Получение вопросов для тестирования по идентификатору урока
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        Task<BaseResult<LessonPracticeDto>> GetLessonQuestionsAsync(int lessonId);

        /// <summary>
        /// Завершение прохождения тестирования
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<PracticeCorrectAnswersDto>> PassLessonQuestionsAsync(PracticeUserAnswersDto dto, Guid userId);
    }
}
