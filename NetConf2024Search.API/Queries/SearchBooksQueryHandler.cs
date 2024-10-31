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

public class SearchBooksQueryHandler(
    TelemetryClient _telemetryClient,
    IKeyVaultHelper _keyVaultHelper,
    IOptions<SearchSettingsDto> searchSettings) : IRequestHandler<SearchBooksQuery, SearchBooksQueryResponse>
{
    private SearchClient? _searchClient;
    private readonly SearchSettingsDto _searchSettings = searchSettings?.Value ?? throw new ArgumentNullException(nameof(searchSettings));
    private readonly string _indexName = "books-index";

    public async Task<SearchBooksQueryResponse> Handle(
        SearchBooksQuery query,
        CancellationToken cancellationToken)
    {
        await InitialiseSearch();

        var options = new SearchOptions
        {
            IncludeTotalCount = true,
            SearchMode = query.SearchMode ?? SearchMode.All,
            Skip = (query.Paging.Number - 1) * query.Paging.Size,
            Size = query.Paging.Size
        };

        SetHighlights(options);
        SetRowLevelSecurity(options);
        SetSorts(options, query);
        SetFacets(options);
        SetPropertyNames(options);

        var results = await _searchClient!.SearchAsync<Book>(query.SearchTerm, options, cancellationToken).ConfigureAwait(false);
        var searchId = LogSearchTelemetry(query.SearchTerm, results);
        var pagedResults = ((SearchResults<Book>)results).GetResults();
        return new SearchBooksQueryResponse
        {
            SearchId = searchId,
            TotalResults = results.Value.TotalCount ?? 0,
            Facets = results.Value.Facets[nameof(Book.Gendre)].ToDictionary(fv => fv.Value.ToString()!, fv => fv.Count ?? 0),
            Books = pagedResults.Select(r => new BookDto
            {
                Id = new Guid(r.Document.Id),
                Title = r.Document.Title,
                Reference = r.Document.Reference,
                Summary = r.Document.Summary,
                Gendre = r.Document.Gendre,
                InStock = r.Document.InStock,
                NumberOfPages = r.Document.NumberOfPages,
                Price = r.Document.Price,
                PublishedOn = r.Document.PublishedOn,
                AuthorName = r.Document.AuthorName,
                AuthorBio = r.Document.AuthorBio,
                LanguagesRaw = r.Document.LanguagesRaw,
                MostRecentComment = r.Document.MostRecentComment,
                NumberOfComments = r.Document.NumberOfComments,
                Score = r.Score,
                Highlights = r.Highlights
            })
        };
    }

    private static void SetHighlights(SearchOptions options)
    {
        options.HighlightPreTag = "<b>";
        options.HighlightPostTag = "</b>";

        options.HighlightFields.Add(nameof(Book.Title));
        options.HighlightFields.Add(nameof(Book.Reference));
        options.HighlightFields.Add(nameof(Book.Summary));
        options.HighlightFields.Add(nameof(Book.Gendre));
        options.HighlightFields.Add(nameof(Book.AuthorFirstName));
        options.HighlightFields.Add(nameof(Book.AuthorLastName));
        options.HighlightFields.Add(nameof(Book.AuthorBio));
    }

    protected async Task InitialiseSearch()
    {
        var searchKey = await _keyVaultHelper.GetSecretAsync(_searchSettings.SearchAdminApiKeySecretName);
        var clientOptions = new SearchClientOptions();
        clientOptions.AddPolicy(new SearchIdPipelinePolicy(), HttpPipelinePosition.PerCall);

        var indexClient = new SearchIndexClient(new Uri(_searchSettings.SearchUri), new AzureKeyCredential(searchKey), clientOptions);
        _searchClient ??= indexClient.GetSearchClient(_indexName);
    }

    private static void SetFacets(SearchOptions options)
    {
        options.Facets.Add($"{nameof(Book.Gendre)},count:20");
    }

    /// <summary>
    /// Enter property names into this list so only these values will be returned.
    /// If Select is empty, all values will be returned, which can be inefficient.
    /// </summary>
    /// <param name="options">Search options</param>
    private static void SetPropertyNames(SearchOptions options)
    {
        options.Select.Add(nameof(Book.Id));
        options.Select.Add(nameof(Book.Title));
        options.Select.Add(nameof(Book.Reference));
        options.Select.Add(nameof(Book.Summary));
        options.Select.Add(nameof(Book.Gendre));
        options.Select.Add(nameof(Book.AuthorFirstName));
        options.Select.Add(nameof(Book.AuthorLastName));
        options.Select.Add(nameof(Book.AuthorBio));
        options.Select.Add(nameof(Book.InStock));
        options.Select.Add(nameof(Book.PublishedOn));
        options.Select.Add(nameof(Book.Price));
        options.Select.Add(nameof(Book.NumberOfPages));
        options.Select.Add($"{nameof(Book.Languages)}/{nameof(Language.LanguageName)}");
        options.Select.Add($"{nameof(Book.Comments)}/{nameof(Comment.Text)}");
        options.Select.Add($"{nameof(Book.Comments)}/{nameof(Comment.Author)}");
        options.Select.Add($"{nameof(Book.Comments)}/{nameof(Comment.PublishedOn)}");
    }

    /// <summary>
    /// Check user details and set row level security filter
    /// </summary>
    /// <param name="options">Search options</param>
    private static void SetRowLevelSecurity(SearchOptions options)
    {
        // Check user details (not possible in this demo as endpoint is annoymous) and set row level security filter
        var userLanguages = new[] { "English" };
        var filterByLanguages = $"{nameof(Book.Languages)}/any(language: search.in(language/{nameof(Language.LanguageName)}, '{string.Join(',', userLanguages)}'))";
        options.Filter = filterByLanguages;
    }

    private static void SetSorts(
        SearchOptions options,
        SearchBooksQuery query)
    {
        if (query.Sorts?.Any() == true)
        {
            // Set sorts
        }
        else
        {
            options.OrderBy.Add($"{nameof(Book.PublishedOn)} desc");
        }
    }

    private string? LogSearchTelemetry<T>(
        string searchText,
        Response<SearchResults<T>> results)
    {
        var searchId = results.GetRawResponse().Headers.TryGetValues("x-ms-azs-searchid", out var headerValues) ? headerValues.FirstOrDefault() : null;
        var properties = new Dictionary<string, string>
        {
            {"SearchServiceName", _searchClient!.ServiceName},
            {"SearchId", searchId!},
            {"IndexName", _indexName},
            {"QueryTerms", searchText},
            {"ResultCount", (results.Value.TotalCount ?? 0).ToString()}
        };

        _telemetryClient.TrackEvent("Search", properties);

        return searchId;
    }
}