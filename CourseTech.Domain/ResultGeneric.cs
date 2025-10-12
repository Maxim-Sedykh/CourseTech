namespace CourseTech.Domain;

/// <summary>
/// Результат операции с данными
/// </summary>
/// <typeparam name="T">Тип данных результата</typeparam>
public class Result<T> : Result
{
    private T _data;
    public Result(IReadOnlyCollection<string> errors) : base(errors) { }

    public Result(T data) : base()
    {
        Data = data;
    }

    /// <summary>
    /// Данные результата
    /// </summary>
    public T Data
    {
        get => IsSuccess ? _data : throw new InvalidOperationException($"{nameof(Success)} : {Success}");
        private set => _data = value;
    }

    /// <summary>
    /// Создание результата с ошибкой
    /// </summary>
    /// <typeparam name="T">Тип данных результата</typeparam>
    /// <param name="error">Ошибка</param>
    /// <returns>Результат с ошибкой</returns>
    public static new Result<T> Failure(string error)
    {
        return new Result<T>([error]);
    }
}
