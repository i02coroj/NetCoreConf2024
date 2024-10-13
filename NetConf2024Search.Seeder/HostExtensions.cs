using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NetConf2024Search.Seeder;

public static class HostExtensions
{

    public static IHost MigrateDbContext<TContext>(this IHost webHost, Action<TContext, IServiceProvider> seeder) where TContext : DbContext
    {
        using var scope = webHost.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
            var policy = ResiliencyExtensions.CreateSQLRetryPolicy(logger, nameof(Seeder));
            policy.Execute(() => MigrateContext(context, seeder, services));
            logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
            throw;
        }

        return webHost;
    }

    private static void MigrateContext<TContext>(TContext context, Action<TContext, IServiceProvider> seeder, IServiceProvider services)
        where TContext : DbContext
    {
        context.Database.Migrate();
        seeder?.Invoke(context, services);
    }
}
