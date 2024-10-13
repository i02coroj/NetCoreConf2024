using FluentValidation;

namespace NetConf2024Search.API.Commands;

public class LogClickCommandValidator : AbstractValidator<LogClickCommand>
{
    public LogClickCommandValidator()
    {
        RuleFor(command => command.SearchId)
            .NotEmpty()
            .NotNull()
            .WithMessage(command => $"{nameof(command.SearchId)} is required");

        RuleFor(command => command.DocumentId)
            .NotEmpty()
            .NotNull()
            .WithMessage(command => $"{nameof(command.DocumentId)} is required");

        RuleFor(command => command.Position)
            .Must(BeValidPositiveInteger)
            .Unless(command => command.Position == null)
            .WithMessage("Not a valid position");
    }

    private static bool BeValidPositiveInteger(int? value)
    {
        return value.HasValue && value > 0;
    }
}