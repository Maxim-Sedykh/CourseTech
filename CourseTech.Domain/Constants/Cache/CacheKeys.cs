using System.Text.RegularExpressions;

namespace CourseTech.Domain.Constants.Cache
{
    /// <summary>
    /// Строковые константы для ключей кэша в Redis
    /// </summary>
    public class CacheKeys
    {
        #region Ключи для кэширования коллекций

        /// <summary>
        /// Отзывы
        /// </summary>
        public const string Reviews = "reviews";

        /// <summary>
        /// Названия уроков
        /// </summary>
        public const string LessonNames = "lessonNames";

        /// <summary>
        /// Пользователи (для админов и модераторов)
        /// </summary>
        public const string Users = "users";

        /// <summary>
        /// Роли
        /// </summary>
        public const string Roles = "roles";

        #endregion

        #region Ключи для отдельных элементов

        /// <summary>
        /// Анализ пользователя
        /// </summary>
        public const string UserAnalys = "userAnalys:userId:";

        /// <summary>
        /// Урок
        /// </summary>
        public const string LessonLecture = "lessonLecture:lessonId:";

        /// <summary>
        /// Записи о прохождении уроков определённым пользователем
        /// </summary>
        public const string UserLessonRecords = "userLessonRecords:userId:";

        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public const string UserProfile = "userProfile:userId:";

        /// <summary>
        /// Вопросы из практической части определённого урока
        /// </summary>
        public const string LessonQuestions = "lessonQuestions:lessonId:";

        #endregion
    }
}
