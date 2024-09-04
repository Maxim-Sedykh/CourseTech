using CourseTech.Domain.Dto.Lesson.LessonInfo;
using CourseTech.Domain.Dto.Lesson.Practice;
using CourseTech.Domain.Dto.Lesson.Test;
using CourseTech.Domain.Dto.UserProfile;
using CourseTech.Domain.Interfaces.Dtos.Question;
using CourseTech.Domain.Result;

namespace CourseTech.Domain.Interfaces.Services
{
    /// <summary>
    /// Сервис для работы с уроками
    /// </summary>
    public interface ILessonService
    {
        /// <summary>
        /// Получение урока с разметкой лекции по его идентификатору
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        Task<BaseResult<LessonLectureDto>> GetLessonLectureAsync(int lessonId);

        /// <summary>
        /// Получение информации про урок с вопросами для тестирования по идентификатору урока
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
        Task<BaseResult<PracticeCorrectAnswersDto>> PassLessonAsync(PracticeUserAnswersDto dto, Guid userId);

        /// <summary>
        /// Обновление урока с разметкой лекции
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<LessonLectureDto>> UpdateLessonLectureAsync(LessonLectureDto dto);

        /// <summary>
        /// Получение названий уроков
        /// </summary>
        /// <returns></returns>
        Task<CollectionResult<LessonNameDto>> GetLessonNamesAsync();

        /// <summary>
        /// Получение списка уроков и количества пройденных уроков 
        /// для пользователя по его идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<BaseResult<UserLessonsDto>> GetLessonsForUserAsync(Guid userId);
    }
}
