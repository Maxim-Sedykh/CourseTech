namespace CourseTech.Domain.Result
{

    /// <summary>
    /// Класс для реализации паттерна Result Pattern.
    /// Абстракция, которая представляет собой результат выполнения операции
    /// </summary>
    public class BaseResult
    {
        protected BaseResult(string message = null)
        {
            Message = message ?? "Unknown error";
        }

        public bool IsSuccess => Message == null;

        public string Message { get; }

        public static BaseResult Success() => new();

        public static BaseResult Failure(string message) =>
            new(message);
    }
}
