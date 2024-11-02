using Azure.Search.Documents.Indexes.Models;
using Azure.Search.Documents.Indexes;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetConf2024Search.API.Dtos;
using NetConf2024Search.API.Helpers;

namespace NetConf2024Search.API;

public static class SearchBuilder
{
    public static async Task UpsertIndexesAndIndexersAsync(IServiceCollection services, IConfiguration configuration)
    {
        var keyVaultHelper = services.BuildServiceProvider().GetRequiredService<IKeyVaultHelper>();
        var searchSettings = configuration.GetSection("SearchSettings").Get<SearchSettingsDto>();
        var searchKey = await keyVaultHelper.GetSecretAsync(searchSettings!.SearchAdminApiKeySecretName);

        await UpsertIndexesAsync(configuration, searchSettings.SearchUri, searchKey);
        await UpsertIndexersAsync(configuration, searchSettings, searchKey, keyVaultHelper);
    }

    private static async Task UpsertIndexesAsync(IConfiguration configuration, string searchUri, string searchKey)
    {
        var indexesSettings = new List<IndexSettingsDto>();
        configuration.GetSection("Indexes").Bind(indexesSettings);

        foreach (var indexSettings in indexesSettings)
        {
            await UpsertIndexAsync(searchUri, searchKey, indexSettings);
        }
    }

    private static async Task UpsertIndexAsync(string searchUri, string searchKey, IndexSettingsDto indexSettings)
    {
        var indexClient = new SearchIndexClient(new Uri(searchUri), new AzureKeyCredential(searchKey));
        var builder = new FieldBuilder();
        var definition = new SearchIndex(
            indexSettings.Name,
            builder.Build(typeof(Program).Assembly.GetType(indexSettings.Type)));

        AddAnalyzers(indexSettings, definition);
        AddSemanticSearch(indexSettings, definition);
        await indexClient.CreateOrUpdateIndexAsync(definition);
    }

    private static void AddSemanticSearch(IndexSettingsDto indexSettings, SearchIndex definition)
    {
        if (indexSettings.SemanticSearch != null)
        {
            definition.SemanticSearch = new SemanticSearch
            {
                Configurations =
                {
                    new SemanticConfiguration($"{indexSettings.Name}-semantic-config", new()
                    {
                        TitleField = new SemanticField(indexSettings.SemanticSearch.TitleField),
                        ContentFields =
                        {
                            new SemanticField(indexSettings.SemanticSearch.ContentFields.First()),
                            new SemanticField(indexSettings.SemanticSearch.ContentFields.Skip(1).First())
                        }
                    })
                }
            };
        }
    }

    private static void AddAnalyzers(IndexSettingsDto index, SearchIndex definition)
    {
        if (index.Analyzers?.Count != 0 == true)
        {
            foreach (var analyzerName in index.Analyzers!)
            {
                var analyzer = analyzerName switch
                {
                    "dots_analyzer" => GetDotsAnalyzerPattern(),
                    _ => throw new ArgumentException($"Analyzer {analyzerName} not supported")
                };

                definition.Analyzers.Add(analyzer);
            }
        }
    }

    /// <summary>
    /// Analyzer for converting dots into spaces, needed so numbers get correctly tokenized. It also lowercases the value.
    /// It uses the existing pattern analyzer, no need to create a custom one.
    /// More details here: https://learn.microsoft.com/en-us/azure/search/index-add-custom-analyzers
    /// </summary>
    /// <example>
    /// Without this Ref=23.526.519-0 gets tokenized as "23.526.519" and "0".
    /// With this analyzer it will be tokenized as "23", "526", "519" and "0" instead.
    /// </example>
    /// <returns>Analyzer with the pattern to replace dots by spaces</returns>
    private static PatternAnalyzer GetDotsAnalyzerPattern()
    {
        var analyzer = new PatternAnalyzer("dot_analyzer")
        {
            Pattern = @"[\.]"
        };
        analyzer.Flags.Add(new RegexFlag("CASE_INSENSITIVE"));
        return analyzer;
    }

    private static async Task UpsertIndexersAsync(
        IConfiguration configuration,
        SearchSettingsDto searchSettings,
        string searchKey,
        IKeyVaultHelper keyVaultHelper)
    {
        var indexers = new List<IndexerSettingsDto>();
        configuration.GetSection("Indexers").Bind(indexers);
        foreach (var indexer in indexers)
        {
            await UpsertIndexerAsync(searchSettings, searchKey, indexer, keyVaultHelper);
        }
    }

    private static async Task UpsertIndexerAsync(
        SearchSettingsDto searchSettings,
        string searchKey,
        IndexerSettingsDto indexerDto,
        IKeyVaultHelper keyVaultHelper)
    {
        var dataSourceConnectionString = await keyVaultHelper.GetSecretAsync(indexerDto.DataSourceConnectionString);

        var indexerClient = new SearchIndexerClient(new Uri(searchSettings.SearchUri), new AzureKeyCredential(searchKey));
        var container = new SearchIndexerDataContainer(indexerDto.DataSourceContainerName);
        var dataSource = new SearchIndexerDataSourceConnection(
            indexerDto.DataSourceName,
            string.IsNullOrEmpty(indexerDto.DataSourceType) ? SearchIndexerDataSourceType.AzureSql : (SearchIndexerDataSourceType)indexerDto.DataSourceType,
            dataSourceConnectionString,
            container);

        await CreateDataSourceAsync(indexerDto, indexerClient, dataSource);
        await CreateIndexerAsync(indexerDto, indexerClient, dataSource, keyVaultHelper);
    }

