namespace CourseTech.Domain.Result;

/// <summary>
/// Класс для реализации паттерна Result Pattern.
/// Абстракция, которая представляет собой результат выполнения операции
/// </summary>
public class BaseResult
{
    protected BaseResult(Error error = null)
    {
        Error = error ?? new Error();
    }

    public bool IsSuccess => Error.Message == null;

    public Error Error { get; }

    public static BaseResult Success() => new BaseResult();

    public static BaseResult Failure(int errorCode, string errorMessage) =>
        new(new Error(errorMessage, errorCode));
}

/// <summary>
/// Класс для реализации паттерна Result Pattern.
/// Абстракция, которая представляет собой результат выполнения операции
/// Имеет свойство Data
/// </summary>
/// <typeparam name="T"></typeparam>
public class DataResult<T> : BaseResult
{
    protected DataResult(T data, Error error = null)
        : base(error)
    {
        Data = data;
    }

    public T Data { get; }

    public static DataResult<T> Success(T data) =>
        new(data);

    public static new DataResult<T> Failure(int errorCode, string errorMessage) =>
        new(default, new Error(errorMessage, errorCode));
}
