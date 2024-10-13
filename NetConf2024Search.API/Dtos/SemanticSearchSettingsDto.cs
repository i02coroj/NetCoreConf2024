namespace NetConf2024Search.API.Dtos;

public class SemanticSearchSettingsDto
{
    public required string TitleField { get; set; }

    public required string[] ContentFields { get; set; }
}