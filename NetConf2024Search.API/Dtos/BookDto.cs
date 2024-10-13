namespace NetConf2024Search.API.Dtos;

public class BookDto
{
    public Guid  Id { get; set; }

    public required string Title { get; set; }

    public required string Reference { get; set; }

    public string? Summary { get; set; }

    public required string Gendre { get; set; }

    public DateTime PublishedOn { get; set; }

    public bool InStock { get; set; }

    public int NumberOfPages { get; set; }

    public double Price { get; set; }

    public string? LanguagesRaw { get; set; }

    public string? MostRecentComment { get; set; }

    public int NumberOfComments { get; set; }

    public required string AuthorName { get; set; }

    public string? AuthorBio { get; set; }

    public double? Score { get; set; }

    public IDictionary<string, IList<string>>? Highlights { get; internal set; }
}