namespace Application.Interfaces.News;

public interface INewsService
{
    Task<List<string>> GetTopHeadlinesTitlesAsync(string query, CancellationToken cancellationToken = default);
}