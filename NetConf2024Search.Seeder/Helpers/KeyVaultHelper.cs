using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;

namespace NetConf2024Search.Seeder.Helpers;

public static class KeyVaultHelper
{
    public static string GetSecret(IConfiguration configuration, string? secretName)
    {
        var keyVaultUri = configuration["KeyVaultUri"] ?? throw new ArgumentNullException(nameof(configuration));
        var credentialOptions = new DefaultAzureCredentialOptions
        {
            ExcludeManagedIdentityCredential = true,
            ExcludeAzureCliCredential = true
        };

        var credential = new DefaultAzureCredential(credentialOptions);
        var secretClient = new SecretClient(new Uri(keyVaultUri), credential);
        var secret = secretClient.GetSecret(secretName);
        var searchKey = secret.Value.Value;
        return searchKey;
    }
}