namespace CourseTech.Domain.Result;

/// <summary>
/// Класс для реализации паттерна Result Pattern.
/// Абстракция, которая представляет собой коллекции результов выполнения операции
/// </summary>
/// <typeparam name="T">Тип элемента коллекции</typeparam>
public class CollectionResult<T> : DataResult<IEnumerable<T>>
{
    private CollectionResult(IEnumerable<T> data, Error error = null)
        : base(data, error)
    {
        Count = data?.Count() ?? 0;
    }

    /// <summary>
    /// Количество элементов в результате
    /// </summary>
    public int Count { get; }

    public static new CollectionResult<T> Success(IEnumerable<T> data) =>
        new(data);

    public static new CollectionResult<T> Failure(int errorCode, string errorMessage) =>
        new([], new Error(errorMessage, errorCode));
}
