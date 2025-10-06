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
        public bool Success { get; protected set; }

        /// <summary>
        /// Список ошибок
        /// </summary>
        public IReadOnlyCollection<string> Errors { get; protected set; }

        protected Result()
        {
            Success = true;
        }

        protected Result(IReadOnlyCollection<string> errors)
        {
            Success = false;
            Errors = errors;
        }

        /// <summary>
        /// Создание успехного результата
        /// </summary>
        /// <returns>Успешный результат операции</returns>
        public static Result Ok()
        {
            return new Result();
        }

        /// <summary>
        /// Создание успешного результата с данными определенного типа
        /// </summary>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="data">Данные результата</param>
        /// <returns>Успешный результат операции</returns>
        public static Result<T> Ok<T>(T data)
        {
            return new Result<T>(data);
        }

        /// <summary>
        /// Создание результата с ошибкой
        /// </summary>
        /// <param name="error">Ошибка</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result Error(string error)
        {
            return new Result([error]);
        }

        /// <summary>
        /// Создание результата с ошибкой
        /// </summary>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="error">Ошибка</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result<T> Error<T>(string error)
        {
            return new Result<T>([error]);
        }

        /// <summary>
        /// Создание результата с ошибкой из другого ошибочного результата
        /// </summary>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="error">Ошибка</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result<T> Error<T>(Result result)
        {
            if (result.Success)
            {
                throw new ArgumentException("Нельзя обернуть в ошибку успешный результат");
            }

            return new Result<T>(result.Errors);
        }

        /// <summary>
        /// Создание результата с ошибкой
        /// </summary>
        /// <param name="errors">Список ошибок</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result Error(IReadOnlyCollection<string> errors)
        {
            return new Result(errors);
        }

        /// <summary>
        /// Создание результата с ошибкой
        /// </summary>
        /// <typeparam name="T">Тип данных результата</typeparam>
        /// <param name="errors">Список ошибок</param>
        /// <returns>Результат с ошибкой</returns>
        public static Result<T> Error<T>(IReadOnlyCollection<string> errors)
        {
            return new Result<T>(errors);
        }
    }
}
