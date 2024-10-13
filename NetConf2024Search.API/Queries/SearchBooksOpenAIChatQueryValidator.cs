using FluentValidation;
using Microsoft.Extensions.Logging;

namespace NetConf2024Search.API.Queries;

public class SearchBooksOpenAIChatQueryValidator : AbstractValidator<SearchBooksOpenAIChatQuery>
{
    public SearchBooksOpenAIChatQueryValidator(ILogger<SearchBooksOpenAIChatQueryValidator> logger)
    {
        RuleFor(query => query.SearchTerm)
            .NotEmpty()
            .NotNull()
            .WithMessage(command => $"{nameof(command.SearchTerm)} is required");

        logger.LogTrace("{className} validation run successfully", GetType().Name);
    }
}