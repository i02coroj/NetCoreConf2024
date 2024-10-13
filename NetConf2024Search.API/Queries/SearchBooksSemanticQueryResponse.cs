using NetConf2024Search.API.Dtos;

namespace NetConf2024Search.API.Queries;

public class SearchBooksSemanticQueryResponse
{
    public IEnumerable<BookSemanticDto>? Books { get; set; }

    public long TotalResults { get; set; }
}