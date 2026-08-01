using Microsoft.Extensions.Logging;
using Shortly.Application.DTOs;
using Shortly.Application.Interfaces;

namespace Shortly.Application.Queries.Link;

public class GetLinksByUserIdQueryHandler
{
    private readonly ILinkReadRepository _repository;
    private readonly ILogger<GetLinksByUserIdQueryHandler> _logger;

    public GetLinksByUserIdQueryHandler(ILinkReadRepository repository, ILogger<GetLinksByUserIdQueryHandler> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<List<LinkResponse>> Handle(GetLinksByUserIdQuery query)
    {
        _logger.LogDebug("Getting links for userId: {UserId}", query.UserId);

        var links = await _repository.GetByUserIdAsync(query.UserId);

        _logger.LogInformation("Retrieved {Count} links for userId: {UserId}.", links.Count, query.UserId);
        return links.Select(LinkResponse.From).ToList();
    }
}