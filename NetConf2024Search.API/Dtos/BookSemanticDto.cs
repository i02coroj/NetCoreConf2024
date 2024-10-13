namespace NetConf2024Search.API.Dtos;

public class BookSemanticDto
{
    public Guid  Id { get; set; }

    public required string Title { get; set; }

    public required string AuthorName { get; set; }

    public double? SemanticScore { get; set; }

    public IList<string>? Highlights { get; set; }

    public IList<string>? Answers { get; set; }
}