    private static async Task CreateDataSourceAsync(
        IndexerSettingsDto indexerDto,
        SearchIndexerClient indexerClient,
        SearchIndexerDataSourceConnection dataSource)
    {
        if (!string.IsNullOrEmpty(indexerDto.WaterMarkField))
        {
            dataSource.DataChangeDetectionPolicy = new HighWaterMarkChangeDetectionPolicy(indexerDto.WaterMarkField);
        }

        if (!string.IsNullOrEmpty(indexerDto.SoftDeleteField))
        {
            dataSource.DataDeletionDetectionPolicy = new SoftDeleteColumnDeletionDetectionPolicy
            {
                SoftDeleteColumnName = indexerDto.SoftDeleteField,
                SoftDeleteMarkerValue = indexerDto.SoftDeleteMarkerValue
            };
        }

        await indexerClient.CreateOrUpdateDataSourceConnectionAsync(dataSource);
    }

    private static async Task CreateIndexerAsync(
        IndexerSettingsDto indexerDto,
        SearchIndexerClient indexerClient,
        SearchIndexerDataSourceConnection dataSource,
        IKeyVaultHelper keyVaultHelper)
    {
        var indexer = new SearchIndexer(
            name: indexerDto.IndexerName,
            dataSourceName: dataSource.Name,
            targetIndexName: indexerDto.TargetIndex);
        
        SetIndexingSchedule(indexerDto, indexer);
        await AddSkills(indexerDto, indexerClient, indexer, keyVaultHelper);
        await indexerClient.CreateOrUpdateIndexerAsync(indexer);
    }

    private static async Task AddSkills(
        IndexerSettingsDto indexerDto,
        SearchIndexerClient indexerClient,
        SearchIndexer indexer,
        IKeyVaultHelper keyVaultHelper)
    {
        if (indexerDto.Skills?.Count != 0 == true && !string.IsNullOrEmpty(indexerDto.AttachedAIServicesKey))
        {
            var skills = new List<SearchIndexerSkill>();
            foreach (var skillDto in indexerDto.Skills!)
            {
                skills.Add(skillDto.Type switch
                {
                    "TextTranslation" => GetTextTranslationSkill(skillDto),
                    "LanguageDetection" => GetLanguageDetectionSkill(skillDto),
                    "Sentiment" => GetSentimentSkill(skillDto),
                    _ => throw new ArgumentException($"Skill {skillDto.Type} not supported")
                });
            }

            var attachedAIServicesKey = await keyVaultHelper.GetSecretAsync(indexerDto.AttachedAIServicesKey!);
            var skillset = new SearchIndexerSkillset($"{indexerDto.IndexerName}-skillset", skills)
            {
                CognitiveServicesAccount = new CognitiveServicesAccountKey(attachedAIServicesKey)
            };
            await indexerClient.CreateOrUpdateSkillsetAsync(skillset);
            indexer.SkillsetName = skillset.Name;
        }
    }

    private static LanguageDetectionSkill GetLanguageDetectionSkill(SkillDto skillDto)
    {
        var languageDetectionSkill = new LanguageDetectionSkill(
            [
                new InputFieldMappingEntry("text")
                {
                    Source = $"/document/{skillDto.Inputs.First()}"
                }
            ],
            [
                new OutputFieldMappingEntry("languageCode")
                {
                    TargetName = skillDto.Outputs.First()
                },
                new OutputFieldMappingEntry("languageName")
                {
                    TargetName = skillDto.Outputs.Last()
                },
            ])
            {
                Name = "LanguageDetectionSkill",
                Description = "Detect the language used in the document"
            };

        return languageDetectionSkill;
    }

    private static TextTranslationSkill GetTextTranslationSkill(SkillDto skillDto)
    {
        return new TextTranslationSkill(
            [
                new InputFieldMappingEntry("text")
                {
                    Source = $"/document/{skillDto.Inputs.First()}"
                }
            ],
            [
                new OutputFieldMappingEntry("translatedText")
                {
                    TargetName = skillDto.Outputs.First()
                }
            ],
            TextTranslationSkillLanguage.Es)
        {
            Name = "TextTranslationSkill",
            Description = "Translate text to Spanish"
        };
    }

    private static SentimentSkill GetSentimentSkill(SkillDto skillDto)
    {
        return new SentimentSkill(
            [
                new InputFieldMappingEntry("text") 
                { 
                    Source = $"/document/{skillDto.Inputs.First()}"
                },
                new InputFieldMappingEntry("languageCode")
                {
                    Source = $"/document/{skillDto.Inputs.Last()}"
                }
            ],
            [
                new OutputFieldMappingEntry("sentiment") 
                {
                    TargetName = skillDto.Outputs.First()
                }
            ],
            SentimentSkill.SkillVersion.V3)
        {
            Name = "SentimentSkill",
            Description = "Detect sentiment in pesonal opinion fields",
            DefaultLanguageCode = "en",
            Context = "/document"
        };
    }

    private static void SetIndexingSchedule(IndexerSettingsDto indexerDto, SearchIndexer indexer)
    {
        if ((indexerDto.IndexingScheduleInMin ?? 0) > 0)
        {
            indexer.Schedule = new IndexingSchedule(TimeSpan.FromMinutes(indexerDto.IndexingScheduleInMin!.Value));
        }
    }
}
