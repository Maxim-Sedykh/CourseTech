namespace CourseTech.Domain
{
    public class CollectionResult<T> : Result<IEnumerable<T>>
    {
        public CollectionResult(IEnumerable<T> data) : base(data)
        {
            Count = data?.Count() ?? 0;
        }

        public CollectionResult(IReadOnlyCollection<string> errors) : base(errors) { }

        /// <summary>
        /// Количество элементов в коллекции
        /// </summary>
        public int Count { get; }

        /// <summary>
        /// Пустая ли коллекция
        /// </summary>
        public bool IsEmpty => Count == 0;

        /// <summary>
        /// Создает успешный результат с коллекцией данных
        /// </summary>
        public static CollectionResult<T> Success(IEnumerable<T> data) =>
            new(data ?? Enumerable.Empty<T>());

        public static new CollectionResult<T> Failure(string error)
        {
            return new CollectionResult<T>([error]);
        }
    }
}
