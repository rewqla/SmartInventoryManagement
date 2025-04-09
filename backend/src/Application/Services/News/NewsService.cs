using Application.Interfaces.News;
using Infrastructure.Interfaces;

namespace Application.Services.News;

public class NewsService : INewsService
{
    private readonly INewsApi _newsApi;

    public NewsService(INewsApi newsApi)
    {
        _newsApi = newsApi;
    }

    public async Task<List<string>> GetTopHeadlinesTitlesAsync(string query, CancellationToken cancellationToken = default)
    {
        var articles = await _newsApi.GetTopHeadlinesAsync(query, cancellationToken);
        return articles.Articles.Select(a => a.Title).ToList();
    }
}