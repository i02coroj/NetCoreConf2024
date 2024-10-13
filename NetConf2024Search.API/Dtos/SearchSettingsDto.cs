namespace NetConf2024Search.API.Dtos;

public class SearchSettingsDto
{
    public required string SearchUri { get; set; }

    public required string SearchAdminApiKeySecretName { get; set; }

    public required string KeyVaultUri { get; set; }
}