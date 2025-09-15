// Rayssa Romaniuk

using AcademiaDoZe.Application.DependencyInjection;
using AcademiaDoZe.Application.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace AcademiaDoZe.Application.Tests;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureServices(string connectionString, EAppDatabaseType databaseType)
    {
        var services = new ServiceCollection();
        services.AddApplicationServices();
        services.AddSingleton(new RepositoryConfig { ConnectionString = connectionString, DatabaseType = databaseType });
        return services;
    }
    public static IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        return services.BuildServiceProvider();
    }
}