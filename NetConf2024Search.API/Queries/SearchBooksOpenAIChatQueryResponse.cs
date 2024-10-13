namespace NetConf2024Search.API.Queries;

public class SearchBooksOpenAIChatQueryResponse
{
    public required string MessageContent { get; set; }

    public List<string>? Citations { get; set; }
}