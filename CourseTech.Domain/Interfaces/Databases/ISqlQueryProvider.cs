namespace CourseTech.Domain.Interfaces.Databases;

/// <summary>
/// Предоставляет функциональность для выполнения SQL-запросов в учебной БД.
/// </summary>
public interface ISqlQueryProvider
{
    /// <summary>
    /// Выполняет запрос пользователя в учебную БД.
    /// </summary>
    /// <param name="sqlQuery">SQL запрос пользователя в виде строки</param>
    /// <returns></returns>
    Task<(List<dynamic> result, double elapsedSeconds)> ExecuteQueryAsync(string sqlQuery);
}
