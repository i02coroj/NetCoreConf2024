using MediatR;

namespace NetConf2024Search.API.Commands;

public class LogClickCommand : IRequest<Unit>
{

    public required string SearchId { get; set; }

    public required string DocumentId { get; set; }

    public int? Position { get; set; }
}