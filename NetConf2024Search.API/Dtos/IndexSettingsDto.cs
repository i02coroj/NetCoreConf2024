namespace NetConf2024Search.API.Dtos;

public class IndexSettingsDto
{
    public required string Name { get; set; }

    public required string Type { get; set; }

    public List<string>? Analyzers { get; set; }

    public SemanticSearchSettingsDto? SemanticSearch { get; set; }
}