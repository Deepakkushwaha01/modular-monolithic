namespace App.Api.Settings;

public class CacheSettings
{
    public const string SectionName = "Cache";

    public RedisSettings Redis { get; set; } = new();
}

public class RedisSettings
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public string Password { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;

    /// <summary>
    /// Helper to generate a StackExchange.Redis compatible connection string
    /// </summary>
    public string ConnectionString => $"{Host}:{Port},user={User},password={Password}";
}