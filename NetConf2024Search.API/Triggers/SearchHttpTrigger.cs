using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;
using NetConf2024Search.API.Commands;
using NetConf2024Search.API.Queries;
using System.Net;

namespace NetConf2024Search.API.Triggers;

public class SearchHttpTrigger(IMediator mediator)
{
    private readonly IMediator _mediator = mediator;

    [Function(nameof(RunSearchBooksHttpTrigger))]
    [OpenApiOperation(operationId: nameof(RunSearchBooksHttpTrigger))]
    [OpenApiRequestBody(nameof(SearchBooksQuery), typeof(SearchBooksQuery))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 200 response with search results")]
    public async Task<HttpResponseData> RunSearchBooksHttpTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "search/books")] HttpRequestData req)
    {
        var query = await req.ReadFromJsonAsync<SearchBooksQuery>();

        var queryResponse = await _mediator.Send(query!);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(queryResponse, "application/json");
        return response;
    }

    [Function(nameof(RunSearchBooksSemanticHttpTrigger))]
    [OpenApiOperation(operationId: nameof(RunSearchBooksSemanticHttpTrigger))]
    [OpenApiRequestBody(nameof(SearchBooksQuery), typeof(SearchBooksSemanticQuery))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 200 response with semantic search results")]
    public async Task<HttpResponseData> RunSearchBooksSemanticHttpTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "search/books/semantic")] HttpRequestData req)
    {
        var query = await req.ReadFromJsonAsync<SearchBooksSemanticQuery>();

        var queryResponse = await _mediator.Send(query!);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(queryResponse, "application/json");
        return response;
    }

    [Function(nameof(RunSearchBooksOpenAIChatHttpTrigger))]
    [OpenApiOperation(operationId: nameof(RunSearchBooksOpenAIChatHttpTrigger))]
    [OpenApiRequestBody(nameof(SearchBooksQuery), typeof(SearchBooksOpenAIChatQuery))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 200 response with OpenAIChat search results")]
    public async Task<HttpResponseData> RunSearchBooksOpenAIChatHttpTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "search/books/OpenAIChat")] HttpRequestData req)
    {
        var query = await req.ReadFromJsonAsync<SearchBooksOpenAIChatQuery>();

        var queryResponse = await _mediator.Send(query!);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(queryResponse, "application/json");
        return response;
    }

    [Function(nameof(LogClickHttpTrigger))]
    [OpenApiOperation(operationId: nameof(LogClickHttpTrigger))]
    [OpenApiRequestBody(nameof(LogClickCommand), typeof(LogClickCommand))]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(string), Description = "Returns a 200 response with search results")]
    public async Task<HttpResponseData> LogClickHttpTrigger(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "search/books/log")] HttpRequestData req)
    {
        var command = await req.ReadFromJsonAsync<LogClickCommand>();

        _ = await _mediator.Send(command!);

        var response = req.CreateResponse(HttpStatusCode.OK);
        return response;
    }
}