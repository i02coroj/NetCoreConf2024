using NetConf2024Search.API.Dtos;

namespace NetConf2024Search.API.Queries;

public class SearchBooksQueryResponse
{
    public IEnumerable<BookDto>? Books { get; set; }

    public long TotalResults { get; set; }

    public string? SearchId { get; set; }

    public IDictionary<string, long>? Facets { get; set; }
}