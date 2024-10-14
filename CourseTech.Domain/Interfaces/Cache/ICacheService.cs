using Microsoft.Extensions.Caching.Distributed;

namespace CourseTech.Domain.Interfaces.Cache
{
    /// <summary>
    /// Сервис для работы с кэшированием
    /// </summary>
    public interface ICacheService
    {
        Task<T> GetObjectAsync<T>(string key) where T : class;

        Task<T> GetOrAddToCache<T>(string key, Func<Task<T>> factory) where T : class;

        Task SetObjectAsync<T>(string key, T obj, DistributedCacheEntryOptions options = null) where T : class;

        Task RemoveAsync(string key);
    }
}
