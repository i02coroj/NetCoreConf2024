using MediatR;

namespace NetConf2024Search.API.Queries;

public class SearchBooksOpenAIChatQuery : IRequest<SearchBooksOpenAIChatQueryResponse>
{
    public required string SearchTerm { get; set; }
}