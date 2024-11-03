using Microsoft.Extensions.Caching.Distributed;

namespace CourseTech.Domain.Interfaces.Cache
{
    /// <summary>
    /// Сервис для работы с кэшированием
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Получить объект из кэша по ключу
        /// </summary>
        /// <typeparam name="T">Тип объекта, который получаем из кэша.</typeparam>
        /// <param name="key">Ключ кэша.</param>
        /// <returns></returns>
        Task<T> GetObjectAsync<T>(string key) where T : class;

        /// <summary>
        /// Попытка получить объект из кэша, если в кэше объекта нет, то получение объекта из БД с помощью фабрики.
        /// </summary>
        /// <typeparam name="T">То что хотим получить из кэша</typeparam>
        /// <param name="key">Ключ кэша</param>
        /// <param name="factory">Фабрика (запрос для БД).</param>
        /// <returns></returns>
        Task<T> GetOrAddToCache<T>(string key, Func<Task<T>> factory) where T : class;

        
        /// <summary>
        /// Положить объект в хранилище кэша.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">Ключ кэша.</param>
        /// <param name="obj">Объект, который кладём в кэш.</param>
        /// <param name="options">Настройки кэширования.</param>
        /// <returns></returns>
        Task SetObjectAsync<T>(string key, T obj, DistributedCacheEntryOptions options = null) where T : class;

        /// <summary>
        /// Убрать объект из кэша по ключу.
        /// </summary>
        /// <param name="key">Ключ кэша.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
