using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Polly;
using Serilog;

namespace Infrastructure;

public static class InfrastructureStartup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContextFactory<ApplicationDbContext>();
        return services;
    }

    public static async Task CheckAndMigrateDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(10));
        var retryPolicy = Policy.Handle<PostgresException>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 2),
                onRetry: (exception, delay, attempt, context) =>
                {
                    Log.Error($"Migration retry {attempt} due to {exception.Message}. Waiting {delay} before next retry.");
                });
        var combinedPolicy = Policy.WrapAsync(retryPolicy, timeoutPolicy);
        try
        {
            Log.Information("Checking and migrating database.");
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await combinedPolicy.ExecuteAsync(async () => await dbContext.Database.MigrateAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}