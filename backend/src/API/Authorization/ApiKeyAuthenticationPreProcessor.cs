using FastEndpoints;

namespace API.Authorization;

public sealed class ApiKeyAuthenticationPreProcessor : IPreProcessor<EmptyRequest>
{
    private const string ApiKeyHeaderName = "ApiKey";
    private readonly IConfiguration _configuration;

    public ApiKeyAuthenticationPreProcessor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task PreProcessAsync(IPreProcessorContext<EmptyRequest> context, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(context);

        var apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(apiKey) || apiKey != _configuration.GetValue<string>("ApiKey"))
        {
            return context.HttpContext.Response.SendUnauthorizedAsync(ct);
        }

        return Task.CompletedTask;
    }
}