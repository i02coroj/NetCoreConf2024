using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace NetConf2024Search.Seeder;

public static class ResiliencyExtensions
{
    public static AsyncRetryPolicy CreateSQLRetryPolicyAsync(
        ILogger logger,
        string prefix,
        int retries = 3)
    {
        return Policy
            .Handle<SqlException>()
            .WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(
                        exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix,
                        exception.GetType().Name,
                        exception.Message,
                        retry,
                        retries);
                });
    }

    public static RetryPolicy CreateSQLRetryPolicy(
        ILogger logger,
        string prefix,
        int retries = 3)
    {
        return Policy
            .Handle<SqlException>()
            .WaitAndRetry(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                onRetry: (exception, timeSpan, retry, ctx) =>
                {
                    logger.LogWarning(
                        exception,
                        "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}",
                        prefix,
                        exception.GetType().Name,
                        exception.Message,
                        retry,
                        retries);
                });
    }
}
