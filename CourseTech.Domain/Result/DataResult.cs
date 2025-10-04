namespace CourseTech.Domain.Result;

/// <summary>
/// Класс для реализации паттерна Result Pattern.
/// Абстракция, которая представляет собой результат выполнения операции
/// Имеет свойство Data
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataResult<T> : BaseResult
{
    protected DataResult(T data, string message = null)
        : base(message)
    {
        Data = data;
    }

    public T Data { get; }

    public static DataResult<T> Success(T data) =>
        new(data);

    public static new DataResult<T> Failure(string errorMessage) =>
        new(default, errorMessage);
}
