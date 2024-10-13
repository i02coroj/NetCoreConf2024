using Azure.Search.Documents.Models;
using MediatR;
using NetConf2024Search.API.Dtos;

namespace NetConf2024Search.API.Queries;

public class SearchBooksSemanticQuery : IRequest<SearchBooksSemanticQueryResponse>
{
    public required string SearchTerm { get; set; }
}