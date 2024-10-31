using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetConf2024Search.Seeder;
using NetConf2024Search.Seeder.Helpers;

var builder = new HostBuilder()
    .ConfigureLogging((hostingContext, logging) =>
    {
        logging.AddConsole();
    })
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.Sources.Clear();
        configuration.AddJsonFile("appsettings.json", optional: true);
    })
    .ConfigureServices((hostContext, services) =>
    {

        var dataSourceConnectionString = KeyVaultHelper.GetSecret(hostContext.Configuration, hostContext.Configuration.GetConnectionString("Books"));
        services
            .AddScoped<Seeder>()
            .AddDbContext<SearchDbContext>(options => options.UseSqlServer(
                dataSourceConnectionString,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(SearchDbContext).Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                }));
    });

var host = builder.Build();

host
    .MigrateDbContext<SearchDbContext>((context, services) =>
    {
        var seeder = services.GetRequiredService<Seeder>();
        seeder.SeedAsync().Wait();
    });

//host.Run();