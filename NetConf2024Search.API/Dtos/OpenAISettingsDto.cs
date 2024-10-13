namespace NetConf2024Search.API.Dtos;

public class OpenAISettingsDto
{
    public required string OpenAIUri { get; set; }

    public required string OpenAIKeySecretName { get; set; }
}