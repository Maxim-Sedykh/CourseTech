using CourseTech.Domain.Constants.Validation;

namespace CourseTech.Domain.Constants
{
    public class ErrorMessages
    {
        public static string GetUserNotFoundMessage(Guid userId) =>
        $"User with id {userId} not found.";
    }
}
