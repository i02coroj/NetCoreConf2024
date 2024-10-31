//using System;
//using Azure.Search.Documents.Indexes.Models;
//using Azure.Search.Documents.Indexes;
//using Azure;
//using Microsoft.Azure.Functions.Worker;
//using Microsoft.Extensions.Logging;
//using NetConf2024Search.API.Helpers;
//using NetConf2024Search.API.Dtos;
//using Microsoft.Extensions.Options;

//namespace NetConf2024Search.API.Triggers;

//public class IndexingTimerTrigger(
//    ILoggerFactory loggerFactory,
//    IKeyVaultHelper keyVaultHelper,
//    IOptions<SearchSettingsDto> searchSettings,
//    IOptions<List<IndexerSettingsDto>> indexerSettingsDtos)
//{
//    private readonly ILogger _logger = loggerFactory.CreateLogger<IndexingTimerTrigger>();
//    private readonly IKeyVaultHelper _keyVaultHelper = keyVaultHelper;
//    private readonly SearchSettingsDto _searchSettings = searchSettings?.Value ?? throw new ArgumentNullException(nameof(searchSettings));
//    private readonly List<IndexerSettingsDto> _indexerSettingsDtos = indexerSettingsDtos.Value ?? throw new ArgumentNullException(nameof(indexerSettingsDtos));

//    [Function(nameof(RunIndexingTimerTrigger))]
//    public void RunIndexingTimerTrigger([TimerTrigger("%SearchSettings:DefaultIndexingInterval%")] TimerInfo myTimer)
//    {
//        // We only run indexers with no predefined indexing schedule
//        var indexersToRun = _indexerSettingsDtos?.Where(indexer => (indexer.IndexingScheduleInMin ?? 0) <= 0);
//        if (indexersToRun?.Any() == true)
//        {
//            indexersToRun
//                .ToList()
//                .ForEach(indexer =>
//                {
//                    RunIndexer(indexer);
//                });
//        }
//    }

//    /// <summary>
//    /// Create and run a <see cref="IndexerSettingsDto"/> against an index
//    /// </summary>
//    /// <param name="indexer">Indexer to run</param>
//    private void RunIndexer(IndexerSettingsDto indexer)
//    {
//        var searchServiceKey = _keyVaultHelper.GetSecretAsync(_searchSettings.SearchAdminApiKeySecretName).GetAwaiter().GetResult();
//        var indexerClient = new SearchIndexerClient(
//            new Uri(_searchSettings.SearchUri),
//            new AzureKeyCredential(searchServiceKey));

//        var azureSqlIndexer = new SearchIndexer(
//            name: indexer.IndexerName,
//            dataSourceName: indexer.DataSourceName,
//            targetIndexName: indexer.TargetIndex);

//        try
//        {
//            indexerClient.RunIndexer(azureSqlIndexer.Name);
//        }
//        catch (RequestFailedException ex) when (ex.Status == 409)
//        {
//            _logger.LogWarning("Failed to run indexer: {0}", ex.Message);
//        }
//    }
//}
