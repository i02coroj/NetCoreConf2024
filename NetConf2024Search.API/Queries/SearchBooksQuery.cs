using Azure.Search.Documents.Models;
using MediatR;
using NetConf2024Search.API.Dtos;

namespace NetConf2024Search.API.Queries;

public class SearchBooksQuery : IRequest<SearchBooksQueryResponse>
{
    public required string SearchTerm { get; set; }

    public SearchMode? SearchMode { get; set; }

    public PagingDto Paging { get; set; } = new();

    public ICollection<SortDto>? Sorts { get; set; }
}