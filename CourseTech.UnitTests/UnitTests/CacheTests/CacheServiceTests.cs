using CourseTech.DAL.Cache;
using CourseTech.Domain.Dto.Category;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;
using Xunit;

namespace CourseTech.Tests.UnitTests.CacheTests;

public class CacheServiceTests
{
    private readonly Mock<IDistributedCache> _mockCache;
    private readonly CacheService _cacheService;

    public CacheServiceTests()
    {
        _mockCache = new Mock<IDistributedCache>();
        _cacheService = new CacheService(_mockCache.Object);
    }

    [Fact]
    public async Task SetObjectAsync_ShouldSaveObjectToCache()
    {
        // Arrange
        var key = "testKey";
        var obj = new CategoryDto { Name = "Test" };
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        };


        // Act
        await _cacheService.SetObjectAsync(key, obj, options);

        // Assert
        _mockCache.Verify(cache => cache.SetAsync(
            key,
            It.IsAny<byte[]>(),
            options,
            default),
            Times.Once);
    }

    [Fact]
    public async Task GetObjectAsync_ShouldReturnDeserializedObject_WhenCacheContainsData()
    {
        // Arrange
        var key = "testKey";
        var obj = new CategoryDto() { Name = "Test" };
        var serializedObj = JsonSerializer.SerializeToUtf8Bytes(obj);
        _mockCache.Setup(cache => cache.GetAsync(key, It.IsAny<CancellationToken>())).ReturnsAsync(serializedObj);

        // Act
        var result = await _cacheService.GetObjectAsync<CategoryDto>(key);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task GetObjectAsync_ShouldReturnNull_WhenCacheIsEmpty()
    {
        // Arrange
        var key = "testKey";
        _mockCache.Setup(cache => cache.GetAsync(key, default)).ReturnsAsync((byte[])null);

        // Act
        var result = await _cacheService.GetObjectAsync<object>(key);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetOrAddToCache_ShouldReturnCachedValue_WhenCacheContainsData()
    {
        // Arrange
        var key = "testKey";
        var cachedObj = new { Name = "Test" };
        var serializedObj = JsonSerializer.SerializeToUtf8Bytes(cachedObj);
        _mockCache.Setup(cache => cache.GetAsync(key, default)).ReturnsAsync(serializedObj);

        // Act
        var result = await _cacheService.GetOrAddToCache(key, async () => new { Name = "New" });

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test", ((dynamic)result).Name);
    }

    [Fact]
    public async Task GetOrAddToCache_ShouldAddToCache_WhenNoDataInCache()
    {
        // Arrange
        var key = "testKey";
        _mockCache.Setup(cache => cache.GetAsync(key, default)).ReturnsAsync((byte[])null);
        var factory = new Func<Task<object>>(async () => new { Name = "New" });

        // Act
        var result = await _cacheService.GetOrAddToCache(key, factory);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New", ((dynamic)result).Name);
        _mockCache.Verify(cache => cache.SetAsync(key, It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), default), Times.Once);
    }
}
