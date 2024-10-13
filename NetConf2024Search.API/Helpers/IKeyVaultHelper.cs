namespace NetConf2024Search.API.Helpers;

public interface IKeyVaultHelper
{
    Task<string> GetSecretAsync(string secretName);
}