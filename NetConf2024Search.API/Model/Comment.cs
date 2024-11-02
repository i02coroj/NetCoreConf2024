using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using System.Text.Json.Serialization;

namespace NetConf2024Search.API.Model;

public class Comment
{
    [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnMicrosoft)]
    public required string Text { get; init; }

    [SearchableField]
    public required string Author { get; init; }

    [SimpleField]
    public DateTime PublishedOn { get; init; }

    [SearchableField]
    public string? Sentiment{ get; init; }

    [JsonIgnore]
    public string CommentSummary => $"{Text} ({Author} - {PublishedOn})";
}