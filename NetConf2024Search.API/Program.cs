using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetConf2024Search.API;
using NetConf2024Search.API.Behaviours;
using NetConf2024Search.API.Dtos;
using NetConf2024Search.API.Helpers;

var builder = new HostBuilder();

builder
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureLogging((ctx, logging) =>
    {
        logging
            .ClearProviders()
            .AddConsole()
            .AddCustomOpenTelemetryWorker(ctx.Configuration);
    })
    .ConfigureServices(async services =>
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("local.settings.json", true, true)
            .AddEnvironmentVariables()
            .Build();

        services
            .AddApplicationInsightsTelemetryWorkerService()
            .ConfigureFunctionsApplicationInsights()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>))
            .AddValidatorsFromAssembly(typeof(Program).Assembly)
            .AddSingleton<IKeyVaultHelper, KeyVaultHelper>()
            .Configure<OpenAISettingsDto>(options => configuration.GetSection("OpenAISettings").Bind(options))
            .Configure<SearchSettingsDto>(options => configuration.GetSection("SearchSettings").Bind(options))
            .Configure<List<IndexSettingsDto>>(options => configuration.GetSection("Indexes").Bind(options))
            .Configure<List<IndexerSettingsDto>>(options => configuration.GetSection("Indexers").Bind(options))
            .AddCustomOpenTelemetryWorker(configuration, "NetConf2024Search.API");

        await SearchBuilder.UpsertIndexesAndIndexersAsync(services, configuration);
    });

await builder
    .Build()
    .RunAsync();