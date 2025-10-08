namespace CourseTech.Domain
{
    /// <summary>
    /// Результат операции
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Успешна ли операция
        /// </summary>
        public bool IsSuccess { get; protected set; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IReadOnlyCollection<string> Errors { get; protected set; }

        protected Result()
        {
            IsSuccess = true;
        }

        protected Result(IReadOnlyCollection<string> errors)
        {
            IsSuccess = false;
            Errors = errors;
        }

        /// <summary>
        /// Создание успешного результата
        /// </summary>
        /// <returns>Успешный результат операции</returns>
        public static Result Success()
        {
            return new Result();
        }

        /// <summary>
        /// Создание успешного результата с данными определенного типа
        /// </summary>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="data">Данные результата</param>
        /// <returns>Успешный результат операции</returns>
        public static Result<T> Success<T>(T data)
        {
            return new Result<T>(data);
        }

        /// <summary>
        /// Создание результата с ошибкой
        /// </summary>
        /// <param name="error">Ошибка</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result Failure(string error)
        {
            return new Result([error]);
        }

        /// <summary>
        /// Создание результата с ошибкой
        /// </summary>
        /// <param name="errors">Список ошибок</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result Failure(IReadOnlyCollection<string> errors)
        {
            return new Result(errors);
        }

        /// <summary>
        /// Создание результата с ошибкой
        /// </summary>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="errors">Список ошибок</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result<T> Failure<T>(IReadOnlyCollection<string> errors)
        {
            return new Result<T>(errors);
        }
    }
}
