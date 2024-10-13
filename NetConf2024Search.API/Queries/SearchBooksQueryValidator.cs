using FluentValidation;
using Microsoft.Extensions.Logging;

namespace NetConf2024Search.API.Queries;

public class SearchBooksQueryValidator : AbstractValidator<SearchBooksQuery>
{
    public SearchBooksQueryValidator(ILogger<SearchBooksQueryValidator> logger)
    {
        RuleFor(query => query.SearchTerm)
            .NotEmpty()
            .NotNull()
            .WithMessage(command => $"{nameof(command.SearchTerm)} is required");

        RuleFor(query => query.Paging.Number).GreaterThan(0).Unless(query => query.Paging == null);
        RuleFor(query => query.Paging.Size).GreaterThan(0).Unless(query => query.Paging == null);
        RuleFor(query => query.Paging.Size).LessThanOrEqualTo(100).Unless(query => query.Paging == null);

        logger.LogTrace("{className} validation run successfully", GetType().Name);
    }
}