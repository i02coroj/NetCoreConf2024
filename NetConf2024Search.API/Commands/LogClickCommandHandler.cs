using Azure.Core;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Options;
using NetConf2024Search.API.Dtos;
using NetConf2024Search.API.Helpers;

namespace NetConf2024Search.API.Commands;

public class LogClickCommandHandler(
    IKeyVaultHelper keyVaultHelper,
    IOptions<SearchSettingsDto> searchSettings,
    TelemetryClient telemetryClient) : IRequestHandler<LogClickCommand, Unit>
{
    private SearchClient? _searchClient;
    private readonly SearchSettingsDto _searchSettings = searchSettings?.Value ?? throw new ArgumentNullException(nameof(searchSettings));
    private readonly IKeyVaultHelper _keyVaultHelper = keyVaultHelper ?? throw new ArgumentNullException(nameof(keyVaultHelper));
    private readonly TelemetryClient _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
    private readonly string _indexName = "books-index";

    protected async Task InitialiseSearch()
    {
        var searchKey = await _keyVaultHelper.GetSecretAsync(_searchSettings.SearchAdminApiKeySecretName);
        var clientOptions = new SearchClientOptions();
        clientOptions.AddPolicy(new SearchIdPipelinePolicy(), HttpPipelinePosition.PerCall);

        var indexClient = new SearchIndexClient(new Uri(_searchSettings.SearchUri), new AzureKeyCredential(searchKey), clientOptions);
        _searchClient ??= indexClient.GetSearchClient(_indexName);
    }

    public async Task<Unit> Handle(
        LogClickCommand command,
        CancellationToken cancellationToken)
    {
        await InitialiseSearch();

        LogClickTelemetry(
            command.SearchId,
            command.DocumentId,
            command.Position);

        return Unit.Value;
    }

    protected void LogClickTelemetry(
        string searchId,
        string documentId,
        int? position)
    {
        var properties = new Dictionary<string, string>
        {
            {"SearchServiceName", _searchClient!.ServiceName},
            {"SearchId", searchId},
            {"ClickedDocId", documentId}
        };

        if (position.HasValue)
        {
            properties.Add("Rank", position.Value.ToString());
        }

        _telemetryClient.TrackEvent("Click", properties);
    }
}