using Azure.Search.Documents.Indexes;
using System.Text.Json.Serialization;

namespace NetConf2024Search.API.Model;

public class Language
{
    [SimpleField(IsFilterable = true)]
    public required string LanguageName { get; init; }
}