using Microsoft.Extensions.Logging;
using Shortly.Application.DTOs;
using Shortly.Application.Interfaces;

namespace Shortly.Application.Queries.Link;

public class GetAllLinksQueryHandler
{
    private readonly ILinkReadRepository _repository;
    private readonly ILogger<GetAllLinksQueryHandler> _logger;

    public GetAllLinksQueryHandler(ILinkReadRepository repository, ILogger<GetAllLinksQueryHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<LinkResponse>> Handle(GetAllLinksQuery query)
    {
        _logger.LogDebug("Getting all links");

        var links = await _repository.GetAllAsync();

        _logger.LogInformation("Retrieved {Count} links.", links.Count);
        return links.Select(LinkResponse.From).ToList();
    }
}