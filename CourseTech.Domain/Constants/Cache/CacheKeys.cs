using System.Text.RegularExpressions;

namespace CourseTech.Domain.Constants.Cache
{
    public class CacheKeys
    {
        // Ключи для кэширования коллекций

        public const string Reviews = "reviews";

        public const string LessonNames = "lessonNames";

        public const string Users = "users";

        public const string Roles = "roles";

        // Ключи для отдельных элементов

        public const string UserAnalys = "userAnalys:userId:";

        public const string LessonLecture = "lessonLecture:lessonId:";

        public const string UserLessonRecords = "userLessonRecords:userId:";

        public const string UserProfile = "userProfile:userId:";

        public const string LessonQuestions = "lessonQuestions:lessonId:";
    }
}
