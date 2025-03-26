namespace CourseTech.Domain.Settings;

/// <summary>
/// Настройка кэша redis
/// </summary>
public class RedisSettings
{
    public string Url { get; set; }

    public string InstanceName { get; set; }
}
