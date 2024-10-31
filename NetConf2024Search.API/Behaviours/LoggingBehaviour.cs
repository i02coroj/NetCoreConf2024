using MediatR;
using Microsoft.Extensions.Logging;
using NetConf2024Search.API.Helpers;
using System.Diagnostics;
using System.Text.Json;

namespace NetConf2024Search.API.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using var activity = DiagnosticsConfig.ActivitySource.StartActivity();
        _logger.LogWithActivity($"Handling command {request.GetGenericTypeName()} ({JsonSerializer.Serialize(request)})", LogLevel.Information, activity);
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();
        _logger.LogWithActivity($"Command {request.GetGenericTypeName()} handled - Execution time={stopwatch.ElapsedMilliseconds}", LogLevel.Information, activity);

        return response;
    }
}