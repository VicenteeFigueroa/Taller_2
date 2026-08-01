using Microsoft.Extensions.Logging;
using Shortly.Application.DTOs;
using Shortly.Application.Interfaces;

namespace Shortly.Application.Queries.Link;

public class GetLinkQueryHandler
{
    private readonly ILinkReadRepository _repository;
    private readonly ILogger<GetLinkQueryHandler> _logger;

    public GetLinkQueryHandler(ILinkReadRepository repository, ILogger<GetLinkQueryHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<LinkResponse> Handle(GetLinkQuery query)
    {
        _logger.LogDebug("Getting link with shortUrl: {ShortUrl}", query.ShortUrl);

        var link = await _repository.GetByShortUrlAsync(query.ShortUrl);
        if (link is null)
        {
            _logger.LogWarning("GetLink failed: No link found with shortUrl {ShortUrl}.", query.ShortUrl);
            throw new KeyNotFoundException($"No link found with shortUrl '{query.ShortUrl}'.");
        }

        _logger.LogInformation("Link retrieved successfully with shortUrl: {ShortUrl}.", link.ShortUrl);
        return LinkResponse.From(link);
    }
}