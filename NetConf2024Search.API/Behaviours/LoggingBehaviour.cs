using MediatR;
using Microsoft.Extensions.Logging;
using NetConf2024Search.API.Helpers;
using System.Diagnostics;
using System.Text.Json;

namespace NetConf2024Search.API.Behaviours;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling command {CommandName} ({@Command})", request.GetGenericTypeName(), JsonSerializer.Serialize(request));
        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();
        _logger.LogInformation("Command {CommandName} handled - Execution time={elapsedMilliseconds}ms - response: {@Response}",
            request.GetGenericTypeName(),
            stopwatch.ElapsedMilliseconds,
            response);

        return response;
    }
}
