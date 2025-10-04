namespace CourseTech.Domain.Result;

/// <summary>
/// Класс для реализации паттерна Result Pattern.
/// Абстракция, которая представляет собой коллекции результов выполнения операции
/// </summary>
/// <typeparam name="T">Тип элемента коллекции</typeparam>
public class CollectionResult<T> : DataResult<IEnumerable<T>>
{
    private CollectionResult(IEnumerable<T> data, string message = null)
        : base(data, message)
    {
        Count = data?.Count() ?? 0;
    }

    public int Count { get; }

    public static new CollectionResult<T> Success(IEnumerable<T> data) =>
        new(data);

    public static new CollectionResult<T> Failure(string errorMessage) =>
        new([], errorMessage);
}
