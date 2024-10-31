using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using System.Diagnostics;

namespace NetConf2024Search.API.Helpers;

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddCustomOpenTelemetryWorker(
        this IServiceCollection services,
        IConfiguration configuration,
        string serviceName)
    {
        var otlpExporterEndpoint = configuration.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT");
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (!string.IsNullOrEmpty(otlpExporterEndpoint))
        {
            var sdk = Sdk.CreateTracerProviderBuilder()
                .AddHttpClientInstrumentation()
                .AddSource(serviceName)
                .AddSource("Microsoft.Azure.Functions.Worker")
                .AddSource("OpenAI.*")
                .AddHttpClientInstrumentation()
                .SetSampler(new AlwaysOnSampler())
                .ConfigureResource(configure => configure
                   .AddService(serviceName))
                .AddOtlpExporter()
                .Build();

            services
            .AddOpenTelemetry()
                .UseFunctionsWorkerDefaults()
                .ConfigureResource(resource => resource.AddService(serviceName))
                .WithMetrics(metrics =>
                {
                    metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddMeter("OpenAI.*")
                    .AddOtlpExporter();
                });
        }

        return services;
    }

    public static ILoggingBuilder AddCustomOpenTelemetryWorker(
        this ILoggingBuilder builder,
        IConfiguration configuration)
    {
        var otlpExporterEndpoint = configuration.GetValue<string>("OTEL_EXPORTER_OTLP_ENDPOINT");
        if (!string.IsNullOrEmpty(otlpExporterEndpoint))
        {
            builder.AddOpenTelemetry(options => options.AddOtlpExporter());
        }

        return builder;
    }

    public static void LogWithActivity(this ILogger logger, string message, LogLevel level, Activity? activity)
    {
        activity?.AddEvent(new ActivityEvent(message));
        logger.Log(level, message);
    }
}
