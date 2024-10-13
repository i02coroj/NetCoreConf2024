using FluentValidation;
using Microsoft.Extensions.Logging;

namespace NetConf2024Search.API.Queries;

public class SearchBooksSemanticQueryValidator : AbstractValidator<SearchBooksSemanticQuery>
{
    public SearchBooksSemanticQueryValidator(ILogger<SearchBooksSemanticQueryValidator> logger)
    {
        RuleFor(query => query.SearchTerm)
            .NotEmpty()
            .NotNull()
            .WithMessage(command => $"{nameof(command.SearchTerm)} is required");

        logger.LogTrace("{className} validation run successfully", GetType().Name);
    }
}