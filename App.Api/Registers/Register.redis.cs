using App.Api.Settings;
using App.Core.SharedLibrary.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace App.API.Registers;

public static partial class Register
{
    public static IServiceCollection RegisterRedis(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // 1. Bind and register options
        IConfigurationSection cacheSection = configuration.GetSection(CacheSettings.SectionName);
        services.Configure<CacheSettings>(cacheSection);

        // 2. Extract Redis configuration
        CacheSettings cacheSettings = cacheSection.Get<CacheSettings>()
            ?? throw new InvalidOperationException($"'{CacheSettings.SectionName}' configuration section is missing.");

        RedisSettings redisSettings = cacheSettings.Redis;

        // 3. Configure StackExchange.Redis options
        ConfigurationOptions redisOptions = new ConfigurationOptions
        {
            EndPoints = { { redisSettings.Host, redisSettings.Port } },
            Password = string.IsNullOrWhiteSpace(redisSettings.Password) ? null : redisSettings.Password,
            User = string.IsNullOrWhiteSpace(redisSettings.User) ? null : redisSettings.User,
            AbortOnConnectFail = false, // Recommended: prevents startup crash if Redis temporarily reconnects
            ConnectTimeout = 5000,
            SyncTimeout = 5000
        };

        // 4. Register ConnectionMultiplexer as a Singleton
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisOptions));

        // 5. Register custom cache service helper
        services.AddSingleton<RedisCacheService>();

        // 6. Register IDistributedCache reusing the existing IConnectionMultiplexer
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConnectionMultiplexerFactory = () =>
                Task.FromResult(services.BuildServiceProvider().GetRequiredService<IConnectionMultiplexer>());
        });

        return services;
    }
}