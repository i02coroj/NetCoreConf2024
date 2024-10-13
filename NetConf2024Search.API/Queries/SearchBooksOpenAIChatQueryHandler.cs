#pragma warning disable AOAI001

using MediatR;
using Microsoft.Extensions.Options;
using NetConf2024Search.API.Dtos;
using NetConf2024Search.API.Helpers;
using Azure.AI.OpenAI;
using System.ClientModel;
using OpenAI.Chat;
using Azure.AI.OpenAI.Chat;
using Azure.Search.Documents.Indexes.Models;

namespace NetConf2024Search.API.Queries;

public class SearchBooksOpenAIChatQueryHandler(
    IKeyVaultHelper keyVaultHelper,
    IOptions<SearchSettingsDto> searchSettings,
    IOptions<OpenAISettingsDto> openAISettingsDto) : IRequestHandler<SearchBooksOpenAIChatQuery, SearchBooksOpenAIChatQueryResponse>
{
    private readonly SearchSettingsDto _searchSettings = searchSettings?.Value ?? throw new ArgumentNullException(nameof(searchSettings));
    private readonly OpenAISettingsDto _openAISettings = openAISettingsDto?.Value ?? throw new ArgumentNullException(nameof(openAISettingsDto));
    private readonly IKeyVaultHelper _keyVaultHelper = keyVaultHelper ?? throw new ArgumentNullException(nameof(keyVaultHelper));
    private readonly string _indexName = "books-index";

    public async Task<SearchBooksOpenAIChatQueryResponse> Handle(
        SearchBooksOpenAIChatQuery query,
        CancellationToken cancellationToken)
    {
        // Azure OpenAI setup
        var apiBase = _openAISettings.OpenAIUri;
        var openAIKey = await _keyVaultHelper.GetSecretAsync(_openAISettings.OpenAIKeySecretName);
        var deploymentName = "gpt-35-turbo";

        // Azure AI Search setup
        var searchKey = await _keyVaultHelper.GetSecretAsync(_searchSettings.SearchAdminApiKeySecretName);

        var client = new AzureOpenAIClient(new Uri(apiBase), new ApiKeyCredential(openAIKey!));
        var chatClient = client.GetChatClient(deploymentName);

        var options = new ChatCompletionOptions
        {
            MaxOutputTokenCount = 800
        };

        options.AddDataSource(new AzureSearchChatDataSource
        {
            Endpoint = new Uri(_searchSettings.SearchUri),
            IndexName = _indexName,
            Authentication = DataSourceAuthentication.FromApiKey(searchKey),
            QueryType = DataSourceQueryType.Semantic,
            SemanticConfiguration = $"{_indexName}-semantic-config"
        });

        var messages = new List<ChatMessage>
        {
            new UserChatMessage(query.SearchTerm)
        };


        var completion = await chatClient.CompleteChatAsync(messages, options, cancellationToken);
        var message  = completion.Value.GetMessageContext();
        return new SearchBooksOpenAIChatQueryResponse
        {
            MessageContent = $"{completion.Value.Role}: {completion.Value.Content[0].Text}",
            Citations = message.Citations.Select(citation => $"{citation.Title}: {citation.Content}")?.ToList()
        };
    }
}