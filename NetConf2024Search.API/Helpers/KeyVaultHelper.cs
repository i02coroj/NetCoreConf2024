using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;
using NetConf2024Search.API.Dtos;

namespace NetConf2024Search.API.Helpers;

public class KeyVaultHelper(IOptions<SearchSettingsDto> searchSettings) : IKeyVaultHelper
{
    public readonly SearchSettingsDto _searchSettings = searchSettings?.Value ?? throw new ArgumentNullException(nameof(searchSettings));
    private readonly Dictionary<string, string> _cache = [];

    public async Task<string> GetSecretAsync(string secretName)
    {
        if (_cache.TryGetValue(secretName, out string? value))
        {
            return value;
        }
        else
        {
            var credentialOptions = new DefaultAzureCredentialOptions
            {
                ExcludeManagedIdentityCredential = true,
                ExcludeAzureCliCredential = true
            };

            var credential = new DefaultAzureCredential(credentialOptions);
            var secretClient = new SecretClient(new Uri(_searchSettings.KeyVaultUri), credential);
            var secret = await secretClient.GetSecretAsync(secretName);
            var searchKey = secret.Value.Value;
            _cache.Add(secretName, searchKey);            
            return searchKey;
        }
    }
}