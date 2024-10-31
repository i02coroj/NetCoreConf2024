using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using NetConf2024Search.API.Helpers;

namespace NetConf2024Search.API.Behaviours;

public class ValidatorBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> _validators,
    ILogger<ValidatorBehavior<TRequest, TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();
        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Count != 0)
        {
            _logger.LogWarning("Validation errors - {CommandType} - Command: {@Command} - Errors: {@ValidationErrors}", typeName, request, failures);

            throw new ValidationException(
                $"Command Validation errors for type {typeof(TRequest).Name}: {string.Join(".", failures)}",
                failures);
        }

        return await next();
    }
}