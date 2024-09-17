using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTech.Domain.Constants.Route
{
    public static class RouteConstants
    {
        #region Аутентификация

        public const string Register = "register";

        public const string Login = "login";

        #endregion

        #region Токен

        public const string RefreshToken = "refresh-token";

        #endregion

        #region Роли

        public const string CreateRole = "create-role";

        public const string UpdateRole = "update-role";

        public const string DeleteRoleById = "delete-role/{id}";

        public const string AddRoleForUser = "add-role-for-user";

        public const string DeleteRoleForUser = "delete-role-for-user";

        public const string UpdateRoleForUser = "update-role-for-user";

        public const string GetAllRoles = "get-all-roles";

        #endregion

        public const string GetUserAnalys = "get-user-analys";

        public const string GetCouserResult = "get-lessons-for-user";




        public const string UpdateLessonLecture = "update-lesson-lecture";
        public const string GetLessonLecture = "get-lesson-lecture";
        public const string GetLessonNames = "lesson-names";
        public const string GetLessonsForUser = "get-lessons-for-user";

        public const string GetLessonsRecords = "get-lesson-records";

        public const string GetLessonQuestions = "get-lesson-questions";
        public const string PassLessonQuestions = "pass-lesson-questions";


        public const string CreateReview = "create-review";
        public const string DeleteReview = "delete-review";
        public const string GetReviews = "get-reviews";
        public const string GetUserReviews = "get-user-reviews";

        public const string GetUsers = "get-users";
        public const string DeleteUser = "delete-user";
        public const string GetUserById = "get-user";
        public const string UpdateUser = "update-user";

        public const string GetUserProfile = "get-user-profile";
        public const string UpdateUserProfile = "update-user-profile";





    }
}
