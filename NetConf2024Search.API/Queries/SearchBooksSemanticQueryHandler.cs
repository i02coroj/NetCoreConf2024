using Azure.Search.Documents.Models;
using Azure;
using MediatR;
using Microsoft.ApplicationInsights;
using Azure.Search.Documents;
using Azure.Core;
using Azure.Search.Documents.Indexes;
using Microsoft.Extensions.Options;
using NetConf2024Search.API.Dtos;
using NetConf2024Search.API.Helpers;
using NetConf2024Search.API.Model;

namespace NetConf2024Search.API.Queries;

public class SearchBooksSemanticQueryHandler(
    TelemetryClient telemetryClient,
    IKeyVaultHelper keyVaultHelper,
    IOptions<SearchSettingsDto> searchSettings) : IRequestHandler<SearchBooksSemanticQuery, SearchBooksSemanticQueryResponse>
{
    private SearchClient? _searchClient;
    private readonly TelemetryClient _telemetryClient = telemetryClient ?? throw new ArgumentNullException(nameof(telemetryClient));
    private readonly SearchSettingsDto _searchSettings = searchSettings?.Value ?? throw new ArgumentNullException(nameof(searchSettings));
    private readonly IKeyVaultHelper _keyVaultHelper = keyVaultHelper ?? throw new ArgumentNullException(nameof(keyVaultHelper));
    private readonly string _indexName = "books-index";

    public async Task<SearchBooksSemanticQueryResponse> Handle(
        SearchBooksSemanticQuery query,
        CancellationToken cancellationToken)
    {
        await InitialiseSearch();

        var options = new SearchOptions
        {
            IncludeTotalCount = true,
            QueryType = SearchQueryType.Semantic,
            SemanticSearch = new()
            {
                SemanticConfigurationName = $"{_indexName}-semantic-config",
                QueryCaption = new(QueryCaptionType.Extractive),
                QueryAnswer = new QueryAnswer(QueryAnswerType.Extractive)
                {
                    Count = 3
                }
            }
        };

        var results = await _searchClient!.SearchAsync<Book>(query.SearchTerm, options, cancellationToken).ConfigureAwait(false);

        var pagedResults = ((SearchResults<Book>)results).GetResults();
        return new SearchBooksSemanticQueryResponse
        {
            TotalResults = results.Value.TotalCount ?? 0,
            Books = pagedResults.Select(r => new BookSemanticDto
            {
                Id = new Guid(r.Document.Id),
                Title = r.Document.Title,
                AuthorName = r.Document.AuthorName,
                SemanticScore = r.SemanticSearch.RerankerScore,
                Highlights = r.SemanticSearch.Captions.Select(c => c.Highlights)?.ToList(),
                Answers = r.SemanticSearch.Captions.Select(c => c.Text)?.ToList()
            })
        };
    }

    protected async Task InitialiseSearch()
    {
        var searchKey = await _keyVaultHelper.GetSecretAsync(_searchSettings.SearchAdminApiKeySecretName);
        var clientOptions = new SearchClientOptions();
        clientOptions.AddPolicy(new SearchIdPipelinePolicy(), HttpPipelinePosition.PerCall);

        var indexClient = new SearchIndexClient(new Uri(_searchSettings.SearchUri), new AzureKeyCredential(searchKey), clientOptions);
        _searchClient ??= indexClient.GetSearchClient(_indexName);
    }
}