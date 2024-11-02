using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using System.Text.Json.Serialization;

namespace NetConf2024Search.API.Model;

public sealed class Book
{
    [SimpleField(IsKey = true, IsFilterable = true)]
    public required string Id { get; init; }

    [SearchableField(IsSortable = true)]
    public required string Title { get; init; }

    [SearchableField(IsSortable = true)]
    public required string Reference { get; init; }

    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string? Summary { get; init; }

    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EsMicrosoft)]
    public string? SummaryTranslated { get; init; }

    [SearchableField(IsSortable = true, IsFacetable = true)]
    public required string Gendre { get; init; }

    [SimpleField(IsSortable = true)]
    public DateTime PublishedOn { get; init; }

    [SimpleField(IsSortable = true)]
    public bool InStock { get; init; }

    [SimpleField(IsSortable = true)]
    public int NumberOfPages { get; init; }

    [SimpleField(IsSortable = true)]
    public double Price { get; init; }

    /// <summary>
    /// High water mark field
    /// </summary>
    [SimpleField(IsFilterable = true, IsHidden = true)]
    public DateTimeOffset LastModifiedDt { get; init; }

    [SearchableField(IsSortable = true)]
    public required string AuthorFirstName { get; init; }

    [SearchableField(IsSortable = true)]
    public required string AuthorLastName { get; init; }

    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public string? AuthorBio { get; init; }

    public Comment[]? Comments { get; init; }

    public Language[]? Languages { get; init; }

    [SearchableField]
    public string? LanguageCode { get; set; }

    [SearchableField]
    public string? LanguageName { get; set; }

    [JsonIgnore]
    public string LanguagesRaw => Languages != null ? string.Join(",", Languages.Select(l => l.LanguageName)) : string.Empty;

    [JsonIgnore]
    public string? MostRecentComment => ($"{Comments?.OrderByDescending(c => c.PublishedOn).FirstOrDefault()?.CommentSummary}").Trim();

    [JsonIgnore]
    public int NumberOfComments => Comments?.Length ?? 0;

    [JsonIgnore]
    public string AuthorName => ($"{AuthorFirstName} {AuthorLastName}").Trim();
}